using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Services.DataDumper;
using HamstarHelpers.Services.Promises;
using Rewards.Configs;
using Rewards.NetProtocols;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsMod : Mod {
		public static RewardsMod Instance { get; private set; }
		


		////////////////

		internal JsonConfig<RewardsSettingsConfigData> SettingsConfigJson;
		public RewardsSettingsConfigData SettingsConfig => SettingsConfigJson.Data;

		internal JsonConfig<RewardsPointsConfigData> PointsConfigJson;
		public RewardsPointsConfigData PointsConfig => PointsConfigJson.Data;

		internal JsonConfig<RewardsShopConfigData> ShopConfigJson;
		public RewardsShopConfigData ShopConfig => ShopConfigJson.Data;


		////

		public bool SuppressConfigAutoSaving { get; internal set; }


		////////////////

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

		////

		internal bool RecentlyUpdatedConfig = false;



		////////////////

		public RewardsMod() {
			RewardsMod.Instance = this;
			
			this.SettingsConfigJson = new JsonConfig<RewardsSettingsConfigData>( RewardsSettingsConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath );
			this.PointsConfigJson = new JsonConfig<RewardsPointsConfigData>( RewardsPointsConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath );
			this.ShopConfigJson = new JsonConfig<RewardsShopConfigData>( RewardsShopConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath );
		}

		public override void Load() {
			string depErr = TmlHelpers.ReportBadDependencyMods( this );
			if( depErr != null ) { throw new HamstarException( depErr ); }

			this.LoadConfigs();

			DataDumper.SetDumpSource( "Rewards", () => {
				if( Main.netMode == 2 ) {
					return "  No 'current player' for server";
				}
				if( Main.myPlayer < 0 || Main.myPlayer >= Main.player.Length || Main.LocalPlayer == null || !Main.LocalPlayer.active ) {
					return "  Invalid player data";
				}

				var myplayer = Main.LocalPlayer.GetModPlayer<RewardsPlayer>();

				return "  IsFullySynced: " + myplayer.IsFullySynced
					+ ", HasKillData: " + myplayer.HasKillData
					+ ", HasModSettings: " + myplayer.HasModSettings;
			} );
		}


		private void LoadConfigs() {
			if( !this.SettingsConfigJson.LoadFile() ) {
				ErrorLogger.Log( "Creating rewards configs anew..." );
				this.SettingsConfigJson.SaveFile();
			}
			if( !this.PointsConfigJson.LoadFile() ) {
				ErrorLogger.Log( "Creating rewards points configs anew..." );
				this.PointsConfigJson.SaveFile();
			}
			if( !this.ShopConfigJson.LoadFile() ) {
				ErrorLogger.Log( "Creating rewards shop configs anew..." );
				this.ShopConfigJson.SaveFile();
			}

			bool isLoaded = false;
			
			Promises.AddPostModLoadPromise( () => {
				if( !isLoaded ) { isLoaded = true; }	// <- Paranoid failsafe?
				else { return; }
				
				if( this.SettingsConfig.CanUpdateVersion( out this.RecentlyUpdatedConfig ) ) {
					this.SettingsConfig.UpdateToLatestVersion();
					ErrorLogger.Log( "Rewards settings updated to " + this.Version.ToString() );

					this.SettingsConfigJson.SaveFile();
				}

				if( this.PointsConfig.CanUpdateVersion() ) {
					this.PointsConfig.UpdateToLatestVersion();
					ErrorLogger.Log( "Rewards points settings to " + this.Version.ToString() );
					
					this.PointsConfigJson.SaveFileAsync( () => { } );
				}

				if( this.ShopConfig.CanUpdateVersion() ) {
					this.ShopConfig.UpdateToLatestVersion();
					ErrorLogger.Log( "Rewards shop settings updated to " + this.Version.ToString() );
					
					this.ShopConfigJson.SaveFileAsync( () => { } );
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
				PacketProtocolSendToServer.QuickSendToServer<PlayerSaveProtocol>();
			}
		}


		////////////////

		public override object Call( params object[] args ) {
			if( args == null || args.Length == 0 ) { throw new HamstarException( "Undefined call type." ); }

			string callType = args[0] as string;
			if( callType == null ) { throw new HamstarException( "Invalid call type." ); }

			var methodInfo = typeof( RewardsAPI ).GetMethod( callType );
			if( methodInfo == null ) { throw new HamstarException( "Invalid call type " + callType ); }

			var newArgs = new object[args.Length - 1];
			Array.Copy( args, 1, newArgs, 0, args.Length - 1 );

			try {
				return ReflectionHelpers.SafeCall( methodInfo, null, newArgs );
			} catch( Exception e ) {
				throw new HamstarException( "Barriers.BarrierMod.Call - Bad API call.", e );
			}
		}
	}
}
