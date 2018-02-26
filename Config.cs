using HamstarHelpers.Utilities.Config;
using Microsoft.Xna.Framework;
using Rewards.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rewards {
	public partial class RewardsConfigData : ConfigurationDataBase {
		public static Version ConfigVersion { get { return new Version(1, 4, 5); } }
		public static string ConfigFileName { get { return "Rewards Config.json"; } }


		////////////////

		public string VersionSinceUpdate = RewardsConfigData.ConfigVersion.ToString();

		public bool DebugModeInfo = false;
		public bool DebugModeEnableCheats = false;
		public bool DebugModeSaveKillsAsJson = false;

		public bool ShowPoints = true;
		public bool ShowPointsInventoryOnly = false;

		public bool PointsDisplayWithoutInventory = true;
		public int PointsDisplayX = -76;
		public int PointsDisplayY = -60;
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
		public IDictionary<string, int> NpcRewardRequiredMinimumKills = new Dictionary<string, int>();
		public IDictionary<string, int> NpcRewardTogetherSets = new Dictionary<string, int>();

		public IList<ShopPackDefinition> ShopLoadout = new List<ShopPackDefinition>();



		////////////////

		public bool _OLD_SETTINGS_BELOW_ = true;

		public bool DebugModeSaveKillsAsText = false;
		public bool CommunismMode = false;



		////////////////
		
		
		
		public static readonly int _1_4_3_PointsDisplayX = -92;
		public static readonly int _1_4_3_PointsDisplayY = -48;

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
				this.NpcRewardRequiredMinimumKills = new_config.NpcRewardRequiredMinimumKills;
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
				} catch( InvalidOperationException _ ) {
					refresh = true;
				}

				if( refresh ) {
					this.NpcRewards = new_config.NpcRewards;
					this.NpcRewardRequiredMinimumKills = new_config.NpcRewardRequiredMinimumKills;
					this.ShopLoadout = new_config.ShopLoadout;
				}
			}

			this.VersionSinceUpdate = new_config.VersionSinceUpdate;

			return true;
		}
	}
}
