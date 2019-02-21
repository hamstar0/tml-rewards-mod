using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using Terraria;


namespace Rewards.Logic {
	partial class WorldLogic {
		public void AddKillReward_Synced( NPC npc ) {
			if( npc.lastInteraction < 0 && npc.lastInteraction >= Main.player.Length ) { return; }

			var mymod = RewardsMod.Instance;
			if( mymod.SettingsConfig.DebugModeKillInfo ) {
				LogHelpers.Alert( NPCIdentityHelpers.GetQualifiedName(npc) );
			}

			var myworld = mymod.GetModWorld<RewardsWorld>();

			bool toAll = KillData.CanReceiveOtherPlayerKillRewards();

			if( Main.netMode == 2 ) {
				if( toAll ) {
					for( int i = 0; i < Main.player.Length; i++ ) {
						Player toPlayer = Main.player[i];
						if( toPlayer == null || !toPlayer.active ) { continue; }

						this.AddKillRewardForPlayer_Synced( toPlayer, npc );
					}
				} else {
					Player toPlayer = Main.player[npc.lastInteraction];
					if( toPlayer != null && toPlayer.active ) {
						this.AddKillRewardForPlayer_Synced( Main.player[npc.lastInteraction], npc );
					}
				}
			} else if( Main.netMode == 0 ) {
				this.AddKillRewardForPlayer_Synced( Main.LocalPlayer, npc );
			}

			// Also for the world
			bool _;
			this.WorldData.RecordKill_NoSync( npc, out _, out _ );
		}


		private void AddKillRewardForPlayer_Synced( Player toPlayer, NPC npc ) {
			bool _;
			KillData data = this.GetPlayerData( toPlayer );
			if( data == null ) { return; }

			data.RewardKill_SyncedFromServer( toPlayer, npc );
			data.RecordKill_NoSync( npc, out _, out _ );
		}
	}
}
