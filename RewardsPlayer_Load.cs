using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Rewards.Logic;
using Rewards.NetProtocols;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsPlayer : ModPlayer {
		internal readonly static object MyValidatorKey;
		public readonly static PromiseValidator EnterWorldValidator;


		////////////////

		static RewardsPlayer() {
			RewardsPlayer.MyValidatorKey = new object();
			RewardsPlayer.EnterWorldValidator = new PromiseValidator( RewardsPlayer.MyValidatorKey );
		}



		////////////////

		private void OnConnectSingle() {
			var mymod = (RewardsMod)this.mod;

			this.IsFullySynced = false;
			this.HasKillData = false;
			this.HasModSettings = false;

			if( !mymod.SuppressConfigAutoSaving ) {
				mymod.ConfigJson.LoadFileAsync( ( success ) => {
					if( !success ) {
						//mymod.ConfigJson.SaveFile();
						LogHelpers.Alert( "Rewards config could not be loaded." );
						Main.NewText( "Invalid config file. Consider using the /rew-shop-add command or a JSON editor program or site.", Color.Red );
					}
					this.FinishLocalKillDataSync();
					this.FinishLocalModSettingsSync();
				} );
			}
		}

		private void OnConnectCurrentClient() {
			if( this.player == null ) {
				LogHelpers.Warn( "Player null" );
			}
			if( !this.player.active ) {
				LogHelpers.Warn( "Player " + this.player.name + " (" + this.player.whoAmI + ") not active" );
			}
			if( Main.player[this.player.whoAmI] != this.player ) {
				LogHelpers.Warn( "Player " + this.player.name + " (" + this.player.whoAmI + ") not found in Main array for some reason..." );
			}

			this.IsFullySynced = false;
			this.HasKillData = false;
			this.HasModSettings = false;

			if( RewardsMod.Instance.Config.DebugModeInfo ) {
				LogHelpers.Alert( "Requesting kill data and mod settings from server..." );
			}

			PacketProtocolRequestToServer.QuickRequestToServer<KillDataProtocol>();
			PacketProtocolRequestToServer.QuickRequestToServer<ModSettingsProtocol>();
		}

		private void OnConnectServer( Player player ) {
			if( player == null ) {
				LogHelpers.Warn( "Player null" );
			}
			if( !player.active ) {
				LogHelpers.Warn( "Player "+player.name+" ("+player.whoAmI+") not active" );
			}
			if( Main.player[ player.whoAmI ] != player ) {
				LogHelpers.Warn( "Player "+player.name+" ("+player.whoAmI+") not found in Main array for some reason..." );
			}
			if( this.player != player ) {
				LogHelpers.Warn( "Player " + player.name + " (" + player.whoAmI + ") does not match our ModPlayer.player" );
			}

			this.HasKillData = true;
			this.HasModSettings = true;
		}
		

		////////////////

		public void FinishLocalKillDataSync() {
			this.HasKillData = true;

			this.AttemptFinishLocalSync();
		}

		public void FinishLocalModSettingsSync() {
			this.HasModSettings = true;

			this.AttemptFinishLocalSync();
		}

		////////////////
		
		private void AttemptFinishLocalSync() {
			if( !this.HasModSettings || !this.HasKillData ) { return; }

			if( this.IsFullySynced ) { return; }
			this.IsFullySynced = true;
			
			if( Main.netMode == 0 ) {
				this.OnFinishPlayerEnterWorldForSingle();
			} else if( Main.netMode == 1 ) {
				this.OnFinishPlayerEnterWorldForClient();
			} else {
				throw new HamstarException( "Servers load player data only after all other data uploaded to server (via. KillDataProtocol)." );
			}
		}


		////////////////

		private void OnFinishPlayerEnterWorldForSingle() {
			this.OnFinishPlayerEnterWorldForHost();
			this.OnFinishPlayerEnterWorldForAny();
		}

		private void OnFinishPlayerEnterWorldForClient() {
			this.OnFinishPlayerEnterWorldForAny();
		}

		internal void OnFinishPlayerEnterWorldForServer() {
			this.OnFinishPlayerEnterWorldForHost();
			this.OnFinishPlayerEnterWorldForAny();
		}

		////

		private void OnFinishPlayerEnterWorldForHost() {
			var mymod = (RewardsMod)this.mod;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			bool success = false;
			
			string playerUid = PlayerIdentityHelpers.GetProperUniqueId( this.player );

			KillData plrData = myworld.Logic.GetPlayerData( this.player );
			if( plrData == null ) {
				LogHelpers.Warn( "Could not get player " + this.player.name + "'s (" + this.player.whoAmI + ") kill data." );
				return;
			}
			
			success = plrData.Load( mymod, playerUid );
			if( !success ) {
				if( KillData.CanReceiveOtherPlayerKillRewards( mymod ) ) {
					plrData.AddToMe( mymod, myworld.Logic.WorldData );
				}
			}

			if( mymod.Config.DebugModeInfo || mymod.Config.DebugModeKillInfo ) {
				LogHelpers.Alert( "who: " + this.player.whoAmI + " success: " + success + ", " + plrData.ToString() );
			}
		}

		private void OnFinishPlayerEnterWorldForAny() {
			Promises.TriggerValidatedPromise( RewardsPlayer.EnterWorldValidator, RewardsPlayer.MyValidatorKey );

			Promises.AddWorldUnloadOncePromise( () => {
				Promises.ClearValidatedPromise( RewardsPlayer.EnterWorldValidator, RewardsPlayer.MyValidatorKey );
			} );
		}


		////////////////

		public void SaveKillData() {
			var mymod = (RewardsMod)this.mod;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			KillData plrData;
			
			string uid = PlayerIdentityHelpers.GetProperUniqueId( this.player );

			lock( WorldLogic.MyLock ) {
				if( !myworld.Logic.PlayerData.ContainsKey( uid ) ) {
					LogHelpers.Warn( "Could not save player kill data; no data found." );
					return;
				}
				plrData = myworld.Logic.PlayerData[uid];
			}

			plrData.Save( mymod, uid );

			if( mymod.Config.DebugModeInfo || mymod.Config.DebugModeKillInfo ) {
				LogHelpers.Alert( "uid: " + uid + ", data: " + plrData.ToString() );
			}
		}
	}
}
