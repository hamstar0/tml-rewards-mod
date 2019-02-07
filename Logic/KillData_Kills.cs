using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using Rewards.NetProtocols;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		public static bool CanReceiveOtherPlayerKillRewards( RewardsMod mymod ) {
			return mymod.Config.SharedRewards;
		}
		

		public float CalculateKillReward( RewardsMod mymod, NPC npc, out bool isGrind, out bool isExpired ) {
			isGrind = false;
			isExpired = false;

			float points = 0;

			if( this.CurrentEvents.Count != 0 ) {
				float ppAmt = -1;

				if( NPCIdentityHelpers.VanillaGoblinArmyTypes.Contains( npc.type ) ) {
					ppAmt = mymod.Config.GoblinInvasionReward;
					isGrind = this.GoblinsConquered > 0;
				} else if( NPCIdentityHelpers.VanillaFrostLegionTypes.Contains( npc.type ) ) {
					ppAmt = mymod.Config.FrostLegionInvasionReward;
					isGrind = this.FrostLegionConquered > 0;
				} else if( NPCIdentityHelpers.VanillaPirateTypes.Contains( npc.type ) ) {
					ppAmt = mymod.Config.PirateInvasionReward;
					isGrind = this.PiratesConquered > 0;
				} else if( NPCIdentityHelpers.VanillaMartianTypes.Contains( npc.type ) ) {
					ppAmt = mymod.Config.MartianInvasionReward;
					isGrind = this.MartiansConquered > 0;
				} else if( NPCIdentityHelpers.VanillaPumpkingMoonTypes.Contains( npc.type ) ) {
					ppAmt = mymod.Config.PumpkingMoonWaveReward;
					isGrind = NPC.waveNumber < this.PumpkinMoonWavesConquered;
				} else if( NPCIdentityHelpers.VanillaFrostMoonTypes.Contains( npc.type ) ) {
					ppAmt = mymod.Config.FrostMoonWaveReward;
					isGrind = NPC.waveNumber < this.FrostMoonWavesConquered;
				}

				if( ppAmt != -1 ) {
					points = ppAmt / Main.invasionSizeStart;
					if( points < 0 || points > ppAmt ) {
						points = 0;
						LogHelpers.Log( "Could not compute invasion kill reward (invasion total default reward: "+ppAmt+", invasion size: "+Main.invasionSizeStart+")" );
					}
				}
			}

			if( points == 0 ) {
				string name = NPCIdentityHelpers.GetQualifiedName( npc );

				if( mymod.Config.NpcRewards.ContainsKey( name ) ) {
					points = mymod.Config.NpcRewards[ name ];
				}

				if( mymod.Config.NpcRewardRequiredAsBoss.Contains( name ) ) {
					if( !npc.boss ) {
						points = 0;
					}
				}
				
				if( mymod.Config.NpcRewardNotGivenAfterNpcKilled.ContainsKey(name) ) {
					string blockingNpcName = mymod.Config.NpcRewardNotGivenAfterNpcKilled[ name ];

					if( NPCIdentityHelpers.NamesToIds.ContainsKey( blockingNpcName ) ) {
						int blockingNpcType = NPCIdentityHelpers.NamesToIds[blockingNpcName];
						
						if( this.KilledNpcs.ContainsKey( blockingNpcType ) && this.KilledNpcs[blockingNpcType] > 0 ) {
							isExpired = true;
						}
					}
				}
				
				if( this.KilledNpcs.ContainsKey( npc.type ) && this.KilledNpcs[npc.type] > 0 ) {
					isGrind = true;
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
			}

			return points;
		}


		////////////////

		public float RecordKill_NoSync( RewardsMod mymod, NPC npc, out bool isGrind, out bool isExpired ) {
			float reward = this.CalculateKillReward( mymod, npc, out isGrind, out isExpired );
			
			if( this.KilledNpcs.ContainsKey( npc.type ) ) {
				this.KilledNpcs[npc.type]++;
			} else {
				this.KilledNpcs[npc.type] = 1;
			}

			return reward;
		}

		public void RewardKill_Synced( RewardsMod mymod, Player toPlayer, NPC npc ) {
			bool isGrind, isExpired;
			float reward = this.CalculateKillReward( mymod, npc, out isGrind, out isExpired );

			if( mymod.Config.DebugModeKillInfo ) {
				int kills = this.KilledNpcs.ContainsKey(npc.type) ? this.KilledNpcs[ npc.type ] : -1;
				Main.NewText( "GiveKillReward to: " + toPlayer.name + ", npc: " + npc.TypeName+" ("+npc.type+")" + ", #: " + kills + ", isGrind: " + isGrind + ", reward: " + reward );
				LogHelpers.Log( " GiveKillReward to: "+toPlayer.name + ", npc: " + npc.TypeName+" ("+npc.type+")" + ", #: " + kills + ", isGrind: " + isGrind + ", reward: " + reward );
			}
			
			this.AddRewardForPlayer( mymod, toPlayer, isGrind, isExpired, reward );

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
