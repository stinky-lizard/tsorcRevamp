﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace tsorcRevamp.Projectiles.Pets
{
    class MiakodaFull : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Full Moon Miakoda");
            Main.projFrames[projectile.type] = 8;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyHornet);
            projectile.width = 18;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.tileCollide = false;
            aiType = ProjectileID.BabyHornet;
            projectile.scale = 1f;
            projectile.scale = 0.85f;
            drawOffsetX = -8;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.hornet = false;

            return true;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            tsorcRevampPlayer modPlayer = player.GetModPlayer<tsorcRevampPlayer>();
            float MiakodaVol = ModContent.GetInstance<tsorcRevampConfig>().MiakodaVolume / 100f;
            Lighting.AddLight(projectile.position, .6f, .6f, .4f);

            Vector2 idlePosition = player.Center;
            Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();

            if (player.dead)
            {
                modPlayer.MiakodaFull = false;
            }
            if (modPlayer.MiakodaFull)
            {
                projectile.timeLeft = 2;
            }

            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1500f)
            { 
                projectile.position = idlePosition;
                projectile.velocity *= 0.1f;
                projectile.netUpdate = true;
            }

            if ((modPlayer.MiakodaEffectsTimer > 720) && (Main.rand.Next(3) == 0))
            {
                if (projectile.direction == 1)
                {
                    int dust = Dust.NewDust(new Vector2(projectile.position.X + 4, projectile.position.Y), projectile.width - 6, projectile.height - 6, 57, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 30, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                }
                if (projectile.direction == -1)
                {
                    int dust = Dust.NewDust(new Vector2(projectile.position.X - 4, projectile.position.Y), projectile.width - 6, projectile.height - 6, 57, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 30, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                }
            }
            
            if (modPlayer.MiakodaEffectsTimer == 720 && MiakodaVol != 0) //sound effect the moment the timer reaches 420, to signal pet ability ready.
            {
                string[] ReadySoundChoices = new string[] { "Sounds/Custom/MiakodaChaaa", "Sounds/Custom/MiakodaChao", "Sounds/Custom/MiakodaDootdoot", "Sounds/Custom/MiakodaHi", "Sounds/Custom/MiakodaOuuee" };
                string ReadySound = Main.rand.Next(ReadySoundChoices);
                if (!Main.dedServ)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, ReadySound).WithVolume(.4f * MiakodaVol).WithPitchVariance(.2f), projectile.Center);
                }
            }

            if (modPlayer.MiakodaFullHeal2) //splash effect and sound once player gets crit+heal.
            {
                if (MiakodaVol != 0)
                {
                    string[] AmgerySoundChoices = new string[] { "Sounds/Custom/MiakodaScream", "Sounds/Custom/MiakodaChaoExcl", "Sounds/Custom/MiakodaUwuu" };
                    string AmgerySound = Main.rand.Next(AmgerySoundChoices);
                    if (!Main.dedServ)
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, AmgerySound).WithVolume(.6f * MiakodaVol).WithPitchVariance(.2f), projectile.Center);
                    }
                }

                for (int d = 0; d < 90; d++)
                {
                    if (projectile.direction == 1)
                    {
                        int dust = Dust.NewDust(new Vector2(projectile.position.X - 4, projectile.position.Y), projectile.width - 6, projectile.height - 6, 57, 0f, 0f, 30, default(Color), 1.5f);
                        Main.dust[dust].velocity *= Main.rand.NextFloat(0.5f, 3.5f);
                        Main.dust[dust].noGravity = true;
                    }
                    if (projectile.direction == -1)
                    {
                        int dust = Dust.NewDust(new Vector2(projectile.position.X + 4, projectile.position.Y), projectile.width - 6, projectile.height - 6, 57, 0f, 0f, 30, default(Color), 1.5f);
                        Main.dust[dust].velocity *= Main.rand.NextFloat(0.5f, 3.5f);
                        Main.dust[dust].noGravity = true;
                    }
                }

                if (modPlayer.MiakodaEffectsTimer < 720)
                {
                    player.GetModPlayer<tsorcRevampPlayer>().MiakodaFullHeal2 = false;
                }
            }
        }
    }
}
