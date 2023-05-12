using System;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.Players;
using ModLibsCore.Services.Network.SimplePacket;
using ModLibsGeneral.Libraries.Players;
using Rewards.Logic;
using Rewards.NetProtocols;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsPlayer : ModPlayer {
		/*
		internal readonly static object MyValidatorKey;
		public readonly static CustomLoadHookValidator<object> EnterWorldValidator;
		*/


		////////////////

		static RewardsPlayer() {
			/*
			RewardsPlayer.MyValidatorKey = new object();
			RewardsPlayer.EnterWorldValidator = new CustomLoadHookValidator<object>( RewardsPlayer.MyValidatorKey );
			*/
		}



		////////////////

		internal void OnConnectSingle() {
			this.IsFullySynced = false;

			this.FinishLocalSync();
		}

		internal void OnConnectCurrentClient() {
			if( this.Player == null ) {
				LogLibraries.Warn( "Player null" );
			}
			if( !this.Player.active ) {
				LogLibraries.Warn( "Player " + this.Player.name + " (" + this.Player.whoAmI + ") not active" );
			}
			if( Main.player[this.Player.whoAmI] != this.Player ) {
				LogLibraries.Warn( "Player " + this.Player.name + " (" + this.Player.whoAmI + ") not found in Main array for some reason..." );
			}

			this.IsFullySynced = false;

			if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
				LogLibraries.Alert( "Requesting kill data, kill point amounts, shop loadout, and mod settings from server..." );
			}

			SimplePacket.SendToServer( new KillDataRequestPacket() );
		}

		private void OnConnectServer( Player player ) {
			if( player == null ) {
				LogLibraries.Warn( "Player null" );
			}
			if( !player.active ) {
				LogLibraries.Alert( "Player "+player.name+" ("+player.whoAmI+") not active" );
			}
			if( Main.player[ player.whoAmI ] != player ) {
				LogLibraries.Warn( "Player "+player.name+" ("+player.whoAmI+") not found in Main array for some reason..." );
			}
			if( this.Player != player ) {
				LogLibraries.Warn( "Player " + player.name + " (" + player.whoAmI + ") does not match our ModPlayer.player" );
			}

			this.IsFullySynced = true;
		}
		

		////////////////

		internal void FinishLocalSync() {
			if( this.IsFullySynced ) { return; }
			this.IsFullySynced = true;

			if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
				LogLibraries.Alert();
			}

			if( Main.netMode == 0 ) {
				this.OnFinishPlayerEnterWorldForSingle();
			} else if( Main.netMode == 1 ) {
				this.OnFinishPlayerEnterWorldForClient();
			} else {
				throw new InvalidOperationException( "Servers load player data only after all other data uploaded to server (via. KillDataProtocol)." );
			}
		}


		////////////////

		private void OnFinishPlayerEnterWorldForSingle() {
			bool _;
			this.OnFinishPlayerEnterWorldForHost( out _ );
			this.OnFinishPlayerEnterWorldForAny();
		}

		private void OnFinishPlayerEnterWorldForClient() {
			this.OnFinishPlayerEnterWorldForAny();
		}

		internal void OnFinishPlayerEnterWorldForServer( out bool isSynced ) {
			this.OnFinishPlayerEnterWorldForHost( out isSynced );
			this.OnFinishPlayerEnterWorldForAny();
		}

		////

		private void OnFinishPlayerEnterWorldForHost( out bool isSynced ) {
			var mymod = (RewardsMod)this.Mod;
			var myworld = ModContent.GetInstance<RewardsSystem>();
			bool success = false;
			
			string playerUid = PlayerIdentityLibraries.GetUniqueId( this.Player );

			KillData plrData = myworld.Logic.GetPlayerData( this.Player );
			if( plrData == null ) {
				LogLibraries.Warn( "Could not get player " + this.Player.name + "'s (" + this.Player.whoAmI + ") kill data." );
				isSynced = false;
				return;
			}
			
			success = plrData.Load( playerUid, this.Player );
			if( !success ) {
				if( KillData.CanReceiveOtherPlayerKillRewards( ) ) {
					plrData.AddToMe( myworld.Logic.WorldData );
				}
			}

			if( mymod.SettingsConfig.DebugModeInfo || mymod.SettingsConfig.DebugModeKillInfo ) {
				LogLibraries.Alert( "who: " + this.Player.whoAmI + " success: " + success + ", " + plrData.ToString() );
			}

			isSynced = success;
		}

		private void OnFinishPlayerEnterWorldForAny() {
			/*
			CustomLoadHooks.TriggerHook( RewardsPlayer.EnterWorldValidator, RewardsPlayer.MyValidatorKey );

			LoadHooks.AddWorldUnloadOnceHook( () => {
				CustomLoadHooks.ClearHook( RewardsPlayer.EnterWorldValidator, RewardsPlayer.MyValidatorKey );
			} );
			*/
		}


		////////////////

		public void SaveKillData() {
			var mymod = (RewardsMod)this.Mod;
			var myworld = ModContent.GetInstance<RewardsSystem>();
			string uid = PlayerIdentityLibraries.GetUniqueId( this.Player );

			KillData plrData = myworld.Logic.GetPlayerData( this.Player );
			if( plrData == null ) {
				LogLibraries.Warn( "Could not save player kill data; no data found." );
				return;
			}

			plrData.Save( uid );

			if( mymod.SettingsConfig.DebugModeInfo ) {
				LogLibraries.Alert( "uid: " + uid + ", data: " + plrData.ToString() );
			}
		}
	}
}
