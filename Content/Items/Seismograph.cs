using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RealisticEarthquake.Common.Players;

namespace RealisticEarthquake.Content.Items
{
    public class Seismograph : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EarthquakePlayer>().HasSeismograph = true;
        }

        public override void UpdateInventory(Player player)
        {
            player.GetModPlayer<EarthquakePlayer>().HasSeismograph = true;
        }

        public override void AddRecipes()
        {
            // === НОВОЕ: Создаём группу "Любые часы" (золотые или платиновые) ===
            // Это позволяет игроку использовать любые часы, которые есть в его мире
            RecipeGroup anyWatchGroup = new RecipeGroup(() => $"[{Lang.GetItemNameValue(ItemID.GoldWatch)}/{Lang.GetItemNameValue(ItemID.PlatinumWatch)}]",
                ItemID.GoldWatch,
                ItemID.PlatinumWatch
            );
            RecipeGroup.RegisterGroup("RealisticEarthquake:AnyWatch", anyWatchGroup);

            // === Основной рецепт ===
            CreateRecipe()
                .AddIngredient(ItemID.StoneBlock, 15)
                .AddIngredient(ItemID.IronBar, 5)
                .AddRecipeGroup(anyWatchGroup, 1)  // ← Используем группу вместо конкретного предмета
                .AddIngredient(ItemID.Lens, 1)     // ← НОВАЯ: Линза для наблюдения
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}