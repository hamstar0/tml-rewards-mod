using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Config;
using Rewards.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Rewards {
	public partial class RewardsConfigData : ConfigurationDataBase {
		public override void OnLoad( bool success ) {
			TmlLoadHelpers.AddPostModLoadPromise( delegate {
				if( !success || this.ShopLoadout.Count == 0 ) {
					this.SetDefaults();
					//RewardsMod.Instance.ConfigJson.SaveFile();
				} else {
					foreach( var info in this.ShopLoadout ) {
						string fail;
						if( !info.Validate( out fail ) ) {
							ErrorLogger.Log( "Could not validate shop item " + info.Name + " (" + fail + ")" );
						}
					}
				}
			} );
		}


		////////////////

		public bool SetDefaults() {
			string wof_name = Lang.GetNPCNameValue( NPCID.WallofFlesh );
			string plantera_name = Lang.GetNPCNameValue( NPCID.Plantera );
			string golem_name = Lang.GetNPCNameValue( NPCID.Golem );
			string moonlord_name = Lang.GetNPCNameValue( NPCID.MoonLordCore );  //NPCID.MoonLordHead?

			this.NpcRewards = new Dictionary<string, float> {
				{ Lang.GetNPCNameValue( NPCID.KingSlime ), 10f },
				{ Lang.GetNPCNameValue( NPCID.EyeofCthulhu ), 10f },
				{ Lang.GetNPCNameValue( NPCID.EaterofWorldsHead ), 25f },
				{ Lang.GetNPCNameValue( NPCID.BrainofCthulhu ), 25f },
				{ Lang.GetNPCNameValue( NPCID.QueenBee ), 20f },
				{ Lang.GetNPCNameValue( NPCID.SkeletronHead ), 30f },
				{ Lang.GetNPCNameValue( NPCID.WallofFlesh ), 50f },
				{ Lang.GetNPCNameValue( NPCID.TheDestroyer ), 50f },
				{ Lang.GetNPCNameValue( NPCID.Retinazer ), 50f / 2 },
				{ Lang.GetNPCNameValue( NPCID.Spazmatism ), 50f / 2 },
				{ Lang.GetNPCNameValue( NPCID.SkeletronPrime ), 50f },
				{ Lang.GetNPCNameValue( NPCID.Plantera ), 100f },
				{ Lang.GetNPCNameValue( NPCID.Golem ), 100f },
				{ Lang.GetNPCNameValue( NPCID.DukeFishron ), 100f },
				{ Lang.GetNPCNameValue( NPCID.CultistBoss ), 50f },
				{ Lang.GetNPCNameValue( NPCID.DD2Betsy ), 100f },
				{ Lang.GetNPCNameValue( NPCID.LunarTowerSolar ), 35f },
				{ Lang.GetNPCNameValue( NPCID.LunarTowerVortex ), 35f },
				{ Lang.GetNPCNameValue( NPCID.LunarTowerNebula ), 35f },
				{ Lang.GetNPCNameValue( NPCID.LunarTowerStardust ), 35f },
				{ moonlord_name, 250f }
			};

			this.NpcRewardRequiredAsBoss = new HashSet<string> {
				Lang.GetNPCNameValue( NPCID.EaterofWorldsHead )
			};

			this.ShopLoadout = new List<ShopPackDefinition> {
				new ShopPackDefinition( "", "Money Purse", 5, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.GoldCoin), 99 )
					} ),
				new ShopPackDefinition( "", "Iron Age Pack", 5, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.IronBar), 99 )
					} ),
				new ShopPackDefinition( "", "Nature Watcher's Pack", 10, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.FlowerBoots), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.Binoculars), 1 )
					} ),
				new ShopPackDefinition( "", "Bindings Pack", 10, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.BandofRegeneration), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.BandofStarpower), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.CelestialMagnet), 1 )
					} ),
				new ShopPackDefinition( "", "Traveler's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.Aglet), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.IceSkates), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.TigerClimbingGear), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.WaterWalkingBoots), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.HandWarmer), 1 )
					} ),
				new ShopPackDefinition( "", "Rough Traveler's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.AnkletoftheWind), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.FeralClaws), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.JellyfishDivingGear), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.LavaCharm), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.MagmaStone), 1 )
					} ),
				new ShopPackDefinition( "", "Ground Hater's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.SandstorminaBottle), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.ShinyRedBalloon), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.LuckyHorseshoe), 1 )
					} ),
				new ShopPackDefinition( "", "Stoicist's Pack", 20, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.CobaltShield), 1 )
					} ),
				new ShopPackDefinition( "", "Alucard's Pack", 35, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.Arkhalis), 1 )
					} ),
				new ShopPackDefinition( "", "Gizmo Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.Toolbox), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.ArchitectGizmoPack), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.ActuationAccessory), 1 )
					} ),
				new ShopPackDefinition( "", "Information Monger's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.GPS), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.GoblinTech), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.REK), 1 )
					} ),
				new ShopPackDefinition( "", "Fisherman's Pack", 50, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.AnglerTackleBag), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.FishFinder), 1 )
					} ),

				// Hard mode only:
				new ShopPackDefinition( wof_name, "Mimic's Lament Pack", 35, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.MagicDagger), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.TitanGlove), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.PhilosophersStone), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.CrossNecklace), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.StarCloak), 1 )
					} ),
				new ShopPackDefinition( wof_name, "Gangster's Pack", 35, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.Uzi), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.RifleScope), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.EndlessMusketPouch), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.GangstaHat), 1 )
					} ),
				new ShopPackDefinition( wof_name, "Avenger Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.AvengerEmblem), 1 )
					} ),
				new ShopPackDefinition( wof_name, "Life Pack", 150, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.AnkhCharm), 1 )
					} ),
				new ShopPackDefinition( wof_name, "Lucky Pack", 150, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.CoinRing), 1 )
					} ),
				new ShopPackDefinition( wof_name, "Dimensionalist's Pack", 200, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.RodofDiscord), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.TeleportationPotion), 30 )
					} ),

				// Post-plantera:
				new ShopPackDefinition( plantera_name, "Whack Pack", 50, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.TheAxe), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.Bananarang), 10 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.SlapHand), 1 )
					} ),

				// Post-golem:
				new ShopPackDefinition( golem_name, "Golem Eye Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.EyeoftheGolem), 1 )
					} ),
				new ShopPackDefinition( golem_name, "Defender's Pack", 150, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.CelestialShell), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.PaladinsShield), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.FrozenTurtleShell), 1 )
					} ),

				// Post-moonlord:
				new ShopPackDefinition( moonlord_name, "Eldritch Pack", 500, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.Meowmere), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.Terrarian), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.StarWrath), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.SDMG), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.FireworksLauncher), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.LastPrism), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.LunarFlareBook), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.RainbowCrystalStaff), 1 ),
						new ShopPackItemDefinition( Lang.GetItemNameValue(ItemID.MoonlordTurretStaff), 1 )
					} )
			};
			/*this.ShopLoadout = new List<ShopPackDefinition> {
				new ShopPackDefinition( "Money Purse", 10, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Gold Coin", 99 )
				} ),
				new ShopPackDefinition( "Iron Age Pack", 10, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Iron Bar", 99 )
				} ),
				new ShopPackDefinition( "Life Pack", 15, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Life Crystal", 10 ),
					new ShopPackItemDefinition( "Healing Potion", 30 )
				} ),
				new ShopPackDefinition( "Evil Essence Pack", 25, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Tissue Sample", 99, true ),
					new ShopPackItemDefinition( "Shadow Orb", 99, false )
				} ),
				new ShopPackDefinition( "Traveler's Pack", 35, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Spectre Boots", 1 ),
					new ShopPackItemDefinition( "Ivy Whip", 1 ),
					new ShopPackItemDefinition( "Obsidian Shield", 1 ),
					new ShopPackItemDefinition( "Blue Horseshoe Balloon", 1 ),
					new ShopPackItemDefinition( "Javelin", 99 )
				} ),
				new ShopPackDefinition( "Monster Pack", 25, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Slime Crown", 3 ),
					new ShopPackItemDefinition( "Suspicious Looking Eye", 3 ),
					new ShopPackItemDefinition( "Abeemination", 3 ),
					new ShopPackItemDefinition( "Goblin Battle Standard", 1 ),
					new ShopPackItemDefinition( "Clothier Voodoo Doll", 1 ),
					new ShopPackItemDefinition( "Guide Voodoo Doll", 1 )
				} ),
				new ShopPackDefinition( "Fire & Brimstone Pack", 50, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Molotov Cocktail", 99 ),
					new ShopPackItemDefinition( "Hellstone Bar", 99 )
				} ),
				new ShopPackDefinition( "Jungle Delver's Pack", 100, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Pickaxe Axe", 1 ),
					new ShopPackItemDefinition( "Cutlass", 1 ),
					new ShopPackItemDefinition( "Flamethrower", 1 )
				} ),
				new ShopPackDefinition( "Cleanup Kit", 50, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Clentaminator", 1 ),
					new ShopPackItemDefinition( "Green Solution", 999 )
				} ),
				new ShopPackDefinition( "Lihzahrd Embassador Kit", 150, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Temple Key", 1 ),
					new ShopPackItemDefinition( "Life Fruit", 10 ),
					new ShopPackItemDefinition( "Venus Magnum", 1 )
				} ),
				new ShopPackDefinition( "Cultist's Pack", 150, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Golem Fist", 1 ),
					new ShopPackItemDefinition( "Pirate Map", 2 ),
					new ShopPackItemDefinition( "Solar Tablet", 1 ),
					new ShopPackItemDefinition( "Pumpkin Moon Medallion", 2 ),
					new ShopPackItemDefinition( "Naughty Present", 2 )
				} )
				//new ShopPackDefinition( "Time Traveler's Pack", 35, new ShopItemInfo[] {
				//	new ShopPackItemDefinition( "Time Capsule", 1 )
				//} )
			};*/

			return true;
		}
	}
}
