﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.NPCs.Bosses.SuperHardMode;

namespace tsorcRevamp.Items.BossItems {
    class SoulOfCinderSpawner : ModItem {

    	public override bool Autoload(ref string name) => false;

		public override void SetDefaults() {
			item.width = 40;
			item.height = 40;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.consumable = false;
			item.rare = ItemRarityID.Expert;
		}

		public override bool CanUseItem(Player player) {
			bool CanUse = false;
			if (!NPC.AnyNPCs(ModContent.NPCType<SoulOfCinder>())) {
				CanUse = true;
			}

			if (ModContent.GetInstance<tsorcRevampConfig>().AdventureModeItems) {
				if (!UsefulFunctions.IsPointWithinEllipse(player.Center, SoulOfCinder.ARENA_LOCATION_ADVENTURE, SoulOfCinder.ARENA_WIDTH, SoulOfCinder.ARENA_HEIGHT)) {
					Main.NewText("This item must be used within the Tomb of Gwyn.", Color.Firebrick);
					CanUse = false;
                }
            }
			return CanUse;
		}


		public override bool UseItem(Player player) {
			Main.PlaySound(SoundID.Roar, player.position, 0);
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - (16*12), ModContent.NPCType<SoulOfCinder>());
			}
			else {
				NetMessage.SendData(MessageID.SpawnBoss, -1, -1, null, player.whoAmI, ModContent.NPCType<SoulOfCinder>());
			}
			return true;
		}
	}
}