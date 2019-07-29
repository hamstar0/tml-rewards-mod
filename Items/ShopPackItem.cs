using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Items.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rewards.Logic;
using Rewards.NetProtocols;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Rewards.Items {
	class ShopPackItem : ModItem {
		public static Item CreateItem( ShopPackDefinition packInfo ) {
			Item item = new Item();
			item.SetDefaults( RewardsMod.Instance.ItemType<ShopPackItem>() );
			item.SetNameOverride( packInfo.Name );

			var myitem = (ShopPackItem)item.modItem;
			myitem.Info = packInfo;

			return item;
		}



		////////////////

		public override bool CloneNewInstances => true;

		internal ShopPackDefinition? Info = null;


		////////////////

		public bool IsClone( Item item ) {
			if( !(item.modItem is ShopPackItem) ) { return false; }

			var otherModItem = (ShopPackItem)item.modItem;
			var def = (ShopPackDefinition)otherModItem.Info;

			return def.IsSameAs( (ShopPackDefinition)this.Info );
		}
		
		public override ModItem Clone() {
			var clone = (ShopPackItem)base.Clone();
			clone.Info = this.Info;
			return clone;
		}

		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Wayfarer's Pack" );
		}

		public override void SetDefaults() {
			this.item.maxStack = 1;
			this.item.width = 22;
			this.item.height = 22;
			this.item.value = 0;
			this.item.rare = ItemRarityAttributeHelpers.QuestItemRarity;
		}


		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			if( this.Info == null ) { return; }

			tooltips.RemoveRange( 1, tooltips.Count - 1 );

			var info = (ShopPackDefinition)this.Info;
			var itemSetTip = new TooltipLine( this.mod, "Items", "Items included:" );
			tooltips.Add( itemSetTip );

			for( int i=0; i< info.Items.Length; i++ ) {
				Color rareColor = Color.Gray;
				Item item = new Item();
				item.SetDefaults( info.Items[i].ItemType );

				var itemTip = new TooltipLine( this.mod, "Item "+i, "  "+ info.Items[i].Stack + " " + info.Items[i].Name );

				if( ItemRarityAttributeHelpers.RarityColor.TryGetValue( item.rare, out rareColor ) ) {
					itemTip.overrideColor = rareColor;
				}

				tooltips.Add( itemTip );
			}

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

			var info = (ShopPackDefinition)this.Info;
			string _;
			if( !info.Validate(out _) ) { return -1; }

			for( int i = 0; i < info.Items.Length; i++ ) {
				ShopPackItemDefinition itemInfo = info.Items[i];
				int bagItemType = itemInfo.ItemType;

				if( bagItemType <= 0 ) { continue; }
				if( bagItemType >= Main.itemTexture.Length ) { continue; }
				if( Main.itemTexture[ bagItemType ] == null ) { continue; }

				return bagItemType;
			}
			return -1;
		}


		////////////////

		public bool BuyAndOpenPack_Synced( Player player, out string output ) {
			var mymod = RewardsMod.Instance;
			ItemHelpers.DestroyItem( this.item );

			if( this.Info == null ) {
				output = "No pack info available.";
				return false;
			}

			var info = (ShopPackDefinition)this.Info;
			int price = info.Price;

			var myworld = mymod.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( player );
			if( data == null ) {
				output = "No player data for " + player.name;
				return false;
			}
			
			if( !data.Spend( price, player ) ) {
				Main.NewText( "Not enough progress points.", Color.Red );
				output = "";
				return true;
			}
			
			if( Main.netMode == 0 ) {
				Item[] items = ShopPackDefinition.OpenPack( player, info );
				
				foreach( var hook in RewardsMod.Instance.OnPointsSpentHooks ) {
					hook( player, info.Name, info.Price, items );
				}
			} else if( Main.netMode == 1 ) {
				PackPurchaseProtocol.SendSpendToServer( info );
			}

			output = player.name + " bought " + info.Name + " (" + info.Price + ")";
			
			return true;
		}
	}
}
