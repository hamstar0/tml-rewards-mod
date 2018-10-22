using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using Rewards.Items;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsPlayer : ModPlayer {
		public bool IsFullySynced { get; private set; }
		public bool HasKillData { get; private set; }
		public bool HasModSettings { get; private set; }


		////////////////

		public override bool CloneNewInstances { get { return false; } }

		public override void Initialize() {
			this.IsFullySynced = false;
			this.HasKillData = false;
			this.HasModSettings = false;
		}


		////////////////

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			var mymod = (RewardsMod)this.mod;

			if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					this.OnConnectServer( Main.player[from_who] );
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			var mymod = (RewardsMod)this.mod;
			
			if( Main.netMode == 0 ) {
				this.OnConnectSingle();
			}
			if( Main.netMode == 1 ) {
				this.OnConnectCurrentClient();
			}
		}


		////////////////

		public override void PreUpdate() {
			if( Main.myPlayer != this.player.whoAmI ) { return; }
			if( !this.IsFullySynced ) { return; }

			int pack_type = this.mod.ItemType<ShopPackItem>();

			for( int i = 0; i < this.player.inventory.Length; i++ ) {
				Item item = this.player.inventory[i];
				if( item == null || !item.active ) { continue; }
				if( item.type != pack_type ) { continue; }

				this.OpenPack( item );
				break;
			}

			if( Main.mouseItem.active ) {
				var myitem = Main.mouseItem.modItem as ShopPackItem;
				if( myitem != null && myitem.Info == null ) {
					ItemHelpers.DestroyItem( Main.mouseItem );
				}
			}
		}


		public void OpenPack( Item pack_item ) {
			var mymod = (RewardsMod)this.mod;
			var myitem = pack_item.modItem as ShopPackItem;
			if( myitem == null) {
				LogHelpers.Log( "!Rewards.RewardsPlayer.OpenPack - Pack item " + pack_item.Name + " missing mod data" );
				ItemHelpers.DestroyItem( pack_item );
				return;
			}

			string output;

			if( !myitem.BuyAndOpenPack_Synced( this.player, out output ) ) {
				LogHelpers.Log( "!Rewards.RewardsPlayer.OpenPack - " + output );
			} else {
				if( mymod.Config.DebugModeInfo ) {
					LogHelpers.Log( "Rewards.RewardsPlayer.OpenPack - " + output );
				}
			}

			if( myitem.IsClone( Main.mouseItem ) ) {
				ItemHelpers.DestroyItem( Main.mouseItem );
				Main.mouseItem = new Item();
			}
		}
	}
}
