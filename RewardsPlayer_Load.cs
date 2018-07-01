using HamstarHelpers.Components.Network;
using HamstarHelpers.DebugHelpers;
using HamstarHelpers.PlayerHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using Rewards.Logic;
using Rewards.NetProtocols;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsPlayer : ModPlayer {
		private void OnEnterWorldForSingle() {
			var mymod = (RewardsMod)this.mod;

			if( !mymod.SuppressConfigAutoSaving ) {
				try {
					if( !mymod.ConfigJson.LoadFile() ) {
						mymod.ConfigJson.SaveFile();
						LogHelpers.Log( "Rewards config " + RewardsConfigData.ConfigVersion.ToString() + " created (ModPlayer.OnEnterWorld())." );
					}
				} catch( Exception ) {
					Main.NewText( "Invalid config file. Consider using the /rewardsshopadd command or a JSON editor.", Color.Red );
				}
			}

			this.FinishKillDataSync();
			this.FinishModSettingsSync();
		}

		private void OnEnterWorldForClient() {
			PacketProtocol.QuickRequestToServer<KillDataProtocol>();
			PacketProtocol.QuickRequestToServer<ModSettingsProtocol>();
		}

		private void OnEnterWorldForServer() {
			this.HasEnteredWorld = true;
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

		public bool IsSynced() {
			return this.HasModSettings && this.HasKillData;
		}

		private void CheckSync() {
			if( !this.HasEnteredWorld && this.IsSynced() ) {
				this.HasEnteredWorld = true;

				this.OnFinishEnterWorld();
			}
		}


		////////////////

		public void OnFinishEnterWorld() {
			var mymod = (RewardsMod)this.mod;
			var myworld = mymod.GetModWorld<RewardsWorld>();

			bool has_uid;
			string player_uid = PlayerIdentityHelpers.GetUniqueId( this.player, out has_uid );
			if( !has_uid ) {
				LogHelpers.Log( "Rewards - RewardsPlayer.OnFinishEnterWorld - Could not enter world for player; no player id." );
				return;
			}

			KillData plr_data = myworld.Logic.GetPlayerData( this.player );
			if( plr_data == null ) { return; }
			
			bool success = plr_data.Load( mymod, player_uid );
			if( !success ) {
				if( KillData.CanReceiveOtherPlayerKillRewards( mymod ) ) {
					plr_data.AddToMe( mymod, myworld.Logic.WorldData );
				}
			}

			Promises.TriggerCustomPromise( "RewardsOnEnterWorld" );
			Promises.AddWorldUnloadOncePromise( () => {
				Promises.ClearCustomPromise( "RewardsOnEnterWorld" );
			} );

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "Rewards - RewardsPlayer.LoadKillData - who: "+this.player.whoAmI+" success: " + success + ", " + plr_data.ToString() );
			}
		}


		public void SaveKillData() {
			var mymod = (RewardsMod)this.mod;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			KillData plr_data;

			bool has_uid;
			string uid = PlayerIdentityHelpers.GetUniqueId( player, out has_uid );
			if( !has_uid ) {
				LogHelpers.Log( "Rewards - RewardsPlayer.SaveKillData - Could not save player kill data; no player id." );
				return;
			}

			lock( WorldLogic.MyLock ) {
				if( !myworld.Logic.PlayerData.ContainsKey( uid ) ) {
					LogHelpers.Log( "Rewards - RewardsPlayer.SaveKillData - Could not save player kill data; no data found." );
					return;
				}
				plr_data = myworld.Logic.PlayerData[uid];
			}

			plr_data.Save( mymod, uid );

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "Rewards - RewardsPlayer.SaveKillData - uid: " + uid + ", data: " + plr_data.ToString() );
			}
		}
	}
}
