using Rewards.Items;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader.Config;


namespace Rewards.Configs {
	public partial class RewardsShopConfig : ModConfig {
		public RewardsShopConfig() {
			var wofDef = new NPCDefinition( NPCID.WallofFlesh );
			var planteraDef = new NPCDefinition( NPCID.Plantera );
			var golemDef = new NPCDefinition( NPCID.Golem );
			var moonlordDef = new NPCDefinition( NPCID.MoonLordCore );  //NPCID.MoonLordHead?

			this.ShopLoadout = new List<ShopPackDefinition> {
				new ShopPackDefinition( null, "Iron Age Pack", 5, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.IronBar), 99 )
				} ),
				new ShopPackDefinition( null, "Money Purse", 10, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.GoldCoin), 99 )
				} ),
				new ShopPackDefinition( null, "Nature Watcher's Pack", 10, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.FlowerBoots), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.Binoculars), 1 )
				} ),
				new ShopPackDefinition( null, "Bindings Pack", 10, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.BandofRegeneration), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.BandofStarpower), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.CelestialMagnet), 1 )
				} ),
				new ShopPackDefinition( null, "Traveler's Pack", 25, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.Aglet), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.IceSkates), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.TigerClimbingGear), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.WaterWalkingBoots), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.HandWarmer), 1 )
				} ),
				new ShopPackDefinition( null, "Rough Traveler's Pack", 25, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.AnkletoftheWind), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.FeralClaws), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.JellyfishDivingGear), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.LavaCharm), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.MagmaStone), 1 )
				} ),
				new ShopPackDefinition( null, "Ground Hater's Pack", 25, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.SandstorminaBottle), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.ShinyRedBalloon), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.LuckyHorseshoe), 1 )
				} ),
				new ShopPackDefinition( null, "Stoicist's Pack", 20, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.CobaltShield), 1 )
				} ),
				new ShopPackDefinition( null, "Alucard's Pack", 35, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.Arkhalis), 1 )
				} ),
				new ShopPackDefinition( null, "Gizmo Pack", 25, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.Toolbox), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.ArchitectGizmoPack), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.ActuationAccessory), 1 )
				} ),
				new ShopPackDefinition( null, "Information Monger's Pack", 25, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.GPS), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.GoblinTech), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.REK), 1 )
				} ),
				new ShopPackDefinition( null, "Fisherman's Pack", 50, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.AnglerTackleBag), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.FishFinder), 1 )
				} ),

				// Hard mode only:
				new ShopPackDefinition( wofDef, "Mimic's Lament Pack", 35, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.MagicDagger), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.TitanGlove), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.PhilosophersStone), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.CrossNecklace), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.StarCloak), 1 )
				} ),
				new ShopPackDefinition( wofDef, "Gangster's Pack", 35, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.Uzi), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.RifleScope), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.EndlessMusketPouch), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.GangstaHat), 1 )
				} ),
				new ShopPackDefinition( wofDef, "Avenger Pack", 25, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.AvengerEmblem), 1 )
				} ),
				new ShopPackDefinition( wofDef, "Life Pack", 150, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.AnkhCharm), 1 )
				} ),
				new ShopPackDefinition( wofDef, "Lucky Pack", 150, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.CoinRing), 1 )
				} ),
				new ShopPackDefinition( wofDef, "Dimensionalist's Pack", 200, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.RodofDiscord), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.TeleportationPotion), 30 )
				} ),

				// Post-plantera:
				new ShopPackDefinition( planteraDef, "Whack Pack", 50, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.TheAxe), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.Bananarang), 10 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.SlapHand), 1 )
				} ),

				// Post-golem:
				new ShopPackDefinition( golemDef, "Golem Eye Pack", 25, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.EyeoftheGolem), 1 )
				} ),
				new ShopPackDefinition( golemDef, "Defender's Pack", 150, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.CelestialShell), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.PaladinsShield), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.FrozenTurtleShell), 1 )
				} ),

				// Post-moonlord:
				new ShopPackDefinition( moonlordDef, "Eldritch Pack", 500, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( new ItemDefinition(ItemID.Meowmere), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.Terrarian), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.StarWrath), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.SDMG), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.FireworksLauncher), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.LastPrism), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.LunarFlareBook), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.RainbowCrystalStaff), 1 ),
					new ShopPackItemDefinition( new ItemDefinition(ItemID.MoonlordTurretStaff), 1 )
				} )
			};
		}
	}
}
