using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using Terraria;


namespace Rewards.Logic {
	partial class WorldLogic {
		public void AddKillReward_Synced( RewardsMod mymod, NPC npc ) {
			if( npc.lastInteraction < 0 && npc.lastInteraction >= Main.player.Length ) { return; }

			if( mymod.Config.DebugModeKillInfo ) {
				LogHelpers.Log( "Rewards.WorldLogic.AddKillReward " + NPCIdentityHelpers.GetQualifiedName(npc) );
			}
			
			var myworld = mymod.GetModWorld<RewardsWorld>();

			bool to_all = KillData.CanReceiveOtherPlayerKillRewards( mymod );

			if( Main.netMode == 2 ) {
				if( to_all ) {
					for( int i = 0; i < Main.player.Length; i++ ) {
						Player to_player = Main.player[i];
						if( to_player == null || !to_player.active ) { continue; }

						this.AddKillRewardForPlayer_Synced( mymod, to_player, npc );
					}
				} else {
					Player to_player = Main.player[npc.lastInteraction];
					if( to_player != null && to_player.active ) {
						this.AddKillRewardForPlayer_Synced( mymod, Main.player[npc.lastInteraction], npc );
					}
				}
			} else if( Main.netMode == 0 ) {
				this.AddKillRewardForPlayer_Synced( mymod, Main.LocalPlayer, npc );
			}

			// Also for the world
			bool _;
			this.WorldData.RecordKill_NoSync( mymod, npc, out _, out _ );
		}


		private void AddKillRewardForPlayer_Synced( RewardsMod mymod, Player to_player, NPC npc ) {
			bool _;
			KillData data = this.GetPlayerData( to_player );
			if( data == null ) { return; }

			data.RewardKill_Synced( mymod, to_player, npc );
			data.RecordKill_NoSync( mymod, npc, out _, out _ );
		}
	}
}
