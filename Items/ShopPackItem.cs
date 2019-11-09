using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Items.Attributes;
using Microsoft.Xna.Framework;
using Rewards.Logic;
using Rewards.NetProtocols;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.Items {
	public partial class ShopPackItem : ModItem {
		public static Item CreateItem( ShopPackDefinition packInfo ) {
			Item item = new Item();
			item.SetDefaults( ModContent.ItemType<ShopPackItem>() );
			item.SetNameOverride( packInfo.Name );

			var myitem = (ShopPackItem)item.modItem;
			myitem.Info = packInfo;

			return item;
		}



		////////////////

		public override bool CloneNewInstances => true;

		internal ShopPackDefinition Info = null;



		////////////////

		public ShopPackItem() { }

		internal ShopPackItem( ShopPackItem clone ) {
			this.Info = clone.Info != null ?
				new ShopPackDefinition( clone.Info ) :
				null;
		}


		////////////////

		public bool IsClone( Item item ) {
			if( !(item.modItem is ShopPackItem) ) { return false; }

			var otherModItem = (ShopPackItem)item.modItem;
			var def = (ShopPackDefinition)otherModItem.Info;

			return def.IsSameAs( (ShopPackDefinition)this.Info );
		}
		
		public override ModItem Clone() {
			var clone = (ShopPackItem)base.Clone();
			clone.Info = this.Info != null ?
				new ShopPackDefinition( this.Info ) :
				null;
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

			var myworld = ModContent.GetInstance<RewardsWorld>();
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
