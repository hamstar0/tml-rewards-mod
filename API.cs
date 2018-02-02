using Rewards.Items;
using System;
using System.Collections.Generic;
using Terraria;


namespace Rewards {
	public static partial class RewardsAPI {
		public static RewardsConfigData GetModSettings() {
			return RewardsMod.Instance.Config;
		}
		
		public static void SaveModSettingsChanges() {
			RewardsMod.Instance.JsonConfig.SaveFile();
		}

		////////////////
		
		public static float GetPoints( Player player ) {
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			return myplayer.Logic.ProgressPoints;
		}

		public static void AddPoints( Player player, float points ) {
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.Logic.ProgressPoints += points;
		}

		////////////////

		public static void ShopClear() {
			var mymod = RewardsMod.Instance;
			mymod.Config.ShopLoadout = new List<ShopPackDefinition>();
			mymod.JsonConfig.SaveFile();
		}

		public static ShopPackDefinition? ShopRemoveLastPack() {
			var mymod = RewardsMod.Instance;
			IList<ShopPackDefinition> shop = mymod.Config.ShopLoadout;
			int last = shop.Count - 1;
			ShopPackDefinition? def = null;
			string _;

			for( int i=last; i>=0; i-- ) {
				if( !shop[i].Validate(out _) || !shop[i].RequirementsMet() ) { continue; }

				def = shop[i];
				shop.RemoveAt( i );

				break;
			}
			
			if( def != null ) {
				mymod.JsonConfig.SaveFile();
			}

			return def;
		}

		public static void ShopAddPack( ShopPackDefinition pack ) {
			var mymod = RewardsMod.Instance;
			string fail;
			if( !pack.Validate(out fail) ) { throw new Exception("Invalid shop pack by name "+pack.Name+" ("+fail+")"); }

			mymod.Config.ShopLoadout.Add( pack );
			mymod.JsonConfig.SaveFile();
		}
	}
}
