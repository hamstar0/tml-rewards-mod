using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Config;


namespace Rewards.Items {
	public class ShopPackDefinition {
		public static Item[] OpenPack( Player player, ShopPackDefinition packDef ) {
			Item[] packItems = new Item[ packDef.Items.Count ];
			int i = 0;

			foreach( ShopPackItemDefinition itemInfo in packDef.Items ) {
				if( !itemInfo.IsAvailable() ) { continue; }

				int itemType = itemInfo.ItemDef.Type;
				Item newItem = new Item();
				newItem.SetDefaults( itemType );

				ItemHelpers.CreateItem( player.position, itemType, itemInfo.Stack, newItem.width, newItem.height );

				packItems[i++] = newItem;
			}

			Main.PlaySound( SoundID.Coins );

			return packItems;
		}

		public static ShopPackDefinition[] GetValidatedLoadout( bool outputValidationErrors ) {
			var mymod = RewardsMod.Instance;
			var defs = new List<ShopPackDefinition>();

			for( int i = 0; i < mymod.ShopConfig.ShopLoadout.Count; i++ ) {
				ShopPackDefinition def = mymod.ShopConfig.ShopLoadout[i];
				string fail;

				if( !def.Validate( out fail ) ) {
					if( outputValidationErrors ) {
						Main.NewText( "Could not load shop item " + def.Name + " (" + fail + ")", Color.Red );
					}
					continue;
				}
				if( !def.RequirementsMet() ) {
					continue;
				}

				defs.Add( def );
			}

			return defs.ToArray();
		}



		////////////////

		public NPCDefinition NeededBossKill { get; set; }
		public string Name { get; set; }
		public int Price { get; set; }
		public List<ShopPackItemDefinition> Items { get; set; }



		////////////////

		public ShopPackDefinition( NPCDefinition neededBoss, string name, int price, List<ShopPackItemDefinition> items ) {
			this.NeededBossKill = neededBoss;
			this.Name = name;
			this.Price = price;
			this.Items = items;
		}

		////////////////

		public bool Validate( out string error ) {
			if( string.IsNullOrEmpty( this.Name ) ) {
				error = "bad name";
				return false;
			}
			if( this.Items.Count == 0 ) {
				error = "no items";
				return false;
			}
			//foreach( ShopPackItemDefinition itemInfo in this.Items ) {
			//	if( !itemInfo.Validate() ) {
			//		error = itemInfo.ItemDef;
			//		return false;
			//	}
			//}
			error = null;
			return true;
		}

		public bool IsSameAs( ShopPackDefinition def ) {
			int count = this.Items.Count;

			if( !def.Name.Equals(this.Name) ) { return false; }
			if( def.Price != this.Price ) { return false; }
			if( def.NeededBossKill != this.NeededBossKill ) { return false; }
			if( def.Items.Count != count ) { return false; }

			for( int i=0; i<count; i++ ) {
				if( this.Items[i].IsSameAs( def.Items[i] ) ) { return false; }
			}
			return true;
		}

		
		public bool RequirementsMet() {
			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();

			if( this.NeededBossKill == null ) {
				return true;
			}
			if( myworld.Logic.WorldData.GetKillsOfNpc( this.NeededBossKill.Type ) > 0 ) {
				return true;
			}

			return false;
		}
	}
}
