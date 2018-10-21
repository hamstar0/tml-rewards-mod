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

			if( !mymod.SuppressConfigAutoSaving ) {
				if( !mymod.ConfigJson.LoadFile() ) {
					//mymod.ConfigJson.SaveFile();
					LogHelpers.Log( "RewardsPlayer.OnPlayerEnterWorldForSingle - Rewards config could not be loaded." );
					Main.NewText( "Invalid config file. Consider using the /rewardsshopadd command or a JSON editor.", Color.Red );
				}

				this.FinishKillDataSync();
				this.FinishModSettingsSync();
			}
		}

		private void OnConnectCurrentClient() {
			PacketProtocol.QuickRequestToServer<KillDataProtocol>();
			PacketProtocol.QuickRequestToServer<ModSettingsProtocol>();
		}

		private void OnConnectServer( Player player ) {
			this.IsFullySynced = true;
			this.HasKillData = true;
			this.HasModSettings = true;
		}


		////////////////

		public void FinishKillDataSync() {
			this.HasKillData = true;

			this.CheckSync();
		}

		public void FinishModSettingsSync() {
			this.HasModSettings = true;

			this.CheckSync();
		}

		////////////////

		public bool HasEachSyncingOccurred() {
			return this.HasModSettings && this.HasKillData;
		}

		private void CheckSync() {
			if( !this.IsFullySynced && this.HasEachSyncingOccurred() ) {
				this.IsFullySynced = true;

				if( Main.netMode == 0 ) {
					this.OnFinishPlayerEnterWorldForSingle();
				} else if( Main.netMode == 1 ) {
					this.OnFinishPlayerEnterWorldForClient();
				} else {
					throw new HamstarException( "Servers load player data only after all other data uploaded to server (via. KillDataProtocol)." );
				}
			}
		}


		////////////////

		public void OnFinishPlayerEnterWorldForSingle() {
			this.OnFinishPlayerEnterWorldForHost();
			this.OnFinishPlayerEnterWorldForAny();
		}

		public void OnFinishPlayerEnterWorldForClient() {
			this.OnFinishPlayerEnterWorldForAny();
		}

		public void OnFinishPlayerEnterWorldForServer() {
			this.OnFinishPlayerEnterWorldForHost();
			this.OnFinishPlayerEnterWorldForAny();
		}


		private void OnFinishPlayerEnterWorldForHost() {
			var mymod = (RewardsMod)this.mod;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			bool success = false;
			
			string player_uid = PlayerIdentityHelpers.GetProperUniqueId( this.player );

			KillData plr_data = myworld.Logic.GetPlayerData( this.player );
			if( plr_data == null ) {
				LogHelpers.Log( "!Rewards.RewardsPlayer.OnFinishEnterWorldForHost - Could not get player " + this.player.name + "'s (" + this.player.whoAmI + ") kill data." );
				return;
			}
			
			success = plr_data.Load( mymod, player_uid );
			if( !success ) {
				if( KillData.CanReceiveOtherPlayerKillRewards( mymod ) ) {
					plr_data.AddToMe( mymod, myworld.Logic.WorldData );
				}
			}

			if( mymod.Config.DebugModeInfo || mymod.Config.DebugModeKillInfo ) {
				LogHelpers.Log( "Rewards.RewardsPlayer.OnFinishEnterWorldForHost - who: " + this.player.whoAmI + " success: " + success + ", " + plr_data.ToString() );
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
			KillData plr_data;
			
			string uid = PlayerIdentityHelpers.GetProperUniqueId( player );

			lock( WorldLogic.MyLock ) {
				if( !myworld.Logic.PlayerData.ContainsKey( uid ) ) {
					LogHelpers.Log( "!Rewards.RewardsPlayer.SaveKillData - Could not save player kill data; no data found." );
					return;
				}
				plr_data = myworld.Logic.PlayerData[uid];
			}

			plr_data.Save( mymod, uid );

			if( mymod.Config.DebugModeInfo || mymod.Config.DebugModeKillInfo ) {
				LogHelpers.Log( "Rewards.RewardsPlayer.SaveKillData - uid: " + uid + ", data: " + plr_data.ToString() );
			}
		}
	}
}
