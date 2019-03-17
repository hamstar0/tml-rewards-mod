using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;


namespace Rewards.Logic {
	partial class WorldLogic {
		public void AddKillReward_Host( NPC npc ) {
			if( npc.lastInteraction < 0 && npc.lastInteraction >= Main.player.Length ) { return; }

			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();

			bool toAll = KillData.CanReceiveOtherPlayerKillRewards();

			if( Main.netMode == 2 ) {
				if( toAll ) {
					for( int i = 0; i < Main.player.Length; i++ ) {
						Player toPlayer = Main.player[i];
						if( toPlayer == null || !toPlayer.active ) { continue; }

						this.AddKillRewardForPlayer_Host( toPlayer, npc );
					}
				} else {
					Player toPlayer = Main.player[npc.lastInteraction];
					if( toPlayer != null && toPlayer.active ) {
						this.AddKillRewardForPlayer_Host( Main.player[npc.lastInteraction], npc );
					}
				}
			} else if( Main.netMode == 0 ) {
				this.AddKillRewardForPlayer_Host( Main.LocalPlayer, npc );
			}

			// Also for the world
			bool _;
			this.WorldData.RecordKill( npc, out _, out _ );
		}


		private void AddKillRewardForPlayer_Host( Player toPlayer, NPC npc ) {
			bool _;
			KillData data = this.GetPlayerData( toPlayer );
			if( data == null ) { return; }

			data.RewardKill_Host( toPlayer, npc );
			data.RecordKill( npc, out _, out _ );
		}
	}
}
