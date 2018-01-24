using HamstarHelpers.ItemHelpers;
using HamstarHelpers.WorldHelpers;
using Rewards.Items;
using Rewards.Logic;
using Rewards.NetProtocol;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Rewards {
	class RewardsPlayer : ModPlayer {
		public PlayerLogic Logic;



		////////////////

		public override void Initialize() {
			this.Logic = new PlayerLogic();
		}


		public override void Load( TagCompound tags ) {
			this.Logic.Load( tags );
		}

		public override TagCompound Save() {
			return this.Logic.Save();
		}

		////////////////

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (RewardsPlayer)client_clone;
			clone.Logic = this.Logic;
		}


		////////////////

		public override void OnEnterWorld( Player player ) {
			var mymod = (RewardsMod)this.mod;

			if( player.whoAmI == Main.myPlayer ) {
				if( Main.netMode == 0 ) {   // Single player
					if( !mymod.JsonConfig.LoadFile() ) {
						mymod.JsonConfig.SaveFile();
					}
				}

				if( Main.netMode == 1 ) {
					ClientPacketHandlers.SendRequestModSettings( mymod );
				}
			}

			if( mymod.Config.DebugModeInfo ) {
				string world_uid = WorldHelpers.GetUniqueId();
				var myworld = mymod.GetModWorld<RewardsWorld>();

				ErrorLogger.Log( world_uid + " => " + string.Join( ", ", myworld.Logic.KilledNpcs.Select(kv => kv.Key + ":" + kv.Value).ToArray()) );
			}
		}


		////////////////

		public override void PreUpdate() {
			if( Main.myPlayer != this.player.whoAmI ) { return; }

			int pack_type = this.mod.ItemType<ShopPackItem>();

			for( int i = 0; i < this.player.inventory.Length; i++ ) {
				Item item = this.player.inventory[i];
				if( item == null || !item.active ) { continue; }
				if( item.type != pack_type ) { continue; }

				var myitem = (ShopPackItem)item.modItem;
				myitem.OpenPack( (RewardsMod)this.mod, this.player );
				
				if( myitem.IsClone(Main.mouseItem) ) {
					ItemHelpers.DestroyItem( Main.mouseItem );
					Main.mouseItem = new Item();
				}
				break;
			}

			if( Main.mouseItem.active ) {
				var myitem = Main.mouseItem.modItem as ShopPackItem;
				if( myitem != null && myitem.Info == null ) {
					ItemHelpers.DestroyItem( Main.mouseItem );
				}
			}
		}
	}
}
