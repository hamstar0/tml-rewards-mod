using Microsoft.Xna.Framework;
using Rewards.Items;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.NPCs {
	partial class WayfarerTownNPC : ModNPC {
		public override void SetChatButtons( ref string button1, ref string button2 ) {
			var mymod = (RewardsMod)this.mod;
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
					WayfarerTownNPC.CurrentShop = ( WayfarerTownNPC.CurrentShop + 1) >= shops ? 0 : ( WayfarerTownNPC.CurrentShop + 1);
				}
			}
		}


		////////////////

		public override void SetupShop( Chest shop, ref int nextSlot ) {
			var mymod = (RewardsMod)this.mod;
			int shopStart = WayfarerTownNPC.CurrentShop * 40;
			
			for( int i = shopStart; i < mymod.ShopConfig.ShopLoadout.Count; i++ ) {
				if( nextSlot >= 40 ) {
					break;
				}

				ShopPackDefinition def = mymod.ShopConfig.ShopLoadout[i];
				string fail;

				if( !def.Validate(out fail) ) {
					Main.NewText( "Could not load shop item " + def.Name + " ("+fail+")", Color.Red );
					continue;
				}
				if( !def.RequirementsMet() ) {
					continue;
				}
				
				shop.item[ nextSlot++ ] = ShopPackItem.CreateItem( def );
			}
		}


		////////////////

		public int CountShopItems() {
			var mymod = (RewardsMod)this.mod;
			int count = 0;

			string _;
			foreach( ShopPackDefinition def in mymod.ShopConfig.ShopLoadout ) {
				if( def.Validate( out _ ) && def.RequirementsMet() ) {
					count++;
				}
			}

			return count;
		}
	}
}
