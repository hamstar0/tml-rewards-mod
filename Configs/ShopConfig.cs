using HamstarHelpers.Helpers.TModLoader.Configs;
using Newtonsoft.Json;
using Rewards.Items;
using System;
using System.Collections.Generic;
using Terraria.ModLoader.Config;


namespace Rewards.Configs {
	public partial class RewardsShopConfig : ModConfig {
		public static bool UpdatePackIfFound( string name, int oldPrice, IList<ShopPackDefinition> oldShop, IList<ShopPackDefinition> newShop ) {
			for( int i = 0; i < oldShop.Count; i++ ) {
				if( oldShop[i].Name != name ) { continue; }

				for( int j = 0; j < newShop.Count; j++ ) {
					if( newShop[j].Name != name ) { continue; }

					if( oldShop[i].Price == oldPrice ) {
						oldShop[i] = newShop[j];
						return true;
					}
					return false;
				}
			}
			return false;
		}



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[Label( "Wayfarer's shop loadout" )]
		public List<ShopPackDefinition> ShopLoadout = new List<ShopPackDefinition>();



		////////////////

		public override ModConfig Clone() {
			var clone = (RewardsShopConfig)base.Clone();

			clone.ShopLoadout = new List<ShopPackDefinition>( this.ShopLoadout );

			return clone;
		}


		////////////////

		public void Reset() {
			JsonConvert.PopulateObject( "{}", this, ConfigManager.serializerSettings );
			this.ShopLoadout = new List<ShopPackDefinition>();

			ConfigHelpers.SyncConfig( this );
		}
	}
}
