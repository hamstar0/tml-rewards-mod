using HamstarHelpers.Components.Config;
using System;
using System.Collections.Generic;


namespace Rewards.Configs {
	public partial class RewardsPointsConfigData : ConfigurationDataBase {
		public static string ConfigFileName => "Rewards Points Config.json";


		////////////////

		public string VersionSinceUpdate = "";

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
		//public bool NpcRewardPrediction = true;



		////////////////
		
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
