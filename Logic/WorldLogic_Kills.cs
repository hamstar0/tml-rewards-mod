using HamstarHelpers.DebugHelpers;
using Terraria;


namespace Rewards.Logic {
	partial class WorldLogic {
		public void AddKillReward( RewardsMod mymod, NPC npc ) {
			if( npc.lastInteraction < 0 && npc.lastInteraction >= Main.player.Length ) { return; }

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "AddKillReward " + npc.TypeName );
			}

			var myworld = mymod.GetModWorld<RewardsWorld>();

			bool to_all = KillData.CanReceiveOtherPlayerKillRewards( mymod );

			if( Main.netMode == 2 ) {
				if( to_all ) {
					for( int i = 0; i < Main.player.Length; i++ ) {
						Player to_player = Main.player[i];
						if( to_player == null || !to_player.active ) { continue; }

						this.AddKillRewardFor( mymod, to_player, npc );
					}
				} else {
					Player to_player = Main.player[npc.lastInteraction];
					if( to_player != null && to_player.active ) {
						this.AddKillRewardFor( mymod, Main.player[npc.lastInteraction], npc );
					}
				}
			} else if( Main.netMode == 0 ) {
				this.AddKillRewardFor( mymod, Main.LocalPlayer, npc );
			}

			// Also for the world
			bool _;
			this.WorldData.RecordKill( mymod, npc, out _ );
		}


		private void AddKillRewardFor( RewardsMod mymod, Player to_player, NPC npc ) {
			KillData data = this.GetPlayerData( to_player );
			if( data == null ) { return; }

			data.RecordKillAndGiveReward( mymod, to_player, npc );
		}
	}
}
