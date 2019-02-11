using HamstarHelpers.Components.Config;
using Rewards.Configs;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-rewards-mod";

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + RewardsSettingsConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( RewardsMod.Instance != null ) {
				var mymod = RewardsMod.Instance;

				if( mymod.SuppressConfigAutoSaving ) {
					Main.NewText( "Rewards config settings auto saving suppressed." );
					return;
				}

				mymod.SettingsConfigJson.LoadFileAsync( ( success ) => {
					if( success ) { return; }
					mymod.SettingsConfigJson.SaveFile();
				} );

				mymod.PointsConfigJson.LoadFileAsync( ( success ) => {
					if( success ) { return; }
					mymod.PointsConfigJson.SaveFile();
				} );

				mymod.ShopConfigJson.LoadFileAsync( ( success ) => {
					if( success ) { return; }
					mymod.PointsConfigJson.SaveFile();
				} );
			}
		}
		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}

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

			mymod.SettingsConfigJson.SaveFile();
			mymod.PointsConfigJson.SaveFileAsync( () => { } );
			mymod.ShopConfigJson.SaveFileAsync( () => { } );
		}
	}
}
