﻿using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Rewards.Items {
	public partial class ShopPackItem : ModItem {
		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			if( this.Info == null ) { return; }

			var info = (ShopPackDefinition)this.Info;
			var itemSetTip = new TooltipLine( this.mod, "Items", "Items included:" );

			try {
				tooltips.RemoveRange( 1, tooltips.Count - 1 );
				tooltips.Add( itemSetTip );

				int count = info.Items.Count;
				for( int i = 0; i < count; i++ ) {
					ShopPackItemDefinition packItemDef = info.Items[i];
					if( packItemDef == null ) {
						LogHelpers.WarnOnce( "Invalid pack item for " + this.DisplayName + " at " + i );
						continue;
					}

					ItemDefinition itemDef = packItemDef.ItemDef;
					if( itemDef == null || itemDef.Type == 0 ) {
						LogHelpers.WarnOnce( "Undefined pack item for " + this.DisplayName + " at " + i );
						continue;
					}

					Item item = new Item();
					item.SetDefaults( itemDef.Type, true );
					item.stack = packItemDef.Stack;

					var itemTip = new TooltipLine( this.mod, "Item " + i, "  " + item.HoverName );

					Color rarityColor;
					if( ItemRarityAttributeHelpers.RarityColor.TryGetValue( item.rare, out rarityColor ) ) {
						itemTip.overrideColor = rarityColor;
					}

					tooltips.Add( itemTip );
				}
			} catch( Exception e ) {
				LogHelpers.WarnOnce( "!!1 " + e.ToString() );
			}
			
			try {
				Color tipColor = info.Price <= 10 ?
					Colors.CoinCopper :
					( info.Price < 100 ?
						Colors.CoinSilver :
						( info.Price < 1000 ?
							Colors.CoinGold :
							Colors.CoinPlatinum ) );
				var ppTip = new TooltipLine( this.mod, "Custom Price", "Buy price: " + info.Price + " progress points" ) {
					overrideColor = tipColor
				};
				tooltips.Add( ppTip );

				var instructTip = new TooltipLine( this.mod, "Items", "Click to purchase" );
				tooltips.Add( instructTip );
			} catch( Exception e ) {
				LogHelpers.WarnOnce( "!!2 " + e.ToString() );
			}
		}


		////////////////
		
		public override void PostDrawInInventory( SpriteBatch sb, Vector2 pos, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale ) {
			int bagItemType = this.GetItemTypeOfIcon();
			if( bagItemType == -1 ) { return; }
			
			Texture2D tex = Main.itemTexture[bagItemType];
			float subScale = scale;

			if( tex.Width >= tex.Height ) {
				if( tex.Width > 20 ) {
					subScale /= (float)tex.Width / 20f;
				}
			} else {
				if( tex.Height > 20 ) {
					subScale /= (float)tex.Height / 20f;
				}
			}

			var rect = new Rectangle( 0, 0, tex.Width, tex.Height );
			pos.X += (float)frame.Width * 0.5f * scale;
			pos.Y += (float)( frame.Height + 6 ) * 0.5f * scale;
			origin = new Vector2( (float)tex.Width, (float)tex.Height ) * 0.5f;

			sb.Draw( tex, pos, rect, drawColor, 0f, origin, subScale, SpriteEffects.None, 1f );
		}


		public override void PostDrawInWorld( SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI ) {
			int bagItemType = this.GetItemTypeOfIcon();
			if( bagItemType == -1 ) { return; }
			
			Texture2D tex = Main.itemTexture[ bagItemType ];
			float subScale = scale;

			if( tex.Width >= tex.Height ) {
				if( tex.Width > 20 ) {
					subScale /= (float)tex.Width / 20f;
				}
			} else {
				if( tex.Height > 20 ) {
					subScale /= (float)tex.Height / 20f;
				}
			}

			var rect = new Rectangle( 0, 0, tex.Width, tex.Height );
			var pos = new Vector2();
			pos.X += (float)item.width * 0.5f * scale;
			pos.Y += (float)(item.height + 6) * 0.5f * scale;
			var origin = new Vector2( (float)tex.Width, (float)tex.Height ) * 0.5f;

			sb.Draw( tex, pos, rect, lightColor, 0f, origin, subScale, SpriteEffects.None, 1f );
		}


		////////////////

		public int GetItemTypeOfIcon() {
			if( this.Info == null ) { return -1; }

			try {
				var info = (ShopPackDefinition)this.Info;
				string _;
				if( !info.Validate( out _ ) ) { return -1; }

				int count = info.Items.Count;
				for( int i = 0; i < count; i++ ) {
					ShopPackItemDefinition itemInfo = info.Items[i];
					int bagItemType = itemInfo.ItemDef?.Type ?? 0;

					if( bagItemType <= 0 ) { continue; }
					if( bagItemType >= Main.itemTexture.Length ) { continue; }
					if( Main.itemTexture[bagItemType] == null ) { continue; }

					return bagItemType;
				}
			} catch( Exception e ) {
				LogHelpers.WarnOnce( "!!3 " + e.ToString() );
			}

			return -1;
		}
	}
}
