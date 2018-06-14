using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Config;
using HamstarHelpers.Utilities.Errors;
using HamstarHelpers.Utilities.Network;
using Rewards.Logic;
using Rewards.NetProtocols;
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



		////////////////
		
		internal JsonConfig<RewardsConfigData> ConfigJson;
		public RewardsConfigData Config { get { return ConfigJson.Data; } }

		public bool SuppressConfigAutoSaving { get; internal set; }

		private IList<Action<Player, float>> _OnPointsGainedHooks = new List<Action<Player, float>>();	// Is this needed...?!
		internal IList<Action<Player, float>> OnPointsGainedHooks {
			get {
				if( this._OnPointsGainedHooks == null ) {
					this._OnPointsGainedHooks = new List<Action<Player, float>>();
				}
				return this._OnPointsGainedHooks;
			}
		}
		private IList<Action<Player, string, float, Item[]>> _OnPointsSpentHooks = new List<Action<Player, string, float, Item[]>>();
		internal IList<Action<Player, string, float, Item[]>> OnPointsSpentHooks {
			get {
				if( this._OnPointsSpentHooks == null ) {
					this._OnPointsSpentHooks = new List<Action<Player, string, float, Item[]>>();
				}
				return this._OnPointsSpentHooks;
			}
		}
		


		////////////////

		public RewardsMod() {
			RewardsMod.Instance = this;

			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			this.ConfigJson = new JsonConfig<RewardsConfigData>( RewardsConfigData.ConfigFileName, ConfigurationDataBase.RelativePath );
		}

		public override void Load() {
			this.LoadConfigs();
		}


		private void LoadConfigs() {
			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
			}
			
			TmlLoadHelpers.AddPostModLoadPromise( () => {
				if( this.Config.CanUpdateVersion() ) {
					this.Config.UpdateToLatestVersion();

					ErrorLogger.Log( "Rewards updated to " + RewardsConfigData.ConfigVersion.ToString() );
					this.ConfigJson.SaveFile();
				}
			} );
		}

		
		public override void Unload() {
			RewardsMod.Instance = null;
		}


		////////////////

		public override void PreSaveAndQuit() {
			if( Main.netMode == 2 ) { return; }	// Redundant?

			if( Main.netMode == 0 ) {
				var myplayer = Main.LocalPlayer.GetModPlayer<RewardsPlayer>();
				myplayer.SaveKillData();
			} else if( Main.netMode == 1 ) {
				PacketProtocol.QuickSendToServer<PlayerSaveProtocol>();
			}
		}


		////////////////

		public override object Call( params object[] args ) {
			if( args.Length == 0 ) { throw new Exception( "Undefined call type." ); }

			string call_type = args[0] as string;
			if( args == null ) { throw new Exception("Invalid call type."); }

			var new_args = new object[ args.Length - 1 ];
			Array.Copy( args, 1, new_args, 0, args.Length - 1 );

			return RewardsAPI.Call( call_type, new_args );
		}


		////////////////
		
		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx != -1 ) {
				GameInterfaceDrawMethod draw_method = delegate {
					try {
						var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();
						KillData data = myworld.Logic.GetPlayerData( Main.LocalPlayer );

						if( data == null ) {
							throw new HamstarException( "RewardsMod.ModifyInterfaceLayers() - No player data for " + Main.LocalPlayer.name );
						}

						if( data.CanDrawPoints( RewardsMod.Instance ) ) {
							data.DrawPointScore( RewardsMod.Instance, Main.spriteBatch );
						}
					} catch( Exception ) { }

					return true;
				};

				var interface_layer = new LegacyGameInterfaceLayer( "Rewards: Points", draw_method,
					InterfaceScaleType.UI );

				layers.Insert( idx, interface_layer );
			}
		}
	}
}
