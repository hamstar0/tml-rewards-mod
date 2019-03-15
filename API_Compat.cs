using HamstarHelpers.Components.Errors;
using Rewards.Configs;
using System;
using Terraria;


namespace Rewards {
	public static partial class RewardsAPI {
		public static void ResetPlayerModData( Player player ) {    // <- In accordance with Mod Helpers convention
			var mymod = RewardsMod.Instance;
			var settingsConfigData = new RewardsSettingsConfigData();
			var pointsConfigData = new RewardsPointsConfigData();
			var shopConfigData = new RewardsShopConfigData();

			settingsConfigData.SetDefaults();
			pointsConfigData.SetDefaults();
			shopConfigData.SetDefaults();

			mymod.SettingsConfigJson.SetData( settingsConfigData );
			mymod.PointsConfigJson.SetData( pointsConfigData );
			mymod.ShopConfigJson.SetData( shopConfigData );

			//mymod.SettingsConfigJson.SaveFile();
			//mymod.PointsConfigJson.SaveFileAsync( () => { } );
			//mymod.ShopConfigJson.SaveFileAsync( () => { } );
		}
	}
}
