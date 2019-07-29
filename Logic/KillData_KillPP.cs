using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.NPCs;
using System;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		public float CalculateKillReward( NPC npc, out bool isGrind, out bool isExpired ) {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			isGrind = false;
			isExpired = false;

			float points = 0;

			if( myworld.Logic.CurrentEvents.Count != 0 ) {
				points += this.CalculateInvasionReward( npc, ref isGrind );
			}

			if( points == 0 ) {
				points += this.CalculateNpcKillReward( npc, ref isGrind, ref isExpired );
			}

			return points;
		}

		////

		private float CalculateInvasionReward( NPC npc, ref bool isGrind ) {
			var mymod = RewardsMod.Instance;
			float points = 0;
			float ppAmt = -1;

			isGrind = false;

			if( NPCGroupIdentityHelpers.VanillaGoblinArmyTypes.Contains( npc.type ) ) {
				ppAmt = mymod.PointsConfig.GoblinInvasionReward;
				isGrind = this.GoblinsConquered > 0;
			} else if( NPCGroupIdentityHelpers.VanillaFrostLegionTypes.Contains( npc.type ) ) {
				ppAmt = mymod.PointsConfig.FrostLegionInvasionReward;
				isGrind = this.FrostLegionConquered > 0;
			} else if( NPCGroupIdentityHelpers.VanillaPirateTypes.Contains( npc.type ) ) {
				ppAmt = mymod.PointsConfig.PirateInvasionReward;
				isGrind = this.PiratesConquered > 0;
			} else if( NPCGroupIdentityHelpers.VanillaMartianTypes.Contains( npc.type ) ) {
				ppAmt = mymod.PointsConfig.MartianInvasionReward;
				isGrind = this.MartiansConquered > 0;
			} else if( NPCGroupIdentityHelpers.VanillaPumpkingMoonTypes.Contains( npc.type ) ) {
				ppAmt = mymod.PointsConfig.PumpkingMoonWaveReward;
				isGrind = NPC.waveNumber < this.PumpkinMoonWavesConquered;
			} else if( NPCGroupIdentityHelpers.VanillaFrostMoonTypes.Contains( npc.type ) ) {
				ppAmt = mymod.PointsConfig.FrostMoonWaveReward;
				isGrind = NPC.waveNumber < this.FrostMoonWavesConquered;
			}

			if( ppAmt != -1 ) {
				points = ppAmt / (float)Main.invasionSizeStart;
				if( Single.IsNaN(points) || points < 0 || points > ppAmt ) {
					points = 0;
					LogHelpers.Log( "Could not compute invasion kill reward (invasion total default reward: " + ppAmt + ", invasion size: " + Main.invasionSizeStart + ")" );
				}
			}

			return points;
		}

		private float CalculateNpcKillReward( NPC npc, ref bool isGrind, ref bool isExpired ) {
			var mymod = RewardsMod.Instance;
			string npcKey = NPCIdentityHelpers.GetUniqueKey( npc );
			float points = 0;
			bool needsBoss = mymod.PointsConfig.NpcRewardRequiredAsBoss.Contains( npcKey );
			bool canReward = !needsBoss || ( needsBoss && npc.boss );

			isGrind = false;
			isExpired = false;
			
			if( mymod.PointsConfig.NpcRewards.ContainsKey( npcKey ) ) {
				if( canReward ) {
					points = mymod.PointsConfig.NpcRewards[npcKey];
				}
			}
			
			if( mymod.PointsConfig.NpcRewardNotGivenAfterNpcKilled.ContainsKey( npcKey ) ) {
				string blockingNpcName = mymod.PointsConfig.NpcRewardNotGivenAfterNpcKilled[npcKey];

				if( NPCIdentityHelpers.NamesToIds.ContainsKey( blockingNpcName ) ) {
					int blockingNpcType = NPCIdentityHelpers.NamesToIds[blockingNpcName];

					if( this.KilledNpcs.ContainsKey( blockingNpcType ) && this.KilledNpcs[blockingNpcType] > 0 ) {
						isExpired = true;
					}
				}
			}
			
			if( this.KilledNpcs.ContainsKey( npc.type ) && this.KilledNpcs[npc.type] > 0 ) {
				isGrind = canReward;
			} else {
				/*if( mymod.Config.NpcRewardPrediction ) {
					Mod boss_list_mod = ModLoader.GetMod( "BossChecklist" );
					if( boss_list_mod != null && boss_list_mod.Version >= new Version(0, 1, 5, 3) ) {
						var boss_info = (Tuple<float, int, bool>)boss_list_mod.Call( "GetBossState", name );

						if( boss_info != null && boss_info.Item2 == 0 ) {
							points = this.EstimateKillReward( mymod, npc, boss_info.Item1 );
						}
					}
				}*/
			}

			return points;
		}
	}
}
