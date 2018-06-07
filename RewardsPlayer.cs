using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.ItemHelpers;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Network;
using Microsoft.Xna.Framework;
using Rewards.Items;
using Rewards.Logic;
using Rewards.NetProtocols;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	class RewardsPlayer : ModPlayer {
		public bool HasEnteredWorld { get; private set; }
		public bool HasKillData { get; private set; }
		public bool HasModSettings { get; private set; }


		////////////////
		
		public override bool CloneNewInstances { get { return false; } }

		public override void Initialize() {
			this.HasEnteredWorld = false;
			this.HasKillData = false;
			this.HasModSettings = false;
		}


		////////////////

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			var mymod = (RewardsMod)this.mod;

			if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					this.OnServerConnect();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			
			var mymod = (RewardsMod)this.mod;
			
			if( Main.netMode == 0 ) {   // Single player
				try {
					if( !mymod.SuppressAutoSaving ) {
						if( !mymod.ConfigJson.LoadFile() ) {
							mymod.ConfigJson.SaveFile();
						}
					}
				} catch( Exception ) {
					Main.NewText( "Invalid config file. Consider using the /rewardsshopadd command or a JSON editor.", Color.Red );
				}

				this.OnSingleConnect();
			}

			if( Main.netMode == 1 ) {
				this.OnClientConnect();
			}
		}

		////////////////

		private void OnSingleConnect() {
			this.FinishKillDataSync();
			this.FinishModSettingsSync();
		}

		private void OnClientConnect() {
			PacketProtocol.QuickRequestToServer<KillDataProtocol>();
			PacketProtocol.QuickRequestToServer<ModSettingsProtocol>();
		}

		private void OnServerConnect() {
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

			TmlLoadHelpers.TriggerCustomPromise( "RewardsOnEnterWorld" );
			TmlLoadHelpers.AddWorldUnloadOncePromise( () => {
				TmlLoadHelpers.ClearCustomPromise( "RewardsOnEnterWorld" );
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


		////////////////

		public override void PreUpdate() {
			if( Main.myPlayer != this.player.whoAmI ) { return; }
			if( !this.IsSynced() ) { return; }

			int pack_type = this.mod.ItemType<ShopPackItem>();

			for( int i = 0; i < this.player.inventory.Length; i++ ) {
				Item item = this.player.inventory[i];
				if( item == null || !item.active ) { continue; }
				if( item.type != pack_type ) { continue; }

				this.CheckPack( (ShopPackItem)item.modItem );
				break;
			}

			if( Main.mouseItem.active ) {
				var myitem = Main.mouseItem.modItem as ShopPackItem;
				if( myitem != null && myitem.Info == null ) {
					ItemHelpers.DestroyItem( Main.mouseItem );
				}
			}
		}


		public void CheckPack( ShopPackItem myitem ) {
			myitem.OpenPack( (RewardsMod)this.mod, this.player );

			if( myitem.IsClone( Main.mouseItem ) ) {
				ItemHelpers.DestroyItem( Main.mouseItem );
				Main.mouseItem = new Item();
			}
		}
	}
}
