using tsorcRevamp.Projectiles.Swords.Runeterra;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Buffs.Runeterra;

namespace tsorcRevamp.Items.Weapons.Melee.Runeterra
{
    public class SteelTempest: ModItem
    {
        public int AttackSpeedScalingDuration;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Steel Tempest");
            Tooltip.SetDefault("Doubled crit chance scaling" +
                "\nThrusts on right click dealing 125% damage, cooldown scales down with attack speed" +
                "\nGain a stack of Steel Tempest upon thrusting any enemy" +
                "\nUpon reaching 2 stacks, the next right click will release a tornado dealing double damage");
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.damage = 20;
            Item.crit = 6;
            Item.width = 86;
            Item.height = 82;
            Item.scale = 0.7f;
            Item.knockBack = 1f;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noUseGraphic = false;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 4.2f;
            Item.useTurn = false;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit *= 2;
        }
        public override void HoldItem(Player player)
        {
            player.GetModPlayer<tsorcRevampPlayer>().DoubleCritChance = true;
            AttackSpeedScalingDuration = (int)(3 / player.GetTotalAttackSpeed(DamageClass.Melee) * 60); //3 seconds divided by player's melee speed
            if (AttackSpeedScalingDuration <= 80)
            {
                AttackSpeedScalingDuration = 80; //1.33 seconds minimum
            }
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (Main.mouseRight & !Main.mouseLeft & player.GetModPlayer<tsorcRevampPlayer>().steeltempest >= 2 & !player.HasBuff(ModContent.BuffType<SteelTempestThrustCooldown>()))
            {
                player.altFunctionUse = 2;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.shoot = ModContent.ProjectileType<SteelTempestTornado>();
                player.AddBuff(ModContent.BuffType<SteelTempestThrustCooldown>(), AttackSpeedScalingDuration);
                player.GetModPlayer<tsorcRevampPlayer>().steeltempest = 0;
            } else
            if (Main.mouseRight & !Main.mouseLeft & player.altFunctionUse == 2 & !player.HasBuff(ModContent.BuffType<SteelTempestThrustCooldown>()))
            {
                player.altFunctionUse = 2;
                Item.useStyle = ItemUseStyleID.Rapier;
                Item.noUseGraphic = true;
                Item.noMelee = true; 
                player.AddBuff(ModContent.BuffType<SteelTempestThrustCooldown>(), AttackSpeedScalingDuration);
                Item.shoot = ModContent.ProjectileType<SteelTempestThrust>();
            }
            if (Main.mouseLeft)
            {
                player.altFunctionUse = 1;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = false;
                Item.noMelee = false;
                Item.useTurn = false;
                Item.shoot = ModContent.ProjectileType<Projectiles.Nothing>();
            }

        }

        public override void UpdateInventory(Player player)
        {   
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse != 2 || !player.HasBuff(ModContent.BuffType<SteelTempestThrustCooldown>()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*public override bool CanShoot(Player player)
        {
            if (player.altFunctionUse == 2 &&  Main.mouseRight)
            {
                return true;
            } return false;
        }*/

        public override bool AltFunctionUse(Player player)
        {
                return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<WorldRune>());
            recipe.AddIngredient(ItemID.Katana);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 2000);

            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }
    }
}