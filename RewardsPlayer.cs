using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using Microsoft.Xna.Framework;
using Rewards.Items;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsPlayer : ModPlayer {
		public bool IsFullySynced { get; private set; }
		public bool HasKillData { get; private set; }

		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void Initialize() {
			this.IsFullySynced = false;
			this.HasKillData = false;
		}

		public override void clientClone( ModPlayer clientClone ) {
			var myclone = (RewardsPlayer)clientClone;
			myclone.IsFullySynced = this.IsFullySynced;
			myclone.HasKillData = this.HasKillData;
		}


		////////////////

		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			var mymod = (RewardsMod)this.mod;

			if( Main.netMode == 2 ) {
				if( toWho == -1 && fromWho == this.player.whoAmI ) {
					this.OnConnectServer( Main.player[fromWho] );
				}
			}
		}


		////////////////

		public override void PreUpdate() {
			if( Main.myPlayer != this.player.whoAmI ) { return; }

			int packType = ModContent.ItemType<ShopPackItem>();

			for( int i = 0; i < this.player.inventory.Length; i++ ) {
				Item item = this.player.inventory[i];
				if( item == null || !item.active ) { continue; }
				if( item.type != packType ) { continue; }

				if( !this.IsFullySynced ) {
					Main.NewText( "Cannot open pack: An error occurred synchronizing with the server.", Color.Red );
					LogHelpers.Alert( "Cannot open pack: An error occurred synchronizing with the server. "
							+"(HasKillData:"+this.HasKillData+")" );

					ItemHelpers.DestroyItem( item );
					this.player.inventory[i] = new Item();
				} else {
					this.OpenPack( item );
				}

				break;
			}

			if( Main.mouseItem.active ) {
				var myitem = Main.mouseItem.modItem as ShopPackItem;
				if( myitem != null && myitem.Info == null ) {
					ItemHelpers.DestroyItem( Main.mouseItem );
				}
			}
		}


		////////////////

		public void OpenPack( Item packItem ) {
			var mymod = (RewardsMod)this.mod;
			var myitem = packItem.modItem as ShopPackItem;
			if( myitem == null) {
				LogHelpers.Warn( "Pack item " + packItem.Name + " missing mod data" );
				ItemHelpers.DestroyItem( packItem );
				return;
			}

			string output;

			if( !myitem.BuyAndOpenPack_Synced( this.player, out output ) ) {
				LogHelpers.Warn( output );
			} else {
				if( mymod.SettingsConfig.DebugModeInfo ) {
					LogHelpers.Alert( output );
				}
			}

			if( myitem.IsClone( Main.mouseItem ) ) {
				ItemHelpers.DestroyItem( Main.mouseItem );
				Main.mouseItem = new Item();
			}
		}
	}
}
