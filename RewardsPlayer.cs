using HamstarHelpers.DebugHelpers;
using HamstarHelpers.ItemHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Rewards.Items;
using Rewards.Logic;
using Rewards.NetProtocol;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Rewards {
	class RewardsPlayer : ModPlayer {
		public RewardsLogic Logic;



		////////////////

		public override void Initialize() {
			this.Logic = new RewardsLogic();
		}
		
		public override void clientClone( ModPlayer client_clone ) {
			var clone = (RewardsPlayer)client_clone;
			clone.Logic = this.Logic;
		}


		public override void Load( TagCompound tags ) {
			this.Logic.Load( tags );
		}

		public override TagCompound Save() {
			return this.Logic.Save();
		}

		////////////////

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			var mymod = (RewardsMod)this.mod;

			if( Main.netMode == 1 ) {
				if( new_player ) {
					ClientPacketHandlers.SendRequestModSettingsFromClient( mymod );
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			var mymod = (RewardsMod)this.mod;

			if( Main.netMode == 0 ) {   // Single player
				if( !mymod.JsonConfig.LoadFile() ) {
					mymod.JsonConfig.SaveFile();
				}
			}

			this.Logic.OnEnterWorld();
		}


		////////////////

		public override void OnHitNPC( Item item, NPC target, int damage, float knockback, bool crit ) {
			if( this.player.whoAmI == Main.myPlayer ) {
				if( target.life <= 0 ) {
					this.Logic.BeginRewardForKill( (RewardsMod)this.mod, target, this.player );
				}
			}
		}

		public override void OnHitNPCWithProj( Projectile proj, NPC target, int damage, float knockback, bool crit ) {
			if( this.player.whoAmI == Main.myPlayer && proj.owner == Main.myPlayer ) {
				if( target.life <= 0 && !proj.npcProj ) {
					this.Logic.BeginRewardForKill( (RewardsMod)this.mod, target, this.player );
				}
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

			this.Logic.UpdateInvasions();
		}


		////////////////

		public bool ChargePlayer( int points ) {
			if( this.Logic.ProgressPoints < points ) {
				return false;
			}

			this.Logic.ProgressPoints -= points;

			return true;
		}


		////////////////

		public void DrawPointScore( SpriteBatch sb ) {
			var mymod = (RewardsMod)this.mod;
			if( !mymod.Config.PointsDisplayWithoutInventory && Main.playerInventory ) { return; }

			float pos_x = mymod.Config.PointsDisplayX;
			float pos_y = mymod.Config.PointsDisplayY;
			pos_x = pos_x < 0 ? Main.screenWidth + pos_x : pos_x;
			pos_y = pos_y < 0 ? Main.screenHeight + pos_y : pos_y;

			sb.DrawString( Main.fontMouseText, "PP: "+(int)this.Logic.ProgressPoints, new Vector2( pos_x, pos_y ), mymod.Config.PointsDisplayColor );
		}
	}
}
