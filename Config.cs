using HamstarHelpers.ItemHelpers;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.Utilities.Config;
using Microsoft.Xna.Framework;
using Rewards.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;


namespace Rewards {
	public partial class RewardsConfigData : ConfigurationDataBase {
		public static Version ConfigVersion { get { return new Version(1, 5, 0); } }
		public static string ConfigFileName { get { return "Rewards Config.json"; } }


		////////////////

		public string VersionSinceUpdate = RewardsConfigData.ConfigVersion.ToString();

		public bool DebugModeInfo = false;
		public bool DebugModeEnableCheats = false;
		public bool DebugModeSaveKillsAsJson = false;

		public bool ShowPoints = true;
		public bool ShowPointsPopups = true;

		public bool PointsDisplayWithoutInventory = false;
		public int PointsDisplayX = 448;
		public int PointsDisplayY = 1;
		public Color PointsDisplayColor = Color.YellowGreen;

		public bool SharedRewards = false;

		public float GrindKillMultiplier = 0.1f;

		public float GoblinInvasionReward = 15f;
		public float FrostLegionInvasionReward = 15f;
		public float PirateInvasionReward = 25f;
		public float MartianInvasionReward = 200f;
		public float PumpkingMoonWaveReward = 10f;
		public float FrostMoonWaveReward = 10f;

		public bool InstantWayfarer = false;

		public IDictionary<string, float> NpcRewards = new Dictionary<string, float>();
		public IDictionary<string, int> NpcRewardTogetherSets = new Dictionary<string, int>();
		public ISet<string> NpcRewardRequiredAsBoss = new HashSet<string>();
		public IDictionary<string, string> NpcRewardNotGivenAfterNpcKilled = new Dictionary<string, string>();
		//public bool NpcRewardPrediction = true;

		public IList<ShopPackDefinition> ShopLoadout = new List<ShopPackDefinition>();

		public bool UseUpdatedWorldFileNameConvention = true;



		////////////////

		public bool _OLD_SETTINGS_BELOW_ = true;

		public bool DebugModeSaveKillsAsText = false;
		public bool CommunismMode = false;



		////////////////
		
		
		
		public static readonly int _1_4_3_PointsDisplayX = -92;
		public static readonly int _1_4_3_PointsDisplayY = -48;

		public static readonly int _1_4_6_Reward_LunarTower = 50;
		public static readonly int _1_4_6_Pack_Mimic = 25;
		public static readonly int _1_4_6_Pack_Avenger = 20;
		public static readonly int _1_4_6_Pack_Life = 100;
		public static readonly int _1_4_6_Pack_Lucky = 125;
		public static readonly int _1_4_6_Pack_Dimensionalist = 150;
		public static readonly int _1_4_6_Pack_Golem = 20;
		public static readonly int _1_4_6_Pack_Defender = 100;
		public static readonly int _1_4_6_Pack_Eldritch = 350;

		public static readonly int _1_4_7_PointsDisplayX = -76;
		public static readonly int _1_4_7_PointsDisplayY = -60;

		public static readonly int _1_4_8_PointsDisplayY = 26;
		
		public static readonly int _1_4_8_1_PointsDisplayX = 500;
		public static readonly int _1_4_8_1_PointsDisplayY = 6;
		
		public static readonly int _1_4_9_PointsDisplayY = 2;

		////////////////

		public static bool UpdatePackIfFound( string name, int old_price, IList<ShopPackDefinition> old_shop, IList<ShopPackDefinition> new_shop ) {
			for( int i = 0; i < old_shop.Count; i++ ) {
				if( old_shop[i].Name != name ) { continue; }

				for( int j = 0; j < new_shop.Count; j++ ) {
					if( new_shop[j].Name != name ) { continue; }

					if( old_shop[i].Price == old_price ) {
						old_shop[i] = new_shop[j];
						return true;
					}
					return false;
				}
			}
			return false;
		}


		////////////////

