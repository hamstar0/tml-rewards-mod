using HamstarHelpers.ItemHelpers;
using HamstarHelpers.MiscHelpers;
using HamstarHelpers.Utilities.Errors;
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
		public static Item CreateItem( ShopPackDefinition pack_info ) {
			Item item = new Item();
			item.SetDefaults( RewardsMod.Instance.ItemType<ShopPackItem>() );
			item.SetNameOverride( pack_info.Name );

			var myitem = (ShopPackItem)item.modItem;
			myitem.Info = pack_info;

			return item;
		}



		////////////////

		public override bool CloneNewInstances { get { return true; } }

		internal ShopPackDefinition? Info = null;


		////////////////

		public bool IsClone( Item item ) {
			if( !(item.modItem is ShopPackItem) ) { return false; }

			var other_mod_item = (ShopPackItem)item.modItem;
			var def = (ShopPackDefinition)other_mod_item.Info;

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
			this.item.rare = ItemAttributeHelpers.QuestItemRarity;
		}


		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			if( this.Info == null ) { return; }

			tooltips.RemoveRange( 1, tooltips.Count - 1 );

			var info = (ShopPackDefinition)this.Info;
			var item_set_tip = new TooltipLine( this.mod, "Items", "Items included:" );
			tooltips.Add( item_set_tip );

			for( int i=0; i< info.Items.Length; i++ ) {
				Item item = new Item();
				item.SetDefaults( info.Items[i].ItemType );

				var item_tip = new TooltipLine( this.mod, "Item "+i, "  "+ info.Items[i].Stack + " " + info.Items[i].Name );
				item_tip.overrideColor = ItemAttributeHelpers.GetRarityColor( item.rare );

				tooltips.Add( item_tip );
			}

			Color tip_color = info.Price <= 10 ?
				Colors.CoinCopper :
				( info.Price < 100 ?
					Colors.CoinSilver :
					( info.Price < 1000 ?
						Colors.CoinGold :
						Colors.CoinPlatinum ) );
			var pp_tip = new TooltipLine( this.mod, "Custom Price", "Buy price: " + info.Price + " progress points" ) {
				overrideColor = tip_color
			};
			tooltips.Add( pp_tip );

			var instruct_tip = new TooltipLine( this.mod, "Items", "Click to purchase" );
			tooltips.Add( instruct_tip );
		}


		////////////////
		
		public override void PostDrawInInventory( SpriteBatch sb, Vector2 pos, Rectangle frame, Color draw_color, Color item_color, Vector2 origin, float scale ) {
			int bag_item_type = this.GetItemTypeOfIcon();
			if( bag_item_type == -1 ) { return; }
			
			Texture2D tex = Main.itemTexture[bag_item_type];
			float sub_scale = scale;

			if( tex.Width >= tex.Height ) {
				if( tex.Width > 20 ) {
					sub_scale /= (float)tex.Width / 20f;
				}
			} else {
				if( tex.Height > 20 ) {
					sub_scale /= (float)tex.Height / 20f;
				}
			}

			var rect = new Rectangle( 0, 0, tex.Width, tex.Height );
			pos.X += (float)frame.Width * 0.5f * scale;
			pos.Y += (float)( frame.Height + 6 ) * 0.5f * scale;
			origin = new Vector2( (float)tex.Width, (float)tex.Height ) * 0.5f;

			sb.Draw( tex, pos, rect, draw_color, 0f, origin, sub_scale, SpriteEffects.None, 1f );
		}


		public override void PostDrawInWorld( SpriteBatch sb, Color light_color, Color alpha_color, float rotation, float scale, int whoAmI ) {
			int bag_item_type = this.GetItemTypeOfIcon();
			if( bag_item_type == -1 ) { return; }
			
			Texture2D tex = Main.itemTexture[ bag_item_type ];
			float sub_scale = scale;

			if( tex.Width >= tex.Height ) {
				if( tex.Width > 20 ) {
					sub_scale /= (float)tex.Width / 20f;
				}
			} else {
				if( tex.Height > 20 ) {
					sub_scale /= (float)tex.Height / 20f;
				}
			}

			var rect = new Rectangle( 0, 0, tex.Width, tex.Height );
			var pos = new Vector2();
			pos.X += (float)item.width * 0.5f * scale;
			pos.Y += (float)(item.height + 6) * 0.5f * scale;
			var origin = new Vector2( (float)tex.Width, (float)tex.Height ) * 0.5f;

			sb.Draw( tex, pos, rect, light_color, 0f, origin, sub_scale, SpriteEffects.None, 1f );
		}


		////////////////

		public int GetItemTypeOfIcon() {
			if( this.Info == null ) { return -1; }

			var info = (ShopPackDefinition)this.Info;
			string _;
			if( !info.Validate(out _) ) { return -1; }

			for( int i = 0; i < info.Items.Length; i++ ) {
				ShopPackItemDefinition item_info = info.Items[i];
				int bag_item_type = item_info.ItemType;

				if( bag_item_type <= 0 ) { continue; }
				if( bag_item_type >= Main.itemTexture.Length ) { continue; }
				if( Main.itemTexture[ bag_item_type ] == null ) { continue; }

				return bag_item_type;
			}
			return -1;
		}

		////////////////


		public void OpenPack( RewardsMod mymod, Player player ) {
			ItemHelpers.DestroyItem( this.item );

			if( this.Info == null ) { return; }
			var info = (ShopPackDefinition)this.Info;

			var myworld = mymod.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( player );
			if( data == null ) { throw new HamstarException( "ShopPackItem.OpenPack() - No player data for " + player.name ); }
			
			if( !data.Spend( info.Price ) ) {
				Main.NewText( "Not enough progress points.", Color.Red );
				return;
			}
			
			if( Main.netMode == 1 ) {
				SpendRewardsProtocol.SendSpendToServer( info.Price );
			}

			foreach( ShopPackItemDefinition item_info in info.Items ) {
				if( !item_info.Validate() || !item_info.IsAvailable() ) { continue; }

				Item new_item = new Item();
				new_item.SetDefaults( item_info.ItemType );

				ItemHelpers.CreateItem( player.position, item_info.ItemType, item_info.Stack, new_item.width, new_item.height );
			}

			Main.PlaySound( SoundID.Coins );
		}
	}
}
