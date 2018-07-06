﻿using HamstarHelpers.Components.Config;
using HamstarHelpers.ItemHelpers;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.Services.Promises;
using Rewards.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Rewards {
	public partial class RewardsConfigData : ConfigurationDataBase {
		public override void OnLoad( bool success ) {
			Promises.AddPostModLoadPromise( delegate {
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
			string wof_name = NPCIdentityHelpers.GetQualifiedName( NPCID.WallofFlesh );
			string plantera_name = NPCIdentityHelpers.GetQualifiedName( NPCID.Plantera );
			string golem_name = NPCIdentityHelpers.GetQualifiedName( NPCID.Golem );
			string moonlord_name = NPCIdentityHelpers.GetQualifiedName( NPCID.MoonLordCore );  //NPCID.MoonLordHead?

			this.NpcRewards = new Dictionary<string, float> {
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.KingSlime ), 10f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.EyeofCthulhu ), 10f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.EaterofWorldsHead ), 25f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.BrainofCthulhu ), 25f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.QueenBee ), 20f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.SkeletronHead ), 30f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.WallofFlesh ), 50f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.TheDestroyer ), 50f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Retinazer ), 50f / 2 },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Spazmatism ), 50f / 2 },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.SkeletronPrime ), 50f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Plantera ), 100f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Golem ), 100f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.DukeFishron ), 100f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.CultistBoss ), 50f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.DD2Betsy ), 100f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.LunarTowerSolar ), 35f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.LunarTowerVortex ), 35f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.LunarTowerNebula ), 35f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.LunarTowerStardust ), 35f },
				{ moonlord_name, 250f }
			};

			this.NpcRewardRequiredAsBoss = new HashSet<string> {
				NPCIdentityHelpers.GetQualifiedName( NPCID.EaterofWorldsHead )
			};

			this.NpcRewardNotGivenAfterNpcKilled = new Dictionary<string, string> {
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.KingSlime ), wof_name },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.EyeofCthulhu ), wof_name },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.EaterofWorldsHead ), wof_name },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.BrainofCthulhu ), wof_name },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.QueenBee ), wof_name },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.SkeletronHead ), wof_name },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.WallofFlesh ), plantera_name },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.TheDestroyer ), golem_name },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Retinazer ), golem_name },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Spazmatism ), golem_name },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.SkeletronPrime ), golem_name },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Plantera ), moonlord_name },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Golem ), moonlord_name }
			};

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
				new ShopPackDefinition( wof_name, "Mimic's Lament Pack", 35, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.MagicDagger), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.TitanGlove), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.PhilosophersStone), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.CrossNecklace), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.StarCloak), 1 )
					} ),
				new ShopPackDefinition( wof_name, "Gangster's Pack", 35, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.Uzi), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.RifleScope), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.EndlessMusketPouch), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.GangstaHat), 1 )
					} ),
				new ShopPackDefinition( wof_name, "Avenger Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.AvengerEmblem), 1 )
					} ),
				new ShopPackDefinition( wof_name, "Life Pack", 150, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.AnkhCharm), 1 )
					} ),
				new ShopPackDefinition( wof_name, "Lucky Pack", 150, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.CoinRing), 1 )
					} ),
				new ShopPackDefinition( wof_name, "Dimensionalist's Pack", 200, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.RodofDiscord), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.TeleportationPotion), 30 )
					} ),

				// Post-plantera:
				new ShopPackDefinition( plantera_name, "Whack Pack", 50, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.TheAxe), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.Bananarang), 10 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.SlapHand), 1 )
					} ),

				// Post-golem:
				new ShopPackDefinition( golem_name, "Golem Eye Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.EyeoftheGolem), 1 )
					} ),
				new ShopPackDefinition( golem_name, "Defender's Pack", 150, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.CelestialShell), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.PaladinsShield), 1 ),
						new ShopPackItemDefinition( ItemIdentityHelpers.GetQualifiedName(ItemID.FrozenTurtleShell), 1 )
					} ),

				// Post-moonlord:
				new ShopPackDefinition( moonlord_name, "Eldritch Pack", 500, new ShopPackItemDefinition[] {
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
