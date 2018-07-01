using HamstarHelpers.DebugHelpers;
using HamstarHelpers.NPCHelpers;
using Rewards.NetProtocols;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		public static bool CanReceiveOtherPlayerKillRewards( RewardsMod mymod ) {
			return mymod.Config.SharedRewards;
		}
		

		public float CalculateKillReward( RewardsMod mymod, NPC npc, out bool is_grind, out bool is_expired ) {
			is_grind = false;
			is_expired = false;

			float points = 0;

			if( this.CurrentEvents.Count != 0 ) {
				float pp_amt = -1;

				if( NPCIdentityHelpers.VanillaGoblinArmyTypes.Contains( npc.type ) ) {
					pp_amt = mymod.Config.GoblinInvasionReward;
					is_grind = this.GoblinsConquered > 0;
				} else if( NPCIdentityHelpers.VanillaFrostLegionTypes.Contains( npc.type ) ) {
					pp_amt = mymod.Config.FrostLegionInvasionReward;
					is_grind = this.FrostLegionConquered > 0;
				} else if( NPCIdentityHelpers.VanillaPirateTypes.Contains( npc.type ) ) {
					pp_amt = mymod.Config.PirateInvasionReward;
					is_grind = this.PiratesConquered > 0;
				} else if( NPCIdentityHelpers.VanillaMartianTypes.Contains( npc.type ) ) {
					pp_amt = mymod.Config.MartianInvasionReward;
					is_grind = this.MartiansConquered > 0;
				} else if( NPCIdentityHelpers.VanillaPumpkingMoonTypes.Contains( npc.type ) ) {
					pp_amt = mymod.Config.PumpkingMoonWaveReward;
					is_grind = NPC.waveNumber < this.PumpkinMoonWavesConquered;
				} else if( NPCIdentityHelpers.VanillaFrostMoonTypes.Contains( npc.type ) ) {
					pp_amt = mymod.Config.FrostMoonWaveReward;
					is_grind = NPC.waveNumber < this.FrostMoonWavesConquered;
				}

				if( pp_amt != -1 ) {
					points = pp_amt / Main.invasionSizeStart;
					if( points < 0 || points > pp_amt ) {
						points = 0;
						LogHelpers.Log( "Could not compute invasion kill reward (invasion total default reward: "+pp_amt+", invasion size: "+Main.invasionSizeStart+")" );
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
					string blocking_npc_name = mymod.Config.NpcRewardNotGivenAfterNpcKilled[ name ];

					if( NPCIdentityHelpers.NamesToIds.ContainsKey( blocking_npc_name ) ) {
						int blocking_npc_type = NPCIdentityHelpers.NamesToIds[blocking_npc_name];

						if( this.KilledNpcs.ContainsKey( blocking_npc_type ) && this.KilledNpcs[blocking_npc_type] > 0 ) {
							is_expired = true;
						}
					}
				}

				if( this.KilledNpcs.ContainsKey( npc.type ) && this.KilledNpcs[npc.type] > 0 ) {
					is_grind = true;
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

		public float RecordKillLocal( RewardsMod mymod, NPC npc, out bool is_grind, out bool is_expired ) {
			float reward = this.CalculateKillReward( mymod, npc, out is_grind, out is_expired );

			if( this.KilledNpcs.ContainsKey( npc.type ) ) {
				this.KilledNpcs[npc.type]++;
			} else {
				this.KilledNpcs[npc.type] = 1;
			}

			return reward;
		}

		public void RewardKillSynced( RewardsMod mymod, Player to_player, NPC npc ) {
			bool is_grind, is_expired;
			float reward = this.CalculateKillReward( mymod, npc, out is_grind, out is_expired );

			if( mymod.Config.DebugModeInfo ) {
				int kills = this.KilledNpcs.ContainsKey(npc.type) ? this.KilledNpcs[ npc.type ] : -1;
				Main.NewText( "GiveKillReward to: " + to_player.name + ", npc: " + npc.TypeName+" ("+npc.type+")" + ", #: " + kills + ", is_grind: " + is_grind + ", reward: " + reward );
				LogHelpers.Log( " GiveKillReward to: "+to_player.name + ", npc: " + npc.TypeName+" ("+npc.type+")" + ", #: " + kills + ", is_grind: " + is_grind + ", reward: " + reward );
			}

			this.AddRewardForPlayerNoSync( mymod, to_player, is_grind, is_expired, reward );

			if( Main.netMode == 2 ) {
				KillRewardProtocol.SendRewardToClient( to_player.whoAmI, -1, npc.type );
			}
		}
	}
}
