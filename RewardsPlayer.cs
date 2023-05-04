using Microsoft.Xna.Framework;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Items;
using Rewards.Items;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsPlayer : ModPlayer {
		public bool IsFullySynced { get; private set; }

		////////////////

		protected override bool CloneNewInstances => false;



		////////////////

		public override void Initialize() {
			this.IsFullySynced = false;
		}

		public override void clientClone( ModPlayer clientClone ) {
			var myclone = (RewardsPlayer)clientClone;
			myclone.IsFullySynced = this.IsFullySynced;
		}

		// Don't remove.
		public override void SendClientChanges( ModPlayer clientPlayer ) {
			
		}


		////////////////

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) {
				return;
			}

			var myplayer = player.GetModPlayer<RewardsPlayer>();

			if( Main.netMode == 0 ) {
				myplayer.OnConnectSingle();
			}
			if( Main.netMode == 1 ) {
				myplayer.OnConnectCurrentClient();
			}

			/*
			InboxMessages.SetMessage( "RewardsModConfigUpdate",
				"Rewards config files updated to use ModConfig (tML v0.11+). The old config files "+
				"(Rewards Config.json, Rewards Points Config.json, Rewards Shop Config.json) are now obsolete. "+
				"If any mod settings have been changed from their defaults in the past, you'll need to import them "+
				"manually (preferably via. the menu's Mod Configuration).",
				false
			);
			*/
		}

		////////////////

		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			var mymod = (RewardsMod)this.Mod;

			if( Main.netMode == 2 ) {
				if( toWho == -1 && fromWho == this.Player.whoAmI ) {
					this.OnConnectServer( Main.player[fromWho] );
				}
			}
		}


		////////////////

		public override void PreUpdate() {
			if( Main.myPlayer != this.Player.whoAmI ) { return; }

			int packType = ModContent.ItemType<ShopPackItem>();

			for( int i = 0; i < this.Player.inventory.Length; i++ ) {
				Item item = this.Player.inventory[i];
				if( item == null || !item.active ) { continue; }
				if( item.type != packType ) { continue; }

				if( !this.IsFullySynced ) {
					Main.NewText( "Cannot open pack: An error occurred synchronizing with the server.", Color.Red );
					LogLibraries.Alert( "Cannot open pack: An error occurred synchronizing with the server." );

					ItemLibraries.DestroyItem( item );
					this.Player.inventory[i] = new Item();
				} else {
					this.OpenPack( item );
				}

				break;
			}

			if( Main.mouseItem.active ) {
				var myitem = Main.mouseItem.ModItem as ShopPackItem;
				if( myitem != null && myitem.Info == null ) {
					ItemLibraries.DestroyItem( Main.mouseItem );
				}
			}
		}


		////////////////

		public void OpenPack( Item packItem ) {
			var mymod = (RewardsMod)this.Mod;
			var myitem = packItem.ModItem as ShopPackItem;
			if( myitem == null) {
				LogLibraries.Warn( "Pack item " + packItem.Name + " missing mod data" );
				ItemLibraries.DestroyItem( packItem );
				return;
			}

			string output;
			
			if( !myitem.BuyAndOpenPack_Synced( this.Player, out output ) ) {
				LogLibraries.Warn( output );
			} else {
				if( mymod.SettingsConfig.DebugModeInfo ) {
					LogLibraries.Alert( output );
				}
			}

			if( myitem.IsClone( Main.mouseItem ) ) {
				ItemLibraries.DestroyItem( Main.mouseItem );
				Main.mouseItem = new Item();
			}
		}
	}
}
