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
					LogHelpers.Alert( "Rewards config could not be loaded." );
					Main.NewText( "Invalid config file. Consider using the /rewardsshopadd command or a JSON editor.", Color.Red );
				}
			}

			this.FinishKillDataSync();
			this.FinishModSettingsSync();
		}

		private void OnConnectCurrentClient() {
			Promises.AddSafeWorldLoadOncePromise( () => {
				PacketProtocol.QuickRequestToServer<KillDataProtocol>();
				PacketProtocol.QuickRequestToServer<ModSettingsProtocol>();
			} );
		}

		private void OnConnectServer( Player player ) {
			this.HasKillData = true;
			this.HasModSettings = true;
		}


		////////////////

		public void FinishKillDataSync() {
			this.HasKillData = true;

			this.FinishSync();
		}

		public void FinishModSettingsSync() {
			this.HasModSettings = true;

			this.FinishSync();
		}

		////////////////
		
		private void FinishSync() {
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
