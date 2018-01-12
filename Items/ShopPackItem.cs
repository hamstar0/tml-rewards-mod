using HamstarHelpers.ItemHelpers;
using HamstarHelpers.MiscHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

		private ShopPackDefinition Info = null;


		////////////////

		public bool IsClone( Item item ) {
			if( !(item.modItem is ShopPackItem) ) { return false; }

			var other_mod_item = (ShopPackItem)item.modItem;
			return other_mod_item.Info.IsSameAs( this.Info );
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
			this.item.rare = ItemIdentityHelpers.QuestItemRarity;
		}


		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			if( this.Info == null ) { return; }

			tooltips.RemoveRange( 1, tooltips.Count - 1 );

			var item_set_tip = new TooltipLine( this.mod, "Items", "Items included:" );
			tooltips.Add( item_set_tip );

			for( int i=0; i<this.Info.Items.Length; i++ ) {
				Item item = new Item();
				item.SetDefaults( this.Info.Items[i].ItemType );

				var item_tip = new TooltipLine( this.mod, "Item "+i, "  "+ this.Info.Items[i].Stack + " " + this.Info.Items[i].Name );
				item_tip.overrideColor = MiscHelpers.GetRarityColor( item.rare );

				tooltips.Add( item_tip );
			}

			Color tip_color = this.Info.Price <= 10 ?
				Colors.CoinCopper :
				( this.Info.Price < 100 ?
					Colors.CoinSilver :
					( this.Info.Price < 1000 ?
						Colors.CoinGold :
						Colors.CoinPlatinum ) );
			var pp_tip = new TooltipLine( this.mod, "Custom Price", "Progress Points needed: " + this.Info.Price ) {
				overrideColor = tip_color
			};
			tooltips.Add( pp_tip );

			var instruct_tip = new TooltipLine( this.mod, "Items", "Click to purchase." ) {
				overrideColor = Color.Red
			};
			tooltips.Add( instruct_tip );
		}


		////////////////
		
		public override void PostDrawInInventory( SpriteBatch sb, Vector2 pos, Rectangle frame, Color draw_color, Color item_color, Vector2 origin, float scale ) {
			int bag_item_type = this.GetItemTypeOfIcon();
			if( bag_item_type == -1 ) { return; }
			
			Texture2D tex = Main.itemTexture[ bag_item_type ];
			var rect = new Rectangle( (int)pos.X, (int)pos.Y, tex.Width / 2, tex.Height / 2 );

			sb.Draw( tex, rect, Color.White );
		}

		public override void PostDrawInWorld( SpriteBatch sb, Color light_color, Color alpha_olor, float rotation, float scale, int whoAmI ) {
			int bag_item_type = this.GetItemTypeOfIcon();
			if( bag_item_type == -1 ) { return; }
			
			Texture2D tex = Main.itemTexture[ bag_item_type ];
			var rect = new Rectangle( (int)this.item.position.X, (int)this.item.position.Y, (int)( (float)tex.Width * scale ), (int)( (float)tex.Height * scale ) );

			sb.Draw( tex, rect, Color.White );
		}


		////////////////

		public int GetItemTypeOfIcon() {
			if( this.Info == null ) { return -1; }

			for( int i = 0; i < this.Info.Items.Length; i++ ) {
				ShopPackItemDefinition item_info = this.Info.Items[i];
				int bag_item_type = item_info.ItemType;

				if( !item_info.IsValidItem() ) { continue; }
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

			if( this.Info == null ) {
				return;
			}

			var myplayer = player.GetModPlayer<RewardsPlayer>();
			
			if( !myplayer.ChargePlayer(this.Info.Price) ) {
				return;
			}
			
			foreach( ShopPackItemDefinition info in this.Info.Items ) {
				if( !info.IsValidItem() ) { continue; }

				Item new_item = new Item();
				new_item.SetDefaults( info.ItemType );

				ItemHelpers.CreateItem( player.position, info.ItemType, info.Stack, new_item.width, new_item.height );
			}

			Main.PlaySound( SoundID.Coins );
		}
	}
}