		public bool CanUpdateVersion() {
			if( this.VersionSinceUpdate == "" ) { return true; }
			var vers_since = new Version( this.VersionSinceUpdate );
			return vers_since < RewardsConfigData.ConfigVersion;
		}
		
		public void UpdateToLatestVersion() {
			var new_config = new RewardsConfigData();
			new_config.SetDefaults();

			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since < new Version(1, 2, 1) ) {		// Sorry :/
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
				this.NpcRewardTogetherSets = new_config.NpcRewardTogetherSets;
				this.ShopLoadout = new_config.ShopLoadout;
			}
			if( vers_since < new Version( 1, 3, 0 ) ) {     // Sorry :/
				this.NpcRewards = new_config.NpcRewards;
				this.ShopLoadout = new_config.ShopLoadout;
			}
			if( vers_since < new Version( 1, 4, 4 ) ) {
				if( this.PointsDisplayX == RewardsConfigData._1_4_3_PointsDisplayX ) {
					this.PointsDisplayX = new_config.PointsDisplayX;
				}
				if( this.PointsDisplayY == RewardsConfigData._1_4_3_PointsDisplayY ) {
					this.PointsDisplayY = new_config.PointsDisplayY;
				}
			}
			if( vers_since < new Version( 1, 4, 5 ) ) {     // Sorry again
				bool refresh = false;

				try {
					var first_npc_reward = this.NpcRewards.First();
					var first_shop_pack = this.ShopLoadout.First();
					string _err;
					refresh = !first_shop_pack.Validate( out _err );
				} catch( InvalidOperationException ) {
					refresh = true;
				}

				if( refresh ) {
					this.NpcRewards = new_config.NpcRewards;
					this.ShopLoadout = new_config.ShopLoadout;
				}
			}
			if( vers_since < new Version( 1, 4, 7 ) ) {
				RewardsConfigData.UpdatePackIfFound( "Mimic's Lament Pack", RewardsConfigData._1_4_6_Pack_Mimic, this.ShopLoadout, new_config.ShopLoadout );
				RewardsConfigData.UpdatePackIfFound( "Avenger Pack", RewardsConfigData._1_4_6_Pack_Avenger, this.ShopLoadout, new_config.ShopLoadout );
				RewardsConfigData.UpdatePackIfFound( "Life Pack", RewardsConfigData._1_4_6_Pack_Life, this.ShopLoadout, new_config.ShopLoadout );
				RewardsConfigData.UpdatePackIfFound( "Lucky Pack", RewardsConfigData._1_4_6_Pack_Lucky, this.ShopLoadout, new_config.ShopLoadout );
				RewardsConfigData.UpdatePackIfFound( "Dimensionalist's Pack", RewardsConfigData._1_4_6_Pack_Dimensionalist, this.ShopLoadout, new_config.ShopLoadout );
				RewardsConfigData.UpdatePackIfFound( "Golem Eye Pack", RewardsConfigData._1_4_6_Pack_Golem, this.ShopLoadout, new_config.ShopLoadout );
				RewardsConfigData.UpdatePackIfFound( "Defender's Pack", RewardsConfigData._1_4_6_Pack_Defender, this.ShopLoadout, new_config.ShopLoadout );
				RewardsConfigData.UpdatePackIfFound( "Eldritch Pack", RewardsConfigData._1_4_6_Pack_Eldritch, this.ShopLoadout, new_config.ShopLoadout );

				string solar_tower = NPCIdentityHelpers.GetQualifiedName( NPCID.LunarTowerSolar );
				string vortex_tower = NPCIdentityHelpers.GetQualifiedName( NPCID.LunarTowerVortex );
				string nebula_tower = NPCIdentityHelpers.GetQualifiedName( NPCID.LunarTowerNebula );
				string stardust_tower = NPCIdentityHelpers.GetQualifiedName( NPCID.LunarTowerStardust );

				if( this.NpcRewards.ContainsKey(solar_tower) && this.NpcRewards[solar_tower] == RewardsConfigData._1_4_6_Reward_LunarTower ) {
					this.NpcRewards[ solar_tower ] = new_config.NpcRewards[ solar_tower ];
				}
				if( this.NpcRewards.ContainsKey( vortex_tower ) && this.NpcRewards[vortex_tower] == RewardsConfigData._1_4_6_Reward_LunarTower ) {
					this.NpcRewards[ vortex_tower ] = new_config.NpcRewards[ vortex_tower ];
				}
				if( this.NpcRewards.ContainsKey( nebula_tower ) && this.NpcRewards[nebula_tower] == RewardsConfigData._1_4_6_Reward_LunarTower ) {
					this.NpcRewards[ nebula_tower ] = new_config.NpcRewards[ nebula_tower ];
				}
				if( this.NpcRewards.ContainsKey( stardust_tower ) && this.NpcRewards[stardust_tower] == RewardsConfigData._1_4_6_Reward_LunarTower ) {
					this.NpcRewards[stardust_tower] = new_config.NpcRewards[stardust_tower];
				}
			}
			if( vers_since < new Version( 1, 4, 8 ) ) {
				if( this.PointsDisplayX == RewardsConfigData._1_4_7_PointsDisplayX ) {
					this.PointsDisplayX = new_config.PointsDisplayX;
				}
				if( this.PointsDisplayY == RewardsConfigData._1_4_7_PointsDisplayY ) {
					this.PointsDisplayY = new_config.PointsDisplayY;
				}
				this.PointsDisplayWithoutInventory = new_config.PointsDisplayWithoutInventory;
			}
			if( vers_since < new Version( 1, 4, 8, 1 ) ) {
				if( this.PointsDisplayY == RewardsConfigData._1_4_8_PointsDisplayY ) {
					this.PointsDisplayY = new_config.PointsDisplayY;
				}
				this.PointsDisplayWithoutInventory = new_config.PointsDisplayWithoutInventory;
			}
			if( vers_since < new Version( 1, 4, 8, 2 ) ) {
				if( this.PointsDisplayX == RewardsConfigData._1_4_8_1_PointsDisplayX ) {
					this.PointsDisplayX = new_config.PointsDisplayX;
				}
				if( this.PointsDisplayY == RewardsConfigData._1_4_8_1_PointsDisplayY ) {
					this.PointsDisplayY = new_config.PointsDisplayY;
				}
			}
			if( vers_since < new Version( 1, 4, 10 ) ) {
				if( this.PointsDisplayY == RewardsConfigData._1_4_9_PointsDisplayY ) {
					this.PointsDisplayY = new_config.PointsDisplayY;
				}
			}
			if( vers_since < new Version( 1, 4, 15 ) ) {
				this.UseUpdatedWorldFileNameConvention = false;
			}
			if( vers_since < new Version( 1, 5, 0 ) ) {
				for( int i=0; i<this.ShopLoadout.Count; i++ ) {
					ShopPackDefinition def = this.ShopLoadout[i];

					if( def.Name == "Money Purse" ) {
						def.Price = 10;
						this.ShopLoadout[i] = def;
						break;
					}
				}
			}
			if( vers_since < new Version( 1, 5, 0, 1 ) ) {
				this.NpcRewardNotGivenAfterNpcKilled = new_config.NpcRewardNotGivenAfterNpcKilled;
				if( vers_since < new Version( 1, 5, 0 ) ) {
					this.NpcRewardRequiredAsBoss = new_config.NpcRewardRequiredAsBoss;
				}

				for( int i = 0; i < this.ShopLoadout.Count; i++ ) {
					ShopPackDefinition def = this.ShopLoadout[i];

					for( int j = 0; j < new_config.ShopLoadout.Count; j++ ) {
						ShopPackDefinition def2 = new_config.ShopLoadout[j];

						if( def.Name == def2.Name ) {
							def.NeededBossKill = def2.NeededBossKill;
							this.ShopLoadout[i] = def;
							break;
						}
					}
				}
			}

			this.VersionSinceUpdate = new_config.VersionSinceUpdate;
		}
	}
}
