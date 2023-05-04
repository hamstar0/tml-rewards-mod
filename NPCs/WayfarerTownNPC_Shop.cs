using Rewards.Items;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.NPCs {
	partial class WayfarerTownNPC : ModNPC {
		public override void SetChatButtons( ref string button1, ref string button2 ) {
			var mymod = (RewardsMod)this.Mod;
			int itemCount = this.CountShopItems();
			bool hasButton2 = false;

			button1 = "Shop";

			if( mymod.ShopConfig.ShopLoadout.Count > 40 ) {
				hasButton2 = this.CountShopItems() > 40;
			}

			if( hasButton2 ) {
				int shops = (int)Math.Ceiling( (float)itemCount / 40f );
				int nextShop = (WayfarerTownNPC.CurrentShop + 1) >= shops ? 0 : ( WayfarerTownNPC.CurrentShop + 1);

				button2 = "Scroll to shop "+(nextShop+1)+" of "+shops;
			}
		}

		public override void OnChatButtonClicked( bool firstButton, ref bool shop ) {
			if( firstButton ) {
				shop = firstButton;
			} else {
				int itemCount = this.CountShopItems();
				int shops = (int)Math.Ceiling( (float)itemCount / 40f );

				if( shops >= 1 ) {
					WayfarerTownNPC.CurrentShop = ( WayfarerTownNPC.CurrentShop + 1) >= shops ?
						0 :
						( WayfarerTownNPC.CurrentShop + 1);
				}
			}
		}


		////////////////

		public override void SetupShop( Chest shop, ref int nextSlot ) {
			var mymod = (RewardsMod)this.Mod;
			int shopStart = WayfarerTownNPC.CurrentShop * 40;
			ShopPackDefinition[] defs = ShopPackDefinition.GetValidatedLoadout( true );
			
			for( int i = shopStart; i < defs.Length; i++ ) {
				if( nextSlot >= 40 ) {
					break;
				}
				
				shop.item[ nextSlot++ ] = ShopPackItem.CreateItem( defs[i] );
			}
		}


		////////////////

		public int CountShopItems() {
			var mymod = (RewardsMod)this.Mod;
			ShopPackDefinition[] defs = ShopPackDefinition.GetValidatedLoadout( false );
			return defs.Length;
		}
	}
}
