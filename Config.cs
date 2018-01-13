using HamstarHelpers.Utilities.Config;
using Microsoft.Xna.Framework;
using Rewards.Items;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace Rewards {
	public class RewardsConfigData : ConfigurationDataBase {
		public static Version ConfigVersion { get { return new Version(1, 0, 0, 1); } }
		public static string ConfigFileName { get { return "Rewards Config.json"; } }


		////////////////

		public string VersionSinceUpdate = RewardsConfigData.ConfigVersion.ToString();

		public bool DebugModeEnableCheats = false;

		public bool PointsDisplayWithoutInventory = true;
		public int PointsDisplayX = -92;
		public int PointsDisplayY = -48;
		public Color PointsDisplayColor = Color.YellowGreen;
		
		public bool CommunismMode = false;

		public float GrindKillMultiplier = 0.1f;

		public float GoblinInvasionReward = 15f;
		public float FrostLegionInvasionReward = 15f;
		public float PirateInvasionReward = 25f;
		public float MartianInvasionReward = 200f;
		public float PumpkingMoonWaveReward = 10f;
		public float FrostMoonWaveReward = 10f;

		public IDictionary<string, float> NpcRewards = new Dictionary<string, float>();
		public IDictionary<string, int> NpcRewardRequiredMinimumKills = new Dictionary<string, int>();
		public IDictionary<string, int> NpcRewardTogetherSets = new Dictionary<string, int>();

		public IList<ShopPackDefinition> ShopLoadout = new List<ShopPackDefinition>();



		////////////////
		
		public override void OnLoad( bool success ) {
			if( RewardsMod.Instance.IsContentSetup ) {
				foreach( var info in this.ShopLoadout ) {
					if( !info.Validate() ) {
						ErrorLogger.Log( "Could not validate shop item " + info.Name );
					}
				}
			}
			
			if( !success ) {
				this.SetDefaults();
			}
		}


		public bool SetDefaults() {
			this.NpcRewards = new Dictionary<string, float> {
				{ "King Slime", 10f },
				{ "Eye of Cthulhu", 10f },
				{ "Eater of Worlds", 25f / 50f },	// per segment
				{ "Brain of Cthulhu", 25f },
				{ "Queen Bee", 20f },
				{ "Skeletron", 35f },
				{ "Wall of Flesh", 75f },
				{ "The Destroyer", 50f },
				{ "Retinazer", 50f / 2 },
				{ "Spazmatism", 50f / 2 },
				{ "Skeletron Prime", 50f },
				{ "Plantera", 100f },
				{ "Golem", 100f },
				{ "Duke Fishron", 100f },
				{ "Lunatic Cultist", 50f },
				{ "Betsy", 100f },
				{ "Solar Pillar", 50f },
				{ "Vortex Pillar", 50f },
				{ "Nebula Pillar", 50f },
				{ "Stardust Pillar", 50f },
				{ "Moon Lord", 250f }
			};
			this.NpcRewardRequiredMinimumKills = new Dictionary<string, int> {
				{ "Eater of Worlds", 50 }
			};
			this.NpcRewardTogetherSets = new Dictionary<string, int> {
				{ "Retinazer", 1 },
				{ "Spazmatism", 1 }
			};

			this.ShopLoadout = new List<ShopPackDefinition> {
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, false, false,
					"Money Purse", 5, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Gold Coin", 99 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, false, false,
					"Iron Age Pack", 5, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Iron Bar", 99 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, false, false,
					"Nature Watcher's Pack", 5, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Flower Boots", 1 ),
						new ShopPackItemDefinition( "Binoculars", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, false, false,
					"Bindings Pack", 10, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Band of Regeneration", 1 ),
						new ShopPackItemDefinition( "Band of Starpower", 1 ),
						new ShopPackItemDefinition( "Celestial Magnet", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, false, false,
					"Traveler's Pack", 20, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Aglet", 1 ),
						new ShopPackItemDefinition( "Ice Skates", 1 ),
						new ShopPackItemDefinition( "Tiger Climbing Gear", 1 ),
						new ShopPackItemDefinition( "Water Walking Boots", 1 ),
						new ShopPackItemDefinition( "Hand Warmer", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, false, false,
					"Rough Traveler's Pack", 20, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Anklet of the Wind", 1 ),
						new ShopPackItemDefinition( "Feral Claws", 1 ),
						new ShopPackItemDefinition( "Jellyfish Diving Gear", 1 ),
						new ShopPackItemDefinition( "Lava Charm", 1 ),
						new ShopPackItemDefinition( "Magma Stone", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, false, false,
					"Ground Hater's Pack", 15, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Sandstorm in a Bottle", 1 ),
						new ShopPackItemDefinition( "Shiny Red Balloon", 1 ),
						new ShopPackItemDefinition( "Lucky Horseshoe", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, false, false,
					"Stoicist's Pack", 15, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Cobalt Shield", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, false, false,
					"Alucard's Pack", 20, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Arkhalis", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, false, false,
					"Gizmo Pack", 20, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Toolbox", 1 ),
						new ShopPackItemDefinition( "Architect Gizmo Pack", 1 ),
						new ShopPackItemDefinition( "Presserator", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, false, false,
					"Information Monger's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "GPS", 1 ),
						new ShopPackItemDefinition( "Goblin Tech", 1 ),
						new ShopPackItemDefinition( "R.E.K. 3000", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, false, false,
					"Fisherman's Pack", 50, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Angler Tackle Bag", 1 ),
						new ShopPackItemDefinition( "Fish Finder", 1 )
					} ),

				// Hard mode only:
				new ShopPackDefinition(
					false, false, false, false, false, false, /**/true, false, false, false, false, false, false,
					"Mimic's Lament Pack", 20, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Magic Dagger", 1 ),
						new ShopPackItemDefinition( "Titan Glove", 1 ),
						new ShopPackItemDefinition( "Philosopher's Stone", 1 ),
						new ShopPackItemDefinition( "Cross Necklace", 1 ),
						new ShopPackItemDefinition( "Star Cloak", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, /**/true, false, false, false, false, false, false,
					"Gangster's Pack", 35, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Uzi", 1 ),
						new ShopPackItemDefinition( "Rifle Scope", 1 ),
						new ShopPackItemDefinition( "Endless Musket Pouch", 1 ),
						new ShopPackItemDefinition( "Gangsta Hat", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, /**/true, false, false, false, false, false, false,
					"Avenger Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Avenger Emblem", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, /**/true, false, false, false, false, false, false,
					"Life Pack", 75, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Ankh Charm", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, /**/true, false, false, false, false, false, false,
					"Lucky Pack", 100, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Coin Ring", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, /**/true, false, false, false, false, false, false,
					"Dimensionalist's Pack", 100, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Rod of Discord", 1 ),
						new ShopPackItemDefinition( "Teleportation Potion", 30 )
					} ),

				// Post-plantera:
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, /**/true, false, false,
					"Whack Pack", 45, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "The Axe", 1 ),
						new ShopPackItemDefinition( "Bananarang", 10 ),
						new ShopPackItemDefinition( "Slap Hand", 1 )
					} ),
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, /**/true, false, false,
					"Defender's Pack", 60, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Celestial Shell", 1 ),
						new ShopPackItemDefinition( "Paladin's Shield", 1 ),
						new ShopPackItemDefinition( "Frozen Turtle Shell", 1 )
					} ),

				// Post-golem:
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, /**/true, false,
					"Golem Eye Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Eye of the Golem", 1 )
					} ),

				// Post-moonlord:
				new ShopPackDefinition(
					false, false, false, false, false, false, false, false, false, false, false, false, true,
					"Eldritch Pack", 300, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Meowmere", 1 ),
						new ShopPackItemDefinition( "Terrarian", 1 ),
						new ShopPackItemDefinition( "Star Wrath", 1 ),
						new ShopPackItemDefinition( "S.D.M.G.", 1 ),
						new ShopPackItemDefinition( "Celebration", 1 ),
						new ShopPackItemDefinition( "Last Prism", 1 ),
						new ShopPackItemDefinition( "Lunar Flare", 1 ),
						new ShopPackItemDefinition( "Rainbow Crystal Staff", 1 ),
						new ShopPackItemDefinition( "Lunar Portal Staff", 1 )
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



		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new RewardsConfigData();
			new_config.SetDefaults();

			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= RewardsConfigData.ConfigVersion ) {
				return false;
			}

			if( vers_since < new Version( 1, 0, 0, 1 ) ) {
				this.PointsDisplayWithoutInventory = new_config.PointsDisplayWithoutInventory;
				this.PointsDisplayX = new_config.PointsDisplayX;
				this.PointsDisplayY = new_config.PointsDisplayY;
				this.PointsDisplayColor = new_config.PointsDisplayColor;
				this.CommunismMode = new_config.CommunismMode;
				this.GrindKillMultiplier = new_config.GrindKillMultiplier;
				this.GoblinInvasionReward = new_config.GoblinInvasionReward;
				this.FrostLegionInvasionReward = new_config.FrostLegionInvasionReward;
				this.PirateInvasionReward = new_config.PirateInvasionReward;
				this.MartianInvasionReward = new_config.MartianInvasionReward;
				this.PumpkingMoonWaveReward = new_config.PumpkingMoonWaveReward;
				this.FrostMoonWaveReward = new_config.FrostMoonWaveReward;
				this.NpcRewards = new_config.NpcRewards;
				this.NpcRewardRequiredMinimumKills = new_config.NpcRewardRequiredMinimumKills;
				this.NpcRewardTogetherSets = new_config.NpcRewardTogetherSets;
				this.ShopLoadout = new_config.ShopLoadout;
			}

			this.VersionSinceUpdate = RewardsConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}
