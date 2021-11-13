﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Projectiles.Enemy.DarkCloud
{
    class DarkAntiMatRound : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = false;
			projectile.hostile = true;
            projectile.aiStyle = 0;
            projectile.ranged = true;
            projectile.tileCollide = false;
			projectile.penetrate = 5;
			//In theory this means the projectile can only ever hit a NPC once.
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anti-Material Round");
		}

		bool reposition = true;
		float sinwaveCounter = -1.4f;
		public override void AI()
		{
			if (reposition)
			{
				projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

				projectile.velocity.X *= 0.45f;
				projectile.velocity.Y *= 0.45f;

				reposition = false;
				projectile.alpha = 255;
			}
			projectile.alpha -= 51;

			for (int i = 0; i < 10; i++)
			{			

				//dust fx
				if (i % 2 == 0)
				{
					if (sinwaveCounter > 0)
					{//tracer
						Vector2 DustPos = projectile.position;

						int DustIndex = Dust.NewDust(DustPos, 0, 0, 6, 0, 0, 100, default(Color), 1.7f);
						Main.dust[DustIndex].noGravity = true;
						Main.dust[DustIndex].velocity = new Vector2(0, 0);
					}
					else
					{//fire effect
						Vector2 DustPos = projectile.position;
						int DustWidth = projectile.width;
						int DustHeight = projectile.height;

						Dust.NewDust(DustPos, DustWidth, DustHeight, 6, 0, 0, 100, default(Color), 1.1f);
						int DustIndex = Dust.NewDust(DustPos, DustWidth, DustHeight, 31, 0, 0, 100, default(Color), 3f);
						Main.dust[DustIndex].noGravity = true;
					}
				}
				if (sinwaveCounter > 0)
				{
					//sine wave top
					Vector2 DustTopPos = projectile.position +
						new Vector2((float)(15 * Math.Cos(sinwaveCounter) * Math.Cos(projectile.rotation)),
						(float)(15 * Math.Sin(sinwaveCounter) * Math.Sin(projectile.rotation)));

					int sineTop = Dust.NewDust(DustTopPos, 0, 0, 60, 0, 0, 100, default(Color), 1f);
					Main.dust[sineTop].noGravity = true;
					Main.dust[sineTop].velocity = new Vector2(0, 0);
					//sine wave bottom
					Vector2 DustBotPos = projectile.position +
						new Vector2((float)(-15 * Math.Cos(sinwaveCounter) * Math.Cos(projectile.rotation)),
						(float)(-15 * Math.Sin(sinwaveCounter) * Math.Sin(projectile.rotation)));

					int sineBot = Dust.NewDust(DustBotPos, 0, 0, 60, 0, 0, 100, default(Color), 1f);
					Main.dust[sineBot].noGravity = true;
					Main.dust[sineBot].velocity = new Vector2(0, 0);
				}
				sinwaveCounter += 0.2f;				

				projectile.position += projectile.velocity;
			}

			Lighting.AddLight((int)((projectile.position.X + (float)(projectile.width / 2)) / 16f), (int)((projectile.position.Y + (float)(projectile.height / 2)) / 16f), 0.9f, 0.2f, 0.1f);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			//Get the premultiplied, properly transparent texture
			Texture2D texture = TransparentTextureHandler.TransparentTextures[TransparentTextureHandler.TransparentTextureType.AntiMaterialRound];
			int frameHeight = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
			int startY = frameHeight * projectile.frame;
			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin = sourceRectangle.Size() / 2f;
			//origin.X = (float)(projectile.spriteDirection == 1 ? sourceRectangle.Width - 20 : 20);
			Color drawColor = projectile.GetAlpha(lightColor);
			Main.spriteBatch.Draw(texture,
				projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY),
				sourceRectangle, drawColor, projectile.rotation, origin, projectile.scale, spriteEffects, 0f);

			return false;
		}

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 43);
			damage = target.defense + projectile.damage;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
		}

        public override bool PreKill(int timeLeft)
        {
			for (int num36 = 0; num36 < 10; num36++)
			{
				Dust.NewDustPerfect(projectile.position, 127, projectile.velocity + new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5)), 100, new Color(), 1f).noGravity = true;
			}
			for (int num36 = 0; num36 < 7; num36++)
			{
				Dust.NewDustPerfect(projectile.position, 130, projectile.velocity + new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5)), 100, new Color(), 2f).noGravity = true;
			}
			return base.PreKill(timeLeft);
        }
	}
}