using HamstarHelpers.Utilities.Config;
using Rewards.NetProtocol;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace Rewards {
	class RewardsMod : Mod {
		public static RewardsMod Instance { get; private set; }

		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-rewards-mod"; } }

		public static string ConfigFileRelativePath {
			get { return JsonConfig<RewardsConfigData>.RelativePath + Path.DirectorySeparatorChar + RewardsConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( RewardsMod.Instance != null ) {
				if( !RewardsMod.Instance.JsonConfig.LoadFile() ) {
					RewardsMod.Instance.JsonConfig.SaveFile();
				}
			}
		}



		////////////////

		public bool IsContentSetup { get; private set; }
		internal JsonConfig<RewardsConfigData> JsonConfig;
		public RewardsConfigData Config { get { return JsonConfig.Data; } }


		////////////////

		public RewardsMod() {
			this.IsContentSetup = false;
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			this.JsonConfig = new JsonConfig<RewardsConfigData>( RewardsConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new RewardsConfigData() );
		}

		public override void Load() {
			RewardsMod.Instance = this;

			var hamhelpmod = ModLoader.GetMod( "HamstarHelpers" );
			var min_vers = new Version( 1, 2, 7 );
			if( hamhelpmod.Version < min_vers ) {
				throw new Exception( "Hamstar Helpers must be version " + min_vers.ToString() + " or greater." );
			}

			this.LoadConfigs();
		}

		private void LoadConfigs() {
			if( !this.JsonConfig.LoadFile() ) {
				this.JsonConfig.SaveFile();
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Rewards updated to " + RewardsConfigData.ConfigVersion.ToString() );
				this.JsonConfig.SaveFile();
			}
		}


		public override void PostSetupContent() {
			this.IsContentSetup = true;
		}


		public override void Unload() {
			RewardsMod.Instance = null;
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			if( Main.netMode == 1 ) {   // Client
				ClientPacketHandlers.HandlePacket( this, reader );
			} else if( Main.netMode == 2 ) {    // Server
				ServerPacketHandlers.HandlePacket( this, reader, player_who );
			}
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx != -1 ) {
				GameInterfaceDrawMethod draw_method = delegate {
					var myplayer = Main.LocalPlayer.GetModPlayer<RewardsPlayer>();

					myplayer.DrawPointScore( Main.spriteBatch );

					return true;
				};

				var interface_layer = new LegacyGameInterfaceLayer( "Rewards: Points", draw_method,
					InterfaceScaleType.UI );

				layers.Insert( idx, interface_layer );
			}
		}
	}
}
