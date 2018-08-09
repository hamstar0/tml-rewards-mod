using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace Rewards.Items {
	public struct ShopPackDefinition {
		public static Item[] OpenPack( Player player, ShopPackDefinition pack_def ) {
			Item[] pack_items = new Item[ pack_def.Items.Length ];
			int i = 0;

			foreach( ShopPackItemDefinition item_info in pack_def.Items ) {
				if( !item_info.Validate() || !item_info.IsAvailable() ) { continue; }

				Item new_item = new Item();
				new_item.SetDefaults( item_info.ItemType );

				ItemHelpers.CreateItem( player.position, item_info.ItemType, item_info.Stack, new_item.width, new_item.height );

				pack_items[i++] = new_item;
			}

			Main.PlaySound( SoundID.Coins );

			return pack_items;
		}



		////////////////

		public string NeededBossKill;
		public string Name;
		public int Price;
		public ShopPackItemDefinition[] Items;



		////////////////

		internal ShopPackDefinition( string needed_boss, string name, int price, ShopPackItemDefinition[] items ) {
			this.NeededBossKill = needed_boss;
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
			if( this.Items.Length == 0 ) {
				error = "no items";
				return false;
			}
			foreach( ShopPackItemDefinition item_info in this.Items ) {
				if( !item_info.Validate() ) {
					error = item_info.Name;
					return false;
				}
			}
			error = null;
			return true;
		}

		public bool IsSameAs( ShopPackDefinition def ) {
			if( !def.Name.Equals(this.Name) ) { return false; }
			if( def.Price != this.Price ) { return false; }
			if( def.NeededBossKill != this.NeededBossKill ) { return false; }
			if( def.Items.Length != this.Items.Length ) { return false; }
			for( int i=0; i<this.Items.Length; i++ ) {
				if( this.Items[i].IsSameAs( def.Items[i] ) ) { return false; }
			}
			return true;
		}

		
		public bool RequirementsMet() {
			if( string.IsNullOrEmpty(this.NeededBossKill) ) {
				return true;
			}

			ISet<int> npc_types;
			if( !NPCIdentityHelpers.NamesToIds.TryGetValues( this.NeededBossKill, out npc_types ) ) {
				Main.NewText( "Required kill npc " + this.NeededBossKill + " for "+this.Name+" not found." );
				LogHelpers.Log( " Required kill npc " + this.NeededBossKill + " for " + this.Name + " not found." );
				return false;
			}

			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();

			foreach( int npc_type in npc_types ) {
				if( myworld.Logic.WorldData.GetKillsOfNpc(npc_type) > 0 ) {
					return true;
				}
			}
			return false;
		}
	}



	public struct ShopPackItemDefinition {
		public string Name;
		public int Stack;
		public bool? CrimsonWorldOnly;

		private int _ItemType;
		internal int ItemType {
			get {
				if( this._ItemType <= 0 ) {
					string name = this.Name == null ? "" : this.Name;

					if( ItemIdentityHelpers.NamesToIds.ContainsKey( name ) ) {
						this._ItemType = ItemIdentityHelpers.NamesToIds[ name ];
					}
				}

				return this._ItemType;
			}
		}


		////////////////

		internal ShopPackItemDefinition( string name, int stack, bool? crimson_only = null ) {
			this.Name = name;
			this.Stack = stack;
			this.CrimsonWorldOnly = crimson_only;
			this._ItemType = -1;
		}

		////////////////

		public bool IsAvailable() {
			if( this.CrimsonWorldOnly != null ) {
				if( this.CrimsonWorldOnly == false ) {
					if( WorldGen.crimson ) { return false; }
				} else {
					if( !WorldGen.crimson ) { return false; }
				}
			}
			return true;
		}

		public bool Validate() {
			return this.ItemType > 0;
		}

		public bool IsSameAs( ShopPackItemDefinition item_def ) {
			if( this.Name.Equals( item_def.Name ) ) { return false; }
			if( this.Stack != item_def.Stack ) { return false; }
			if( this.CrimsonWorldOnly != item_def.CrimsonWorldOnly ) { return false; }
			return true;
		}
	}
}
