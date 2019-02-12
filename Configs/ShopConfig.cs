using HamstarHelpers.Components.Config;
using Rewards.Items;
using System;
using System.Collections.Generic;


namespace Rewards.Configs {
	public partial class RewardsShopConfigData : ConfigurationDataBase {
		public static string ConfigFileName => "Rewards Shop Config.json";

		
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

		public string VersionSinceUpdate = "";
		
		public IList<ShopPackDefinition> ShopLoadout = new List<ShopPackDefinition>();



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
