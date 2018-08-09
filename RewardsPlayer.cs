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
					this.OnPlayerEnterWorldForServer();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			var mymod = (RewardsMod)this.mod;
			
			if( Main.netMode == 0 ) {
				this.OnPlayerEnterWorldForSingle();
			}
			if( Main.netMode == 1 ) {
				this.OnPlayerEnterWorldForClient();
			}
		}


		////////////////

		public override void PreUpdate() {
			if( Main.myPlayer != this.player.whoAmI ) { return; }
			if( !this.HasEachSyncingOccurred() ) { return; }

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
			myitem.BuyAndOpenPack_Synced( this.player );

			if( myitem.IsClone( Main.mouseItem ) ) {
				ItemHelpers.DestroyItem( Main.mouseItem );
				Main.mouseItem = new Item();
			}
		}
	}
}
