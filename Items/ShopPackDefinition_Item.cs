using HamstarHelpers.Helpers.Debug;
using Terraria;
using Terraria.ModLoader.Config;


namespace Rewards.Items {
	public class ShopPackItemDefinition {
		public ItemDefinition ItemDef;
		public int Stack;
		public bool? CrimsonWorldOnly;


		////////////////

		internal ShopPackItemDefinition( ItemDefinition itemDef, int stack, bool? crimsonOnly = null ) {
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
			if( this.ItemDef.Equals( itemDef.ItemDef ) ) { return false; }
			if( this.Stack != itemDef.Stack ) { return false; }
			if( this.CrimsonWorldOnly != itemDef.CrimsonWorldOnly ) { return false; }
			return true;
		}
	}
}
