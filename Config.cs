using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.NPCHelpers;
using Microsoft.Xna.Framework;
using Rewards.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;


namespace Rewards {
	public partial class RewardsConfigData : ConfigurationDataBase {
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

		public static bool UpdatePackIfFound( string name, int oldPrice, IList<ShopPackDefinition> oldShop, IList<ShopPackDefinition> newShop ) {
			for( int i = 0; i < oldShop.Count; i++ ) {
				if( oldShop[i].Name != name ) { continue; }

				for( int j = 0; j < newShop.Count; j++ ) {
					if( newShop[j].Name != name ) { continue; }

					if( oldShop[i].Price == oldPrice ) {
						oldShop[i] = newShop[j];
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
			var versSince = new Version( this.VersionSinceUpdate );
			return versSince < RewardsMod.Instance.Version;
		}
		
		public bool UpdateToLatestVersion() {
			var mymod = RewardsMod.Instance;
			var newConfig = new RewardsConfigData();
			newConfig.SetDefaults();

			var versSince = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( this.VersionSinceUpdate == "" ) {
				this.SetDefaults();
			}

			this.VersionSinceUpdate = mymod.Version.ToString();

			return true;
		}
	}
}
