﻿
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Projectiles
{
    class Barrier : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barrier");
        }
        public override void SetDefaults()
        {
            drawHeldProjInFrontOfHeldItemAndArms = true; // Makes projectile appear in front of arms, not just in between body and arms
            projectile.friendly = true;
            projectile.width = 48;
            projectile.height = 62;
            projectile.penetrate = -1;
            projectile.scale = 1;
            projectile.tileCollide = false;
            projectile.timeLeft = 2;
            projectile.alpha = 160;
        }
        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                var player = Main.player[projectile.owner];

                if (player.dead)
                {
                    projectile.Kill();
                    return;
                }

                if (Main.rand.Next(3) == 0)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 156, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 30, default(Color), .6f);
                    Main.dust[dust].noGravity = true;
                }


                Player projOwner = Main.player[projectile.owner];
                projOwner.heldProj = projectile.whoAmI; //this makes it appear in front of the player
                projectile.velocity.X = player.velocity.X;
                projectile.velocity.Y = player.velocity.Y;
                //projectile.position.X = player.position.X - (float)(player.width / 2);
                //projectile.position.Y = player.position.Y - (float)(player.height / 2);
            }
            //Barrier now has a second mode used exclusively by Attraidies, that occurs when its ai[0] is set to 1. It checks if he exists, and if not then dies.
            else
            {
                projectile.timeLeft = 5;
                if (!NPC.AnyNPCs(ModContent.NPCType<NPCs.Special.AttraidiesApparition>()))
                {
                    projectile.Kill();
                }
                else
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 156, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 30, default(Color), .6f);
                        Main.dust[dust].noGravity = true;
                    }
                }                
            }
        }
        public override bool CanDamage()
        {
            return false;
        }
    }
}