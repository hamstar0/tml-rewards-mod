using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Services.Hooks.LoadHooks;
using Rewards.Logic;
using Rewards.NetProtocols;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsPlayer : ModPlayer {
		internal readonly static object MyValidatorKey;
		public readonly static CustomLoadHookValidator<object> EnterWorldValidator;


		////////////////

		static RewardsPlayer() {
			RewardsPlayer.MyValidatorKey = new object();
			RewardsPlayer.EnterWorldValidator = new CustomLoadHookValidator<object>( RewardsPlayer.MyValidatorKey );
		}



		////////////////

		internal void OnConnectSingle() {
			var mymod = (RewardsMod)this.mod;

			this.IsFullySynced = false;
			this.HasKillData = false;

			this.FinishLocalKillDataSync();
		}

		internal void OnConnectCurrentClient() {
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

			if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
				LogHelpers.Alert( "Requesting kill data, kill point amounts, shop loadout, and mod settings from server..." );
			}

			KillDataProtocol.QuickRequest();
		}

		private void OnConnectServer( Player player ) {
			if( player == null ) {
				LogHelpers.Warn( "Player null" );
			}
			if( !player.active ) {
				LogHelpers.Alert( "Player "+player.name+" ("+player.whoAmI+") not active" );
			}
			if( Main.player[ player.whoAmI ] != player ) {
				LogHelpers.Warn( "Player "+player.name+" ("+player.whoAmI+") not found in Main array for some reason..." );
			}
			if( this.player != player ) {
				LogHelpers.Warn( "Player " + player.name + " (" + player.whoAmI + ") does not match our ModPlayer.player" );
			}

			this.HasKillData = true;
		}
		

		////////////////

		public void FinishLocalKillDataSync() {
			this.HasKillData = true;
			if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
				LogHelpers.Alert();
			}
			this.AttemptFinishLocalSync();
		}

		////////////////

		private void AttemptFinishLocalSync() {
			if( !this.HasKillData ) { return; }

			if( this.IsFullySynced ) { return; }
			this.IsFullySynced = true;

			if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
				LogHelpers.Alert();
			}

			if( Main.netMode == 0 ) {
				this.OnFinishPlayerEnterWorldForSingle();
			} else if( Main.netMode == 1 ) {
				this.OnFinishPlayerEnterWorldForClient();
			} else {
				throw new ModHelpersException( "Servers load player data only after all other data uploaded to server (via. KillDataProtocol)." );
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
			var mymod = (RewardsMod)this.mod;
			var myworld = ModContent.GetInstance<RewardsWorld>();
			bool success = false;
			
			string playerUid = PlayerIdentityHelpers.GetUniqueId( this.player );

			KillData plrData = myworld.Logic.GetPlayerData( this.player );
			if( plrData == null ) {
				LogHelpers.Warn( "Could not get player " + this.player.name + "'s (" + this.player.whoAmI + ") kill data." );
				isSynced = false;
				return;
			}
			
			success = plrData.Load( playerUid, this.player );
			if( !success ) {
				if( KillData.CanReceiveOtherPlayerKillRewards( ) ) {
					plrData.AddToMe( myworld.Logic.WorldData );
				}
			}

			if( mymod.SettingsConfig.DebugModeInfo || mymod.SettingsConfig.DebugModeKillInfo ) {
				LogHelpers.Alert( "who: " + this.player.whoAmI + " success: " + success + ", " + plrData.ToString() );
			}

			isSynced = success;
		}

		private void OnFinishPlayerEnterWorldForAny() {
			CustomLoadHooks.TriggerHook( RewardsPlayer.EnterWorldValidator, RewardsPlayer.MyValidatorKey );

			LoadHooks.AddWorldUnloadOnceHook( () => {
				CustomLoadHooks.ClearHook( RewardsPlayer.EnterWorldValidator, RewardsPlayer.MyValidatorKey );
			} );
		}


		////////////////

		public void SaveKillData() {
			var mymod = (RewardsMod)this.mod;
			var myworld = ModContent.GetInstance<RewardsWorld>();
			string uid = PlayerIdentityHelpers.GetUniqueId( this.player );

			KillData plrData = myworld.Logic.GetPlayerData( this.player );
			if( plrData == null ) {
				LogHelpers.Warn( "Could not save player kill data; no data found." );
				return;
			}

			plrData.Save( uid );

			if( mymod.SettingsConfig.DebugModeInfo ) {
				LogHelpers.Alert( "uid: " + uid + ", data: " + plrData.ToString() );
			}
		}
	}
}
