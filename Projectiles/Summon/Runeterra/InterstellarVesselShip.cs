using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Items.Weapons.Summon.Runeterra;
using tsorcRevamp.Buffs.Runeterra;
using tsorcRevamp.Projectiles.Trails;
using Microsoft.Xna.Framework.Graphics;

namespace tsorcRevamp.Projectiles.Summon.Runeterra
{
	public class InterstellarVesselShip : ModProjectile
	{
		public float angularSpeed2 = 0.03f;
		public static float circleRad2 = 50f;
		public float currentAngle2 = 0;
		bool spawnedTrail = false;

        public override void SetStaticDefaults()
		{
			Main.projPet[Projectile.type] = true; 
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; 
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 98;
			Projectile.height = 54;
			Projectile.tileCollide = false;

			Projectile.friendly = true; 
			Projectile.minion = true;
			Projectile.DamageType = DamageClass.Summon; 
			Projectile.minionSlots = 1f; 
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 1;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player owner = Main.player[Projectile.owner];
            if (owner.GetModPlayer<tsorcRevampPlayer>().InterstellarBoost)
            {
                damage += (Projectile.damage / 4);
            }
        }
        public override void OnSpawn(IEntitySource source) 
		{
			InterstellarVesselControls.projectiles.Add(this);
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public override void Kill(int timeLeft) 
		{
			InterstellarVesselControls.projectiles.Remove(this);
		}
		Vector2 lastPos;
		InterstellarVesselTrail trail;
		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];

			Vector2 visualplayercenter = owner.Center + new Vector2(-27, -12);

			if (!CheckActive(owner))
			{
				return;
            }

            if (owner.GetModPlayer<tsorcRevampPlayer>().InterstellarBoost)
            {
                angularSpeed2 = 0.075f;
				owner.statMana -= 1;
				owner.manaRegenDelay = 10;
            }
            if (!owner.GetModPlayer<tsorcRevampPlayer>().InterstellarBoost || (owner.statMana <= 0) || owner.HasBuff(BuffID.ManaSickness))
            {
                angularSpeed2 = 0.03f;
				owner.GetModPlayer<tsorcRevampPlayer>().InterstellarBoost = false;
            }

            currentAngle2 += (angularSpeed2 / (circleRad2 * 0.001f + 1f)); 

			Vector2 offset = new Vector2(MathF.Sin(currentAngle2), MathF.Cos(currentAngle2)) * circleRad2;

			Projectile.position = visualplayercenter + offset;            
			if (!spawnedTrail)
			{
				trail = (InterstellarVesselTrail)Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<InterstellarVesselTrail>(), 0, 0, Projectile.owner, 0, Projectile.whoAmI).ModProjectile;
				spawnedTrail = true;
			}

            Visuals();
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float distance = Vector2.Distance(projHitbox.Center.ToVector2(), targetHitbox.Center.ToVector2());
			if (distance < Projectile.height * 1.2f && distance > Projectile.height * 1.2f - 32)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
			{
				owner.ClearBuff(ModContent.BuffType<InterstellarCommander>());

				return false;
			}

			if (!owner.HasBuff(ModContent.BuffType<InterstellarCommander>()))
			{
				circleRad2 = 50f;
				currentAngle2 = 0;
				InterstellarVesselControls.projectiles.Clear();
			}

			if (owner.HasBuff(ModContent.BuffType<InterstellarCommander>()))
			{
				Projectile.timeLeft = 2;
			}

			return true;
		}
		private void Visuals()
		{

		Projectile.rotation = currentAngle2 * -1f;

		//float frameSpeed = 3f;

		//Projectile.frameCounter++;

		/*if (Projectile.frameCounter >= frameSpeed)
		{
			Projectile.frameCounter = 0;
			Projectile.frame++;

			if (Projectile.frame >= Main.projFrames[Projectile.type])
			{
					Projectile.frame = 0;
			}
		}*/

		Lighting.AddLight(Projectile.Center, Color.Gold.ToVector3() * 0.48f);
		}

		public static Texture2D texture;
		public static Texture2D glowTexture;
		public override bool PreDraw(ref Color lightColor)
        {
			//if (texture == null || texture.IsDisposed)
			{
				texture = (Texture2D)ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad);
			}
			//if (glowTexture == null || glowTexture.IsDisposed)
			{
				glowTexture = (Texture2D)ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "Glowmask", ReLogic.Content.AssetRequestMode.ImmediateLoad);
			}

			Rectangle sourceRectangle = new Rectangle(0,0, texture.Width, texture.Height);
			Vector2 origin = sourceRectangle.Size() / 2f;

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, sourceRectangle, Color.Lerp(lightColor, Color.Orange, 0.25f), Projectile.rotation, origin, 1, SpriteEffects.None, 0f);

			Main.spriteBatch.Draw(glowTexture, Projectile.Center - Main.screenPosition, sourceRectangle, Color.White, Projectile.rotation, origin, 1, SpriteEffects.None, 0f);

			return false;
		}
    }	
}