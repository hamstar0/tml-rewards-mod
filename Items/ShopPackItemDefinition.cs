using HamstarHelpers.Helpers.Debug;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;


namespace Rewards.Items {
	public class ShopPackItemDefinition {
		public ItemDefinition ItemDef { get; set; }

		[Label("Item stack size")]
		[Range(1, 9999)]
		[DefaultValue(1)]
		public int Stack { get; set; }

		public bool? CrimsonWorldOnly { get; set; }



		////////////////

		public ShopPackItemDefinition() { }

		internal ShopPackItemDefinition( ShopPackItemDefinition clone ) {
			this.ItemDef = clone.ItemDef != null ?
				new ItemDefinition( clone.ItemDef.mod, clone.ItemDef.name ) :
				null;
			this.Stack = clone.Stack;
			this.CrimsonWorldOnly = clone.CrimsonWorldOnly;
		}

		public ShopPackItemDefinition( ItemDefinition itemDef, int stack, bool? crimsonOnly = null ) {
			this.ItemDef = itemDef;
			this.Stack = stack;
			this.CrimsonWorldOnly = crimsonOnly;
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

		public bool IsSameAs( ShopPackItemDefinition itemDef ) {
			if( this.ItemDef == null ) { return false; }
			if( this.ItemDef.name == itemDef.ItemDef?.name ) { return false; }
			if( this.ItemDef.mod == itemDef.ItemDef?.mod ) { return false; }
			if( this.Stack != itemDef.Stack ) { return false; }
			if( this.CrimsonWorldOnly != itemDef.CrimsonWorldOnly ) { return false; }
			return true;
		}
	}
}
