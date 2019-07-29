using HamstarHelpers.Components.Errors;
using System;
using Terraria;


namespace Rewards {
	public static partial class RewardsAPI {
		public static void ResetPlayerModData( Player player ) {    // <- In accordance with Mod Helpers convention
			var mymod = RewardsMod.Instance;
			mymod.SettingsConfig.Reset();
			mymod.PointsConfig.Reset();
			mymod.ShopConfig.Reset();

			//mymod.SettingsConfigJson.SaveFile();
			//mymod.PointsConfigJson.SaveFileAsync( () => { } );
			//mymod.ShopConfigJson.SaveFileAsync( () => { } );
		}
	}
}
