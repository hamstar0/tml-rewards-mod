using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.DataDumper;
using HamstarHelpers.Services.Promises;
using Rewards.NetProtocols;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsMod : Mod {
		public static RewardsMod Instance { get; private set; }
		


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
			
			this.ConfigJson = new JsonConfig<RewardsConfigData>( RewardsConfigData.ConfigFileName, ConfigurationDataBase.RelativePath );
		}

		public override void Load() {
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
			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
			}
			
			Promises.AddPostModLoadPromise( () => {
				if( this.Config.CanUpdateVersion() ) {
					this.Config.UpdateToLatestVersion();

					ErrorLogger.Log( "Rewards updated to " + this.Version.ToString() );
					
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
