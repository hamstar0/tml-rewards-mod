using HamstarHelpers.Utilities.Config;
using Microsoft.Xna.Framework;
using Rewards.Items;
using System;
using System.Collections.Generic;


namespace Rewards {
	public class RewardsConfigData : ConfigurationDataBase {
		public static Version ConfigVersion { get { return new Version(1, 0, 0); } }
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
		public float FrostLegionInvasionReward = 25f;
		public float PirateInvasionReward = 50f;
		public float MartianInvasionReward = 250f;
		public float PumpkingMoonWaveReward = 20f;
		public float FrostMoonWaveReward = 20f;

		public IDictionary<string, float> NpcRewards;
		public IDictionary<string, int> NpcRewardTogetherSets;

		public IList<ShopPackDefinition> ShopLoadout;



		////////////////

		public override void OnLoad( bool success ) {
			if( success ) {
				foreach( var info in this.ShopLoadout ) {
					if( !info.Validate() ) {
						throw new Exception( "Could not validate shop item "+info.Name );
					}
				}
				return;
			}


			this.NpcRewards = new Dictionary<string, float> {
				{ "King Slime", 5f },
				{ "Eye of Cthulhu", 5f },
				{ "Eater of Worlds", 25f / 50f },	// per segment
				{ "Brain of Cthulhu", 25f },
				{ "Queen Bee", 15f },
				{ "Skeletron", 25f },
				{ "Wall of Flesh", 100f },
				{ "The Destroyer", 100f },
				{ "Retinazer", 100f },
				{ "Spazmatism", 100f },
				{ "Skeletron Prime", 100f },
				{ "Plantera", 250f },
				{ "Golem", 250f },
				{ "Duke Fishron", 350f },
				{ "Lunatic Cultist", 250f },
				{ "Betsy", 250f },
				{ "Solar Pillar", 200f },
				{ "Vortex Pillar", 200f },
				{ "Nebula Pillar", 200f },
				{ "Stardust Pillar", 200f },
				{ "Moon Lord", 1000f },
			};
			this.NpcRewardTogetherSets = new Dictionary<string, int> {
				{ "Retinazer", 1 },
				{ "Spazmatism", 1 },
			};

			this.ShopLoadout = new List<ShopPackDefinition> {
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
				//new ShopPackInfo( "Time Traveler's Pack", 35, new ShopItemInfo[] {
				//	new ShopItemInfo( "Time Capsule", 1 )
				//} )
			};
		}


		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new RewardsConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= RewardsConfigData.ConfigVersion ) {
				return false;
			}

			this.VersionSinceUpdate = RewardsConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}
