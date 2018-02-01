using HamstarHelpers.ItemHelpers;
using HamstarHelpers.NPCHelpers;
using Terraria;


namespace Rewards.Items {
	public struct ShopPackDefinition {
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

		public bool Validate( out string which ) {
			if( string.IsNullOrEmpty( this.Name ) ) {
				which = "bad name";
				return false;
			}
			if( this.Items.Length == 0 ) {
				which = "no items";
				return false;
			}
			foreach( ShopPackItemDefinition item_info in this.Items ) {
				if( !item_info.Validate() ) {
					which = item_info.Name;
					return false;
				}
			}
			which = null;
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
			if( string.IsNullOrEmpty(this.NeededBossKill) ) { return true; }

			int npc_type;
			if( !NPCIdentityHelpers.NamesToIds.TryGetValue( this.NeededBossKill, out npc_type ) ) {
				return false;
			}

			return NPCHelpers.CurrentPlayerKillsOfNpc( npc_type ) > 0;
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
					if( ItemIdentityHelpers.NamesToIds.ContainsKey( this.Name ) ) {
						this._ItemType = ItemIdentityHelpers.NamesToIds[this.Name];
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
