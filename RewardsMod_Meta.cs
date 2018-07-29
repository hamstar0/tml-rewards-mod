using HamstarHelpers.Components.Config;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsMod : Mod {
		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-rewards-mod"; } }

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + RewardsConfigData.ConfigFileName; }
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
				if( !mymod.ConfigJson.LoadFile() ) {
					mymod.ConfigJson.SaveFile();
				}
			}
		}
		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}

			var config_data = new RewardsConfigData();
			config_data.SetDefaults();
			
			RewardsMod.Instance.ConfigJson.SetData( config_data );
			RewardsMod.Instance.ConfigJson.SaveFile();
		}
	}
}
