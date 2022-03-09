using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.NPCs.Bosses.SuperHardMode.GhostWyvernMage
{
    
    class MageShadow : ModNPC
    {
        public override void SetDefaults()
        {
            npc.npcSlots = 3;
            Main.npcFrameCount[npc.type] = 3;
            animationType = 29;
            npc.aiStyle = 0;
            npc.damage = 90;
            npc.defense = 96;
            npc.height = 44;
            npc.timeLeft = 22500;
            npc.lifeMax = 1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath5;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.alpha = 249;
            
            npc.lavaImmune = true;
            npc.value = 0;
            npc.width = 28;
            npc.knockBackResist = 0.2f;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Confused] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            
        }


        int frozenSawDamage = 35;
        int lightningDamage = 64;
        int Timer2 = -Main.rand.Next(200);

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage / 2);
            frozenSawDamage = (int)(frozenSawDamage / 2);
            lightningDamage = (int)(lightningDamage / 2);
        }

       


        #region AI
        
        public override void AI()
        {
            
           

            npc.netUpdate = false;
            npc.ai[0]++; // Timer Scythe
            
            npc.ai[1]++; // Timer Teleport
            

            // npc.ai[2]++; // Shots

            if (npc.life < 51)
            {
                int dust = Dust.NewDust(new Vector2((float)npc.position.X, (float)npc.position.Y), npc.width, npc.height, Type: DustID.MagicMirror, npc.velocity.X, npc.velocity.Y, 150, Color.Blue, 1f);
                Main.dust[dust].noGravity = true;
            }
            

            if (Main.netMode != 2)
            {
                if (npc.ai[0] >= 7 && npc.ai[2] < 3)
                {
                    float num48 = 1f;
                    Vector2 vector8 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height / 2));
                    int type = ModContent.ProjectileType<Projectiles.Enemy.FrozenSawII>();
                    float rotation = (float)Math.Atan2(vector8.Y - (Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5f)), vector8.X - (Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5f)));
                    int num54 = Projectile.NewProjectile(vector8.X, vector8.Y, (float)((Math.Cos(rotation) * num48) * -1), (float)((Math.Sin(rotation) * num48) * -1), type, frozenSawDamage, 0f, Main.myPlayer);
                    Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 20);
                    npc.ai[0] = 0;
                    npc.ai[2]++;
                }
            }


            if (npc.ai[1] >= 10)
            {
                npc.velocity.X *= 0.77f;
                npc.velocity.Y *= 0.27f;
            }

            if ((npc.ai[1] >= 200 && npc.life > 2000) || (npc.ai[1] >= 120 && npc.life <= 2000))
            {
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
                for (int num36 = 0; num36 < 10; num36++)
                {
                    int dust = Dust.NewDust(new Vector2((float)npc.position.X, (float)npc.position.Y), npc.width, npc.height, 54, npc.velocity.X + Main.rand.Next(-10, 10), npc.velocity.Y + Main.rand.Next(-10, 10), 200, Color.Red, 4f);
                    Main.dust[dust].noGravity = false;
                }
                npc.ai[3] = (float)(Main.rand.Next(360) * (Math.PI / 180));
                npc.ai[2] = 0;
                npc.ai[1] = 0;


                Player Pt = Main.player[npc.target];
                Vector2 NC = npc.position + new Vector2(npc.width / 2, npc.height / 2);
                Vector2 PtC = Pt.position + new Vector2(Pt.width / 2, Pt.height / 2);
                npc.position.X = Pt.position.X + (float)((600 * Math.Cos(npc.ai[3])) * -1);
                npc.position.Y = Pt.position.Y - 45 + (float)((30 * Math.Sin(npc.ai[3])) * -1);

                float MinDIST = 400f;
                float MaxDIST = 700f;
                Vector2 Diff = npc.position - Pt.position;
                if (Diff.Length() > MaxDIST)
                {
                    Diff *= MaxDIST / Diff.Length();
                }
                if (Diff.Length() < MinDIST)
                {
                    Diff *= MinDIST / Diff.Length();
                }
                npc.position = Pt.position + Diff;

                NC = npc.position + new Vector2(npc.width / 2, npc.height / 2);

                float rotation = (float)Math.Atan2(NC.Y - PtC.Y, NC.X - PtC.X);
                npc.velocity.X = (float)(Math.Cos(rotation) * 13) * -1;
                npc.velocity.Y = (float)(Math.Sin(rotation) * 13) * -1;

            }

            //end of W1k's Death code
            Timer2++;
            if (Timer2 >= 0)
            {


                bool clearSpace = true;
                for (int i = 0; i < 15; i++)
                {
                    if (UsefulFunctions.IsTileReallySolid((int)npc.Center.X / 16, ((int)npc.Center.Y / 16) - i))
                    {
                        clearSpace = false;
                    }
                }

                if (clearSpace)
                {
                    Vector2 speed = UsefulFunctions.BallisticTrajectory(npc.Center, Main.player[npc.target].Center, 5);


                    speed.Y += Main.rand.NextFloat(-2f, -6f);
                    //speed += Main.rand.NextVector2Circular(-10, -8);
                    if (((speed.X < 0f) && (npc.velocity.X < 0f)) || ((speed.X > 0f) && (npc.velocity.X > 0f)))
                    {
                        int lob = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, speed.X, speed.Y, ProjectileID.DD2DrakinShot, 80, 0f, Main.myPlayer);
                        //ModContent.ProjectileType<Projectiles.Enemy.EnemySporeTrap>()
                        //DesertDjinnCurse; ProjectileID.DD2DrakinShot

                        Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 20, 0.2f, -0.5f);
                        if (Timer2 >= 300)
                        {
                            Timer2 = -200 - Main.rand.Next(1250);
                        }
                    }
                }
                else
                {
                    Timer2 = -200 - Main.rand.Next(100);
                }

            }
            //beginning of Omnir's Ultima Weapon projectile code

            npc.ai[3]++;

            if (npc.ai[3] >= 100) //how often the crystal attack can happen in frames per second
            {
                if (Main.rand.Next(2) == 0) //1 in 2 chance boss will use attack when it flies down on top of you
                {
                    float num48 = 0.9f;
                    Vector2 vector9 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y - 220 + (npc.height / 2));
                    float speedX = ((Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5f)) - vector9.X) + Main.rand.Next(-20, 0x15);
                    float speedY = ((Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5f)) - vector9.Y) + Main.rand.Next(-20, 0x15);
                    if (((speedX < 0f) && (npc.velocity.X < 0f)) || ((speedX > 0f) && (npc.velocity.X > 0f)))
                    {
                        float num51 = (float)Math.Sqrt((double)((speedX * speedX) + (speedY * speedY)));
                        num51 = num48 / num51;
                        speedX *= num51;
                        speedY *= num51;
                        int type = ModContent.ProjectileType<Projectiles.Enemy.EnemySpellLightning4Ball>();//44;//0x37; //14;
                        int num54 = Projectile.NewProjectile(vector9.X, vector9.Y, speedX, speedY, type, lightningDamage, 0f, Main.myPlayer);
                        Main.projectile[num54].timeLeft = 250;
                        Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 25);
                        npc.ai[3] = 0;
                    }
                }

                
            }

            npc.ai[3] += 1; // my attempt at adding the timer that switches back to the shadow orb
            if (npc.ai[3] >= 800)
            {
                if (npc.ai[1] == 0) npc.ai[1] = 1;
                else npc.ai[1] = 0;
            }
        }

        #endregion

        public override void FindFrame(int currentFrame)
        {

            if ((npc.velocity.X > -9 && npc.velocity.X < 9) && (npc.velocity.Y > -9 && npc.velocity.Y < 9))
            {
                npc.frameCounter = 0;
                npc.frame.Y = 0;
                if (npc.position.X > Main.player[npc.target].position.X)
                {
                    npc.spriteDirection = -1;
                }
                else
                {
                    npc.spriteDirection = 1;
                }
            }

            int num = 1;
            if (!Main.dedServ)
            {
                num = Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type];
            }
            if ((npc.velocity.X > -2 && npc.velocity.X < 2) && (npc.velocity.Y > -2 && npc.velocity.Y < 2))
            {
                npc.frameCounter = 0;
                npc.frame.Y = 0;
            }
            else
            {
                npc.frameCounter += 1.0;
            }
            if (npc.frameCounter >= 1.0)
            {
                npc.frame.Y = npc.frame.Y + num;
                npc.frameCounter = 0.0;
            }
            if (npc.frame.Y >= num * Main.npcFrameCount[npc.type])
            {
                npc.frame.Y = 0;
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        #region Gore
        public override void NPCLoot()
        {
            Vector2 vector8 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height / 2));
            if (npc.life <= 0)
            {
                for (int num36 = 0; num36 < 50; num36++)
                {
                    {
                        Color color = new Color();
                        int dust = Dust.NewDust(new Vector2((float)npc.position.X, (float)npc.position.Y), npc.width, npc.height * 4, DustID.PurpleTorch, Main.rand.Next(-20, 20) * 2, Main.rand.Next(-20, 20) * 2, 100, color, 4f);
                        Main.dust[dust].noGravity = true;
                        dust = Dust.NewDust(new Vector2((float)npc.position.X, (float)npc.position.Y), npc.width, npc.height * 4, DustID.PurpleTorch, Main.rand.Next(-20, 20) * 2, Main.rand.Next(-20, 20) * 2, 100, color, 4f);
                        Main.dust[dust].noGravity = true;
                        dust = Dust.NewDust(new Vector2((float)npc.position.X, (float)npc.position.Y), npc.width, npc.height * 4, DustID.PurpleTorch, Main.rand.Next(-20, 20) * 2, Main.rand.Next(-20, 20) * 2, 100, color, 4f);
                        Main.dust[dust].noGravity = true;
                        dust = Dust.NewDust(new Vector2((float)npc.position.X, (float)npc.position.Y), npc.width, npc.height * 4, DustID.PurpleTorch, Main.rand.Next(-20, 20) * 2, Main.rand.Next(-20, 20) * 2, 100, color, 4f);
                        Main.dust[dust].noGravity = true;
                    }
                }
            }
        }
        #endregion

        public static Texture2D texture;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            GhostDragonHead.GhostEffect(npc, spriteBatch, ref texture, 0.9f);
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            GhostDragonHead.GhostEffect(npc, spriteBatch, ref texture, 1f);
        }

    }
}