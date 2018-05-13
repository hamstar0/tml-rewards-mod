using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.ItemHelpers;
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
		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			var mymod = (RewardsMod)this.mod;

			if( Main.netMode == 1 ) {
				if( new_player ) {
					this.OnClientConnect();
					return;
				}
			} else {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					this.OnServerConnect();
				}
			}
		}
		
		////////////////

		private void OnClientConnect() { }

		private void OnServerConnect() { }


		////////////////

		public override void OnEnterWorld( Player player ) {
			var mymod = (RewardsMod)this.mod;

			if( player.whoAmI == Main.myPlayer ) {
				if( Main.netMode == 0 ) {   // Single player
					try {
						if( !mymod.SuppressAutoSaving ) {
							if( !mymod.JsonConfig.LoadFile() ) {
								mymod.JsonConfig.SaveFile();
							}
						}
					} catch( Exception ) {
						Main.NewText( "Invalid config file. Consider using the /rewardsshopadd command or a JSON editor.", Color.Red );
					}
				}

				if( Main.netMode == 1 ) {
					PacketProtocol.QuickRequestToServer<KillDataProtocol>();
					PacketProtocol.QuickRequestToServer<ModSettingsProtocol>();
				}
				
				if( Main.netMode == 0 ) {
					this.LoadKillData();
				}
			}
		}


		////////////////
		
		public void LoadKillData() {
			var mymod = (RewardsMod)this.mod;
			var myworld = mymod.GetModWorld<RewardsWorld>();

			bool has_uid;
			string player_uid = PlayerIdentityHelpers.GetUniqueId( this.player, out has_uid );
			if( !has_uid ) { return; }

			KillData plr_data = myworld.Logic.GetPlayerData( this.player );
			if( plr_data == null ) { return; }
			
			bool success = plr_data.Load( mymod, player_uid );
			if( !success ) {
				if( KillData.CanReceiveOtherPlayerKillRewards( mymod ) ) {
					plr_data.AddToMe( mymod, myworld.Logic.WorldData );
				}
			}

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "RewardsPlayer.LoadKillData - who: "+this.player.whoAmI+" success: " + success + ", " + plr_data.ToString() );
			}
		}


		public void SaveKillData() {
			var mymod = (RewardsMod)this.mod;
			var myworld = mymod.GetModWorld<RewardsWorld>();

			bool has_uid;
			string uid = PlayerIdentityHelpers.GetUniqueId( player, out has_uid );
			if( !has_uid ) { return; }

			if( !myworld.Logic.PlayerData.ContainsKey( uid ) ) { return; }
			KillData plr_data = myworld.Logic.PlayerData[uid];

			plr_data.Save( mymod, uid );

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "RewardsPlayer.SaveKillData - pid: " + has_uid + ", data: " + plr_data.ToString() );
			}
		}


		////////////////

		public void FinishModSettingsSync() { }


		////////////////

		public override void PreUpdate() {
			if( Main.myPlayer != this.player.whoAmI ) { return; }

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
