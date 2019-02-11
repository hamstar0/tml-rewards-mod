using HamstarHelpers.Components.Config;
using Microsoft.Xna.Framework;
using System;


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
		public Color PointsDisplayColor = Color.YellowGreen;

		public bool SharedRewards = true;

		public bool InstantWayfarer = false;
		
		public bool UseUpdatedWorldFileNameConvention = true;



		////////////////

		public void SetDefaults() { }

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
