﻿using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.NPCs.Enemies
{
    class Archdeacon : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.npcSlots = 5;
            //npc.maxSpawns = 2; todo investigate
            NPC.aiStyle = 0;
            NPC.damage = 70;
            NPC.defense = 8;
            NPC.height = 44;
            NPC.timeLeft = 22500;
            NPC.lifeMax = 150;
            NPC.scale = 1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.lavaImmune = true;
            NPC.value = 1500;
            NPC.width = 28;
            NPC.knockBackResist = 0.2f;
            Main.npcFrameCount[NPC.type] = 3;
            AnimationType = NPCID.GoblinSorcerer;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Confused] = true;
            Banner = NPC.type;
            //BannerItem = ModContent.ItemType<Banners.Archdeacon>();
        }

       

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float chance = 0;
            if (spawnInfo.Player.ZoneSkyHeight && spawnInfo.Player.ZoneSnow && Main.hardMode && NPC.CountNPCS(ModContent.NPCType<NPCs.Enemies.AttraidiesManifestation>()) < 1 && NPC.CountNPCS(ModContent.NPCType<NPCs.Enemies.AttraidiesIllusion>()) < 1
               && NPC.CountNPCS(ModContent.NPCType<NPCs.Enemies.BarrowWight>()) < 2)
            {
                chance += 0.03f;
            }
            return chance;
        }

        public override void AI()
        {

            NPC.netUpdate = false;
            NPC.ai[0]++; // Timer Scythe
            NPC.ai[1]++; // Timer Teleport
                         // npc.ai[2]++; // Shots


            bool validTarget = Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height);

            
            if (NPC.life > 120)
            {
                Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 2f); //Pick a color, any color. The 0.5f tones down its intensity by 50%
                int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 6, NPC.velocity.X, NPC.velocity.Y, 200, Color.LightCyan, 1f);
                Main.dust[dust].noGravity = true;
            }
            else if (NPC.life <= 120)
            {
                int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 54, NPC.velocity.X, NPC.velocity.Y, 140, Color.DarkCyan, 2f);
                Main.dust[dust].noGravity = true;
            }

            //Bubble Attack & Hold Attack
            if (Main.netMode != NetmodeID.Server && NPC.ai[1] >= 60)
            {
                int choice = Main.rand.Next(6);

                if (choice >= 1)
                {
                    if (NPC.ai[0] >= 12 && NPC.ai[2] < 3 && Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > 220) //22 was 12
                    {
                        float num48 = 3f;
                        Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                        int damage = 22;
                        int type = ModContent.ProjectileType<Projectiles.Enemy.Bubble>();
                        float rotation = (float)Math.Atan2(vector8.Y - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), vector8.X - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
                        int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * num48) * -1), (float)((Math.Sin(rotation) * num48) * -1), type, damage, 0f, Main.myPlayer);
                        Main.projectile[proj].timeLeft = 170;

                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item20 with { Volume = 0.5f, PitchVariance = 2f }, NPC.Center);
                        NPC.ai[0] = 0;
                        NPC.ai[2]++;
                    }
                }
                if (choice == 0)
                {
                        if (NPC.ai[0] >= 12 && NPC.ai[2] < 1 && Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > 200) //22 was 12
                        {
                            float num48 = 3f;
                            Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                            int damage = 22;
                            int type = ModContent.ProjectileType<Projectiles.Enemy.EnemySpellHoldBall>(); //EnemyIceBallUp
                        float rotation = (float)Math.Atan2(vector8.Y - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), vector8.X - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
                            int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * num48) * -1), (float)((Math.Sin(rotation) * num48) * -1), type, damage, 0f, Main.myPlayer);
                            Main.projectile[proj].timeLeft = 420;

                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item20 with { Volume = 0.5f, PitchVariance = 2f }, NPC.Center);
                            NPC.ai[0] = 0;
                            NPC.ai[2]++;


                        }
                        
                }
   
                
            }

            if (NPC.ai[1] >= 20)
            {
                NPC.velocity.X *= 0.17f;
                NPC.velocity.Y *= 0.03f;//was.17
            }

            if ((NPC.ai[1] >= 270 && NPC.life > 150) || (NPC.ai[1] >= 190 && NPC.life <= 150))//was 260 and 180
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                for (int num36 = 0; num36 < 10; num36++)
                {
                    int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 54, NPC.velocity.X + Main.rand.Next(-10, 10), NPC.velocity.Y + Main.rand.Next(-10, 10), 200, Color.Red, 2f);
                    Main.dust[dust].noGravity = false;
                }
                NPC.ai[3] = (float)(Main.rand.Next(360) * (Math.PI / 180));
                NPC.ai[2] = 0;
                NPC.ai[1] = 0;
                if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                {
                    NPC.TargetClosest(true);
                }
                if (Main.player[NPC.target].dead)
                {
                    NPC.position.X = 0;
                    NPC.position.Y = 0;
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 0;
                        return;
                    }
                }
                else
                {
                    //WyvernMageTeleport();
                    tsorcRevampAIs.Teleport(NPC, 38, true);






                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, NPC.Center);





                    //old teleportation code, couldn't get the enemy to reliably spawn above player, hovering in the air most of the time
                    /*
                     * 
                     * for (int i = 0; i < 10; i++)
                    {
                        int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 27, NPC.velocity.X + Main.rand.Next(-10, 10), NPC.velocity.Y + Main.rand.Next(-10, 10), 200, Color.Purple, 1f);
                        Main.dust[dust].noGravity = true;
                    }



                    //region teleportation - can't believe I got this to work.. yayyyyy :D lol

                    int target_x_blockpos = (int)Main.player[NPC.target].position.X / 16; // corner not center
                    int target_y_blockpos = (int)Main.player[NPC.target].position.Y / 16; // corner not center
                    int x_blockpos = (int)NPC.position.X / 16; // corner not center
                    int y_blockpos = (int)NPC.position.Y / 16; // corner not center
                    int tp_radius = 40; // radius around target(upper left corner) in blocks to teleport into
                    int tp_counter = 0;
                    bool flag7 = false;
                    if (Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) + Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 9000000f)
                    { // far away from target; 4000 pixels = 250 blocks
                        tp_counter = 100;
                        flag7 = false; // always teleport, use true for no teleport when far away
                    }
                    while (!flag7) // loop always ran full 100 time before I added "flag7 = true" below
                    {
                        if (tp_counter >= 100) // run 100 times
                            break; //return;
                        tp_counter++;

                        int tp_x_target = Main.rand.Next(target_x_blockpos - tp_radius, target_x_blockpos + tp_radius);  //  pick random tp point (centered on corner)
                        int tp_y_target = Main.rand.Next(target_y_blockpos - tp_radius, target_y_blockpos + tp_radius);  //  pick random tp point (centered on corner)
                        for (int m = tp_y_target; m < target_y_blockpos + tp_radius; m++) // traverse y downward to edge of radius
                        { // (tp_x_target,m) is block under its feet I think
                            if ((m < target_y_blockpos - 19  || tp_x_target < target_x_blockpos - 19 || tp_x_target > target_x_blockpos + 19) && (m < y_blockpos - 4 || m > y_blockpos + 4 || tp_x_target < x_blockpos - 4 || tp_x_target > x_blockpos + 4))//&& Main.tile[tp_x_target, m].HasTile //|| m > target_y_blockpos + 0
                            { // over 13 blocks distant from player & over 4 block distant from old position & tile active(to avoid surface? want to tp onto a block?)
                                bool safe_to_stand = true;
                                bool dark_caster = false; // not a fighter type AI...
                                if (dark_caster && Main.tile[tp_x_target, m - 1].WallType == 0) // Dark Caster & ?outdoors
                                    safe_to_stand = false;
                                else if (Main.tile[tp_x_target, m - 1].LiquidType == LiquidID.Lava) // feet submerged in lava
                                    safe_to_stand = false;

                                if (!Collision.SolidTiles(tp_x_target - 1, tp_x_target + 1, m - 4, m - 1))//safe_to_stand && Main.tileSolid[(int)Main.tile[tp_x_target, m].TileType] &&
                                { // safe enviornment & solid below feet & 3x4 tile region is clear; (tp_x_target,m) is below bottom middle tile

                                    NPC.TargetClosest(true);
                                    NPC.position.X = (float)(tp_x_target * 16 - NPC.width / 2); // center x at target
                                    NPC.position.Y = (float)(m * 16 - NPC.height); // y so block is under feet
                                    Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                                    float rotation = (float)Math.Atan2(vector8.Y - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), vector8.X - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
                                    NPC.velocity.X = (float)(Math.Cos(rotation) * 4) * -1;
                                    NPC.velocity.Y = (float)(Math.Sin(rotation) * 4) * -1;


                                    // npc.position.X = (float)(tp_x_target * 16 - npc.width / 2); // center x at target
                                    // npc.position.Y = (float)(m * 16 - npc.height); // y so block is under feet
                                    NPC.netUpdate = true;

                                    //npc.ai[3] = -120f; // -120 boredom is signal to display effects & reset boredom next tick in section "teleportation particle effects"
                                    flag7 = true; // end the loop (after testing every lower point :/)
                                    NPC.ai[1] = 0;
                                }
                            } // END over 4 blocks distant from player...
                        } // END traverse y down to edge of radius
                    } // END try 100 times
                    */

                }
            }
            //end region teleportation




            //beginning of Omnir's Ultima Weapon projectile code

            NPC.ai[3]++;

            if (Main.netMode != NetmodeID.Server)
            { 
            if (NPC.ai[3] >= 300) //how often the crystal attack can happen in frames per second
            {
                if (Main.rand.NextBool(2)) //1 in 2 chance boss will use attack when it flies down on top of you
                {
                    if (Main.rand.NextBool(140)) //1 in 2 chance boss will use attack when it flies down on top of you
                    {
                        
                        float num48 = 2f;
                        Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                        int damage = 22;
                        int type = ModContent.ProjectileType<Projectiles.Enemy.EnemyIceBallUp>();
                        float rotation = (float)Math.Atan2(vector8.Y - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), vector8.X - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
                        int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * num48) * -1), (float)((Math.Sin(rotation) * num48) * -1), type, damage, 0f, Main.myPlayer);
                        Main.projectile[proj].timeLeft = 320;

                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                        int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, DustID.AncientLight, NPC.velocity.X, NPC.velocity.Y, 0, Color.Black, 2f);
                        Main.dust[dust].noGravity = true;
                        
                        NPC.ai[3] = 0;

                    }
                }
            }
            }
            NPC.ai[3] += 1; // my attempt at adding the timer that switches back to the shadow orb
            if (NPC.ai[3] >= 600)
            {
                if (NPC.ai[1] == 0) NPC.ai[1] = 1;
                else NPC.ai[1] = 0;
            }
        }





        Vector2 nextWarpPoint;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(nextWarpPoint);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            nextWarpPoint = reader.ReadVector2();
        }

        public void WyvernMageTeleport()
        {
            if (nextWarpPoint != null)
            {
                //Check if the player has line of sight to the warp point. If not, rotate it by 90 degrees and try again. After 4 checks, give up.
                //Lazy way to do this, but it's deterministic and works for 99% of cases so it works.
                //We can't check collision when we pre-select the warp point, because it moves the npc relative to the player and the player might move in the meantime
                if (Collision.CanHit(Main.player[NPC.target].Center + nextWarpPoint, 1, 1, Main.player[NPC.target].Center, 1, 1) || Collision.CanHitLine(Main.player[NPC.target].Center + nextWarpPoint, 1, 1, Main.player[NPC.target].Center, 1, 1))
                {
                    NPC.Center = Main.player[NPC.target].Center + nextWarpPoint;
                }
                else if (Collision.CanHit(Main.player[NPC.target].Center + nextWarpPoint.RotatedBy(MathHelper.ToRadians(90)), 1, 1, Main.player[NPC.target].Center, 1, 1) || Collision.CanHitLine(Main.player[NPC.target].Center + nextWarpPoint.RotatedBy(MathHelper.ToRadians(90)), 1, 1, Main.player[NPC.target].Center, 1, 1))
                {
                    NPC.Center = Main.player[NPC.target].Center + (nextWarpPoint.RotatedBy(MathHelper.ToRadians(90)));
                }
                else if (Collision.CanHit(Main.player[NPC.target].Center + nextWarpPoint.RotatedBy(MathHelper.ToRadians(270)), 1, 1, Main.player[NPC.target].Center, 1, 1) || Collision.CanHitLine(Main.player[NPC.target].Center + nextWarpPoint.RotatedBy(MathHelper.ToRadians(270)), 1, 1, Main.player[NPC.target].Center, 1, 1))
                {
                    NPC.Center = Main.player[NPC.target].Center + (nextWarpPoint.RotatedBy(MathHelper.ToRadians(270)));
                }
                else if (Collision.CanHit(Main.player[NPC.target].Center + nextWarpPoint.RotatedBy(MathHelper.ToRadians(180)), 1, 1, Main.player[NPC.target].Center, 1, 1) || Collision.CanHitLine(Main.player[NPC.target].Center + nextWarpPoint.RotatedBy(MathHelper.ToRadians(180)), 1, 1, Main.player[NPC.target].Center, 1, 1))
                {
                    NPC.Center = Main.player[NPC.target].Center + (nextWarpPoint.RotatedBy(MathHelper.ToRadians(180)));
                }
                else
                {
                    NPC.Center = Main.player[NPC.target].Center + nextWarpPoint;
                }
            }

            NPC.velocity = UsefulFunctions.GenerateTargetingVector(NPC.Center, Main.player[NPC.target].Center, 13);

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
            
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, DustID.Wraith, NPC.velocity.X + Main.rand.Next(-10, 10), NPC.velocity.Y + Main.rand.Next(-10, 10), 200, Color.Red, 4f);
                Main.dust[dust].noGravity = false;
            }

            nextWarpPoint = Main.rand.NextVector2CircularEdge(640, 640);
            NPC.netUpdate = true;
        }


        public override void FindFrame(int frameHeight)
        {
            if ((NPC.velocity.X != 0 || NPC.velocity.Y != 0) || (NPC.ai[0] >= 12 && NPC.ai[2] < 5))
            {
                NPC.frame.Y = 1 * frameHeight;
            }
            else
            {
                NPC.frame.Y = 0 * frameHeight;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
            npcLoot.Add(new Terraria.GameContent.ItemDropRules.CommonDrop(ModContent.ItemType<Items.Potions.HealingElixir>(), 10, 1, 1, 3));
            npcLoot.Add(new Terraria.GameContent.ItemDropRules.CommonDrop(ItemID.SpellTome, 100, 1, 1, 7));
            npcLoot.Add(new Terraria.GameContent.ItemDropRules.CommonDrop(ItemID.ManaRegenerationPotion, 5));
            npcLoot.Add(new Terraria.GameContent.ItemDropRules.CommonDrop(ItemID.GreaterHealingPotion, 5));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
            if (NPC.life <= 0)
            {
                if (!Main.dedServ)
                {
                    Gore.NewGore(NPC.GetSource_Death(), vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Undead Caster Gore 1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Undead Caster Gore 2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Undead Caster Gore 2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Undead Caster Gore 3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Undead Caster Gore 3").Type, 1f);
                }
            }
        }
    }
}