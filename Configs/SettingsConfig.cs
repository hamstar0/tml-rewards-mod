using HamstarHelpers.Components.Config;
using Microsoft.Xna.Framework;
using Rewards.Items;
using System;
using System.Collections.Generic;

namespace Rewards.Configs {
	public partial class RewardsSettingsConfigData : ConfigurationDataBase {
		public static string ConfigFileName => "Rewards Config.json";


		////////////////

		public string VersionSinceUpdate = "";

		public bool DebugModeInfo = false;
		public bool DebugModeKillInfo = false;
		public bool DebugModeEnableCheats = false;
		public bool DebugModeSaveKillsAsJson = false;

		public bool ShowPoints = true;
		public bool ShowPointsPopups = true;

		public bool PointsDisplayWithoutInventory = false;
		public int PointsDisplayX = 448;
		public int PointsDisplayY = 1;
		public byte[] PointsDisplayColorRGB = new byte[] { Color.YellowGreen.R, Color.YellowGreen.G, Color.YellowGreen.B };

		public bool SharedRewards = true;

		public bool InstantWayfarer = false;
		
		public bool UseUpdatedWorldFileNameConvention = true;

		////

		public string _OBSOLETE_SETTINGS_BELOW_ = "";

		 public float GrindKillMultiplier = 0.1f;

		 public float GoblinInvasionReward = 15f;
		 public float FrostLegionInvasionReward = 15f;
		 public float PirateInvasionReward = 25f;
		 public float MartianInvasionReward = 200f;
		 public float PumpkingMoonWaveReward = 10f;
		 public float FrostMoonWaveReward = 10f;

		 public IDictionary<string, float> NpcRewards = new Dictionary<string, float>();
		 public IDictionary<string, int> NpcRewardTogetherSets = new Dictionary<string, int>();
		 public ISet<string> NpcRewardRequiredAsBoss = new HashSet<string>();
		 public IDictionary<string, string> NpcRewardNotGivenAfterNpcKilled = new Dictionary<string, string>();

		 public IList<ShopPackDefinition> ShopLoadout = new List<ShopPackDefinition>();



		////////////////

		public void SetDefaults() {
			this.PointsDisplayColorRGB = new byte[] { Color.YellowGreen.R, Color.YellowGreen.G, Color.YellowGreen.B };
		}

		////

		public bool CanUpdateVersion() {
			if( this.VersionSinceUpdate == "" ) { return true; }
			var versSince = new Version( this.VersionSinceUpdate );
			return versSince < RewardsMod.Instance.Version;
		}
		
		public void UpdateToLatestVersion() {
			var mymod = RewardsMod.Instance;
			var newConfig = new RewardsSettingsConfigData();
			newConfig.SetDefaults();

			var versSince = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( this.VersionSinceUpdate == "" ) {
				this.SetDefaults();
			}

			this.VersionSinceUpdate = mymod.Version.ToString();
		}
	}
}
