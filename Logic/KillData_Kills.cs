using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using Rewards.NetProtocols;
using System;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		public static bool CanReceiveOtherPlayerKillRewards() {
			var mymod = RewardsMod.Instance;
			return mymod.SettingsConfig.SharedRewards;
		}


		public float CalculateKillReward( NPC npc, out bool isGrind, out bool isExpired ) {
			var mymod = RewardsMod.Instance;
			isGrind = false;
			isExpired = false;

			float points = 0;

			if( this.CurrentEvents.Count != 0 ) {
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

			if( NPCIdentityHelpers.VanillaGoblinArmyTypes.Contains( npc.type ) ) {
				ppAmt = mymod.PointsConfig.GoblinInvasionReward;
				isGrind = this.GoblinsConquered > 0;
			} else if( NPCIdentityHelpers.VanillaFrostLegionTypes.Contains( npc.type ) ) {
				ppAmt = mymod.PointsConfig.FrostLegionInvasionReward;
				isGrind = this.FrostLegionConquered > 0;
			} else if( NPCIdentityHelpers.VanillaPirateTypes.Contains( npc.type ) ) {
				ppAmt = mymod.PointsConfig.PirateInvasionReward;
				isGrind = this.PiratesConquered > 0;
			} else if( NPCIdentityHelpers.VanillaMartianTypes.Contains( npc.type ) ) {
				ppAmt = mymod.PointsConfig.MartianInvasionReward;
				isGrind = this.MartiansConquered > 0;
			} else if( NPCIdentityHelpers.VanillaPumpkingMoonTypes.Contains( npc.type ) ) {
				ppAmt = mymod.PointsConfig.PumpkingMoonWaveReward;
				isGrind = NPC.waveNumber < this.PumpkinMoonWavesConquered;
			} else if( NPCIdentityHelpers.VanillaFrostMoonTypes.Contains( npc.type ) ) {
				ppAmt = mymod.PointsConfig.FrostMoonWaveReward;
				isGrind = NPC.waveNumber < this.FrostMoonWavesConquered;
			}

			if( ppAmt != -1 ) {
				points = ppAmt / Main.invasionSizeStart;
				if( points < 0 || points > ppAmt ) {
					points = 0;
					LogHelpers.Log( "Could not compute invasion kill reward (invasion total default reward: " + ppAmt + ", invasion size: " + Main.invasionSizeStart + ")" );
				}
			}

			return points;
		}

		private float CalculateNpcKillReward( NPC npc, ref bool isGrind, ref bool isExpired ) {
			var mymod = RewardsMod.Instance;
			string name = NPCIdentityHelpers.GetQualifiedName( npc );
			float points = 0;
			bool needsBoss = mymod.PointsConfig.NpcRewardRequiredAsBoss.Contains( name );
			bool canReward = !needsBoss || ( needsBoss && npc.boss );

			isGrind = false;
			isExpired = false;
			
			if( mymod.PointsConfig.NpcRewards.ContainsKey( name ) ) {
				if( canReward ) {
					points = mymod.PointsConfig.NpcRewards[name];
				}
			}
			
			if( mymod.PointsConfig.NpcRewardNotGivenAfterNpcKilled.ContainsKey( name ) ) {
				string blockingNpcName = mymod.PointsConfig.NpcRewardNotGivenAfterNpcKilled[name];

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


		////////////////

		public float RecordKill_NoSync( NPC npc, out bool isGrind, out bool isExpired ) {
			var mymod = RewardsMod.Instance;
			float reward = this.CalculateKillReward( npc, out isGrind, out isExpired );

			string name = NPCIdentityHelpers.GetQualifiedName( npc );
			bool needsBoss = mymod.PointsConfig.NpcRewardRequiredAsBoss.Contains( name );

			if( !needsBoss || (needsBoss && npc.boss) ) {
				if( this.KilledNpcs.ContainsKey( npc.type ) ) {
					this.KilledNpcs[npc.type]++;
				} else {
					this.KilledNpcs[npc.type] = 1;
				}
			}

			return reward;
		}

		public void RewardKill_Synced( Player toPlayer, NPC npc ) {
			var mymod = RewardsMod.Instance;
			bool isGrind, isExpired;
			float reward = this.CalculateKillReward( npc, out isGrind, out isExpired );

			if( mymod.SettingsConfig.DebugModeKillInfo ) {
				int kills = this.KilledNpcs.ContainsKey(npc.type) ? this.KilledNpcs[ npc.type ] : -1;
				string name = NPCIdentityHelpers.GetQualifiedName( npc );
				bool needsBoss = mymod.PointsConfig.NpcRewardRequiredAsBoss.Contains( name );

				Main.NewText( "GiveKillReward to: " + toPlayer.name + ", npc: " + npc.TypeName + " (" + npc.type + ")" + ", #: " + kills + ", isGrind: " + isGrind + ", reward: " + reward + ", needsBoss:" + needsBoss+" (is? "+npc.boss+")" );
				LogHelpers.Log( " GiveKillReward to: "+toPlayer.name + ", npc: " + npc.TypeName+" ("+npc.type+")" + ", #: " + kills + ", isGrind: " + isGrind + ", reward: " + reward + ", needsBoss:" + needsBoss+" (is? "+npc.boss+")" );
			}
			
			this.AddRewardForPlayer( toPlayer, isGrind, isExpired, reward );

			if( Main.netMode == 2 ) {
				KillRewardProtocol.SendRewardToClient( toPlayer.whoAmI, -1, npc.type );
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
