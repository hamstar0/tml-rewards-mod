using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using Rewards.Items;
using System.Collections.Generic;
using Terraria.ID;


namespace Rewards.Configs {
	public partial class RewardsShopConfigData : ConfigurationDataBase {
		public bool SetDefaults() {
			string wofName = NPCIdentityHelpers.GetQualifiedName( NPCID.WallofFlesh );
			string planteraName = NPCIdentityHelpers.GetQualifiedName( NPCID.Plantera );
			string golemName = NPCIdentityHelpers.GetQualifiedName( NPCID.Golem );
			string moonlordName = NPCIdentityHelpers.GetQualifiedName( NPCID.MoonLordCore );  //NPCID.MoonLordHead?

			this.ShopLoadout = new List<ShopPackDefinition> {
				new ShopPackDefinition( "", "Iron Age Pack", 5, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.IronBar), 99 )
					} ),
				new ShopPackDefinition( "", "Money Purse", 10, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.GoldCoin), 99 )
					} ),
				new ShopPackDefinition( "", "Nature Watcher's Pack", 10, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.FlowerBoots), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.Binoculars), 1 )
					} ),
				new ShopPackDefinition( "", "Bindings Pack", 10, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.BandofRegeneration), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.BandofStarpower), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.CelestialMagnet), 1 )
					} ),
				new ShopPackDefinition( "", "Traveler's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.Aglet), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.IceSkates), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.TigerClimbingGear), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.WaterWalkingBoots), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.HandWarmer), 1 )
					} ),
				new ShopPackDefinition( "", "Rough Traveler's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.AnkletoftheWind), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.FeralClaws), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.JellyfishDivingGear), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.LavaCharm), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.MagmaStone), 1 )
					} ),
				new ShopPackDefinition( "", "Ground Hater's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.SandstorminaBottle), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.ShinyRedBalloon), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.LuckyHorseshoe), 1 )
					} ),
				new ShopPackDefinition( "", "Stoicist's Pack", 20, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.CobaltShield), 1 )
					} ),
				new ShopPackDefinition( "", "Alucard's Pack", 35, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.Arkhalis), 1 )
					} ),
				new ShopPackDefinition( "", "Gizmo Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.Toolbox), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.ArchitectGizmoPack), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.ActuationAccessory), 1 )
					} ),
				new ShopPackDefinition( "", "Information Monger's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.GPS), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.GoblinTech), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.REK), 1 )
					} ),
				new ShopPackDefinition( "", "Fisherman's Pack", 50, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.AnglerTackleBag), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.FishFinder), 1 )
					} ),

				// Hard mode only:
				new ShopPackDefinition( wofName, "Mimic's Lament Pack", 35, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.MagicDagger), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.TitanGlove), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.PhilosophersStone), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.CrossNecklace), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.StarCloak), 1 )
					} ),
				new ShopPackDefinition( wofName, "Gangster's Pack", 35, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.Uzi), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.RifleScope), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.EndlessMusketPouch), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.GangstaHat), 1 )
					} ),
				new ShopPackDefinition( wofName, "Avenger Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.AvengerEmblem), 1 )
					} ),
				new ShopPackDefinition( wofName, "Life Pack", 150, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.AnkhCharm), 1 )
					} ),
				new ShopPackDefinition( wofName, "Lucky Pack", 150, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.CoinRing), 1 )
					} ),
				new ShopPackDefinition( wofName, "Dimensionalist's Pack", 200, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.RodofDiscord), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.TeleportationPotion), 30 )
					} ),

				// Post-plantera:
				new ShopPackDefinition( planteraName, "Whack Pack", 50, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.TheAxe), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.Bananarang), 10 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.SlapHand), 1 )
					} ),

				// Post-golem:
				new ShopPackDefinition( golemName, "Golem Eye Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.EyeoftheGolem), 1 )
					} ),
				new ShopPackDefinition( golemName, "Defender's Pack", 150, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.CelestialShell), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.PaladinsShield), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.FrozenTurtleShell), 1 )
					} ),

				// Post-moonlord:
				new ShopPackDefinition( moonlordName, "Eldritch Pack", 500, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.Meowmere), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.Terrarian), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.StarWrath), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.SDMG), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.FireworksLauncher), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.LastPrism), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.LunarFlareBook), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.RainbowCrystalStaff), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.MoonlordTurretStaff), 1 )
					} )
			};

			return true;
		}
	}
}
