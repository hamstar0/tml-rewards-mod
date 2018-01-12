using HamstarHelpers.ItemHelpers;
using System;
using Terraria;


namespace Rewards.Items {
	public class ShopPackDefinition {
		public string Name;
		public int Price;
		public ShopPackItemDefinition[] Items;


		////////////////

		public ShopPackDefinition( string name, int price, ShopPackItemDefinition[] items ) {
			this.Name = name;
			this.Price = price;
			this.Items = items;

			if( !this.Validate() ) {
				throw new Exception( "Shop options must have at least 1 item." );
			}
		}

		public bool Validate() {
			return !string.IsNullOrEmpty( this.Name ) && this.Items.Length > 0;
		}

		public bool IsSameAs( ShopPackDefinition def ) {
			if( !def.Name.Equals(this.Name) ) { return false; }
			if( def.Price != this.Price ) { return false; }
			if( def.Items.Length != this.Items.Length ) { return false; }
			for( int i=0; i<this.Items.Length; i++ ) {
				if( this.Items[i].IsSameAs( def.Items[i] ) ) { return false; }
			}
			return true;
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
					if( !ItemIdentityHelpers.NamesToIds.ContainsKey( this.Name ) ) {
						throw new Exception( "No such item of type " + this.Name );
					}
					this._ItemType = ItemIdentityHelpers.NamesToIds[ this.Name ];
				}
				return this._ItemType;
			}
		}


		////////////////

		public ShopPackItemDefinition( string name, int stack, bool? crimson_only = null ) {
			this.Name = name;
			this.Stack = stack;
			this.CrimsonWorldOnly = crimson_only;
			this._ItemType = -1;
		}


		public bool IsValidItem() {
			if( this.CrimsonWorldOnly != null ) {
				if( this.CrimsonWorldOnly == false ) {
					if( WorldGen.crimson ) { return false; }
				} else {
					if( !WorldGen.crimson ) { return false; }
				}
			}
			return true;
		}

		public bool IsSameAs( ShopPackItemDefinition item_def ) {
			if( this.Name.Equals( item_def.Name ) ) { return false; }
			if( this.Stack != item_def.Stack ) { return false; }
			if( this.CrimsonWorldOnly != item_def.CrimsonWorldOnly ) { return false; }
			return true;
		}
	}
}
