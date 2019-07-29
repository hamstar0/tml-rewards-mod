using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.NPCs;
using Rewards.NetProtocols;
using System;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		public static bool CanReceiveOtherPlayerKillRewards() {
			var mymod = RewardsMod.Instance;
			return mymod.SettingsConfig.SharedRewards;
		}


		////////////////

		public float RecordKill( NPC npc, out bool isGrind, out bool isExpired ) {
			var mymod = RewardsMod.Instance;
			float reward = this.CalculateKillReward( npc, out isGrind, out isExpired );

			string name = NPCIdentityHelpers.GetQualifiedName( npc );
			bool needsBoss = mymod.PointsConfig.NpcRewardRequiredAsBoss.Contains( name );
			bool canReward = !needsBoss || ( needsBoss && npc.boss );

			if( canReward ) {
				if( this.KilledNpcs.ContainsKey( npc.type ) ) {
					this.KilledNpcs[npc.type]++;
				} else {
					this.KilledNpcs[npc.type] = 1;
				}
			}

			return reward;
		}

		public void RewardKill_Host( Player toPlayer, NPC npc ) {
			var mymod = RewardsMod.Instance;
			bool isGrind, isExpired;
			float reward = this.CalculateKillReward( npc, out isGrind, out isExpired );

			if( mymod.SettingsConfig.DebugModeKillInfo ) {
				int kills = this.KilledNpcs.ContainsKey(npc.type) ? this.KilledNpcs[ npc.type ] : -1;
				string name = NPCIdentityHelpers.GetQualifiedName( npc );
				bool needsBoss = mymod.PointsConfig.NpcRewardRequiredAsBoss.Contains( name );

				string msg = "RewardKill_SyncsFromHost to: " + toPlayer.name + ", npc: " + npc.TypeName + " (" + npc.type + ")" + ", #: " + kills
						+ ", isGrind: " + isGrind + ", reward: " + reward + ", needsBoss:" + needsBoss+" (is? "+npc.boss+")";
				Main.NewText( msg );
				LogHelpers.Log( " "+ msg );
			}

			float finalReward = this.AddRewardForPlayer( toPlayer, isGrind, isExpired, reward );

			if( Main.netMode == 2 ) {
				if( finalReward > 0 ) {	// <- Careful! Any uses for 0 reward packets?
					KillRewardProtocol.SendRewardToClient( toPlayer.whoAmI, -1, npc );
				}
			}
		}


		////////////////

		public int GetKillsOfNpc( int npcType ) {
			if( this.KilledNpcs.ContainsKey(npcType) ) {
				return this.KilledNpcs[ npcType ];
			}
			return 0;
		}
	}
}
