using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.NPCs;
using Rewards.Items;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Terraria.ID;
using Terraria.ModLoader.Config;


namespace Rewards.Configs {
	public partial class RewardsShopConfig : ModConfig {
		[OnDeserialized]
		internal void OnDeserializedMethod( StreamingContext context ) {
			if( this.ShopLoadout != null ) {
				return;
			}

			string wofName = NPCIdentityHelpers.GetUniqueKey( NPCID.WallofFlesh );
			string planteraName = NPCIdentityHelpers.GetUniqueKey( NPCID.Plantera );
			string golemName = NPCIdentityHelpers.GetUniqueKey( NPCID.Golem );
			string moonlordName = NPCIdentityHelpers.GetUniqueKey( NPCID.MoonLordCore );  //NPCID.MoonLordHead?

			this.ShopLoadout = new List<ShopPackDefinition> {
				new ShopPackDefinition( "", "Iron Age Pack", 5, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.IronBar), 99 )
					} ),
				new ShopPackDefinition( "", "Money Purse", 10, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.GoldCoin), 99 )
					} ),
				new ShopPackDefinition( "", "Nature Watcher's Pack", 10, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.FlowerBoots), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.Binoculars), 1 )
					} ),
				new ShopPackDefinition( "", "Bindings Pack", 10, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.BandofRegeneration), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.BandofStarpower), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.CelestialMagnet), 1 )
					} ),
				new ShopPackDefinition( "", "Traveler's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.Aglet), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.IceSkates), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.TigerClimbingGear), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.WaterWalkingBoots), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.HandWarmer), 1 )
					} ),
				new ShopPackDefinition( "", "Rough Traveler's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.AnkletoftheWind), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.FeralClaws), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.JellyfishDivingGear), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.LavaCharm), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.MagmaStone), 1 )
					} ),
				new ShopPackDefinition( "", "Ground Hater's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.SandstorminaBottle), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.ShinyRedBalloon), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.LuckyHorseshoe), 1 )
					} ),
				new ShopPackDefinition( "", "Stoicist's Pack", 20, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.CobaltShield), 1 )
					} ),
				new ShopPackDefinition( "", "Alucard's Pack", 35, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.Arkhalis), 1 )
					} ),
				new ShopPackDefinition( "", "Gizmo Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.Toolbox), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.ArchitectGizmoPack), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.ActuationAccessory), 1 )
					} ),
				new ShopPackDefinition( "", "Information Monger's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.GPS), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.GoblinTech), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.REK), 1 )
					} ),
				new ShopPackDefinition( "", "Fisherman's Pack", 50, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.AnglerTackleBag), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.FishFinder), 1 )
					} ),

				// Hard mode only:
				new ShopPackDefinition( wofName, "Mimic's Lament Pack", 35, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.MagicDagger), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.TitanGlove), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.PhilosophersStone), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.CrossNecklace), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.StarCloak), 1 )
					} ),
				new ShopPackDefinition( wofName, "Gangster's Pack", 35, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.Uzi), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.RifleScope), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.EndlessMusketPouch), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.GangstaHat), 1 )
					} ),
				new ShopPackDefinition( wofName, "Avenger Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.AvengerEmblem), 1 )
					} ),
				new ShopPackDefinition( wofName, "Life Pack", 150, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.AnkhCharm), 1 )
					} ),
				new ShopPackDefinition( wofName, "Lucky Pack", 150, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.CoinRing), 1 )
					} ),
				new ShopPackDefinition( wofName, "Dimensionalist's Pack", 200, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.RodofDiscord), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.TeleportationPotion), 30 )
					} ),

				// Post-plantera:
				new ShopPackDefinition( planteraName, "Whack Pack", 50, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.TheAxe), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.Bananarang), 10 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.SlapHand), 1 )
					} ),

				// Post-golem:
				new ShopPackDefinition( golemName, "Golem Eye Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.EyeoftheGolem), 1 )
					} ),
				new ShopPackDefinition( golemName, "Defender's Pack", 150, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.CelestialShell), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.PaladinsShield), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.FrozenTurtleShell), 1 )
					} ),

				// Post-moonlord:
				new ShopPackDefinition( moonlordName, "Eldritch Pack", 500, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.Meowmere), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.Terrarian), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.StarWrath), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.SDMG), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.FireworksLauncher), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.LastPrism), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.LunarFlareBook), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.RainbowCrystalStaff), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetUniqueKey(ItemID.MoonlordTurretStaff), 1 )
					} )
			};
		}
	}
}
