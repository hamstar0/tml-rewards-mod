using Microsoft.Xna.Framework;
using ModLibsCore.Services.Network.SimplePacket;
using ModLibsGeneral.Libraries.Items;
using ModLibsGeneral.Libraries.Items.Attributes;
using Rewards.Logic;
using Rewards.NetProtocols;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.Items {
	public partial class ShopPackItem : ModItem {
		public static Item CreateItem( ShopPackDefinition packInfo ) {
			Item item = new Item();
			item.SetDefaults( ModContent.ItemType<ShopPackItem>() );
			item.SetNameOverride( packInfo.Name );

			var myitem = (ShopPackItem)item.ModItem;
			myitem.Info = packInfo;

			return item;
		}



		////////////////

		protected override bool CloneNewInstances => true;

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
			if( !(item.ModItem is ShopPackItem otherModItem) ) { return false; }

			var def = (ShopPackDefinition)otherModItem.Info;

			return def.IsSameAs( (ShopPackDefinition)this.Info );
		}

		public override ModItem Clone( Item newEntity ) {
			var clone = (ShopPackItem)base.Clone( newEntity );
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
			this.Item.maxStack = 1;
			this.Item.width = 22;
			this.Item.height = 22;
			this.Item.value = 0;
			this.Item.rare = ItemRarityAttributeLibraries.QuestItemRarity;
		}


		////////////////

		public bool BuyAndOpenPack_Synced( Player player, out string output ) {
			var mymod = RewardsMod.Instance;
			ItemLibraries.DestroyItem( this.Item );

			if( this.Info == null ) {
				output = "No pack info available.";
				return false;
			}

			var info = (ShopPackDefinition)this.Info;
			int price = info.Price;

			var myworld = ModContent.GetInstance<RewardsSystem>();
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
				SimplePacket.SendToServer( new PackPurchaseProtocol( info ) );
			}

			output = player.name + " bought " + info.Name + " (" + info.Price + ")";
			
			return true;
		}
	}
}
