using HamstarHelpers.DebugHelpers;
using HamstarHelpers.NPCHelpers;
using Rewards.NetProtocols;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		public static bool CanReceiveOtherPlayerKillRewards( RewardsMod mymod ) {
			return mymod.Config.SharedRewards;
		}
		

		public float CalculateKillReward( RewardsMod mymod, NPC npc, out bool is_grind ) {
			float points = 0;
			is_grind = false;

			if( this.CurrentEvents.Count != 0 ) {
				if( NPCIdentityHelpers.VanillaGoblinArmyTypes.Contains( npc.type ) ) {
					points = mymod.Config.GoblinInvasionReward / Main.invasionSizeStart;
					is_grind = this.GoblinsConquered > 0;
				} else if( NPCIdentityHelpers.VanillaFrostLegionTypes.Contains( npc.type ) ) {
					points = mymod.Config.FrostLegionInvasionReward / Main.invasionSizeStart;
					is_grind = this.FrostLegionConquered > 0;
				} else if( NPCIdentityHelpers.VanillaPirateTypes.Contains( npc.type ) ) {
					points = mymod.Config.PirateInvasionReward / Main.invasionSizeStart;
					is_grind = this.PiratesConquered > 0;
				} else if( NPCIdentityHelpers.VanillaMartianTypes.Contains( npc.type ) ) {
					points = mymod.Config.MartianInvasionReward / Main.invasionSizeStart;
					is_grind = this.MartiansConquered > 0;
				} else if( NPCIdentityHelpers.VanillaPumpkingMoonTypes.Contains( npc.type ) ) {
					points = mymod.Config.PumpkingMoonWaveReward / Main.invasionSizeStart;
					is_grind = NPC.waveNumber < this.PumpkinMoonWavesConquered;
				} else if( NPCIdentityHelpers.VanillaFrostMoonTypes.Contains( npc.type ) ) {
					points = mymod.Config.FrostMoonWaveReward / Main.invasionSizeStart;
					is_grind = NPC.waveNumber < this.FrostMoonWavesConquered;
				}
			}

			if( points == 0 ) {
				string name = NPCIdentityHelpers.GetQualifiedName( npc );

				if( mymod.Config.NpcRewards.ContainsKey( name ) ) {
					points = mymod.Config.NpcRewards[ name ];
				}
				
				if( this.KilledNpcs.ContainsKey( npc.type ) ) {
					if( mymod.Config.NpcRewardRequiredMinimumKills.ContainsKey( name ) ) {
						is_grind = this.KilledNpcs[npc.type] >= mymod.Config.NpcRewardRequiredMinimumKills[ name ];
					} else {
						is_grind = true;
					}
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

		public float RecordKill( RewardsMod mymod, NPC npc, out bool is_grind ) {
			float reward = this.CalculateKillReward( mymod, npc, out is_grind );

			if( this.KilledNpcs.ContainsKey( npc.type ) ) {
				this.KilledNpcs[npc.type]++;
			} else {
				this.KilledNpcs[npc.type] = 1;
			}

			return reward;
		}

		public void RewardKill( RewardsMod mymod, Player to_player, NPC npc ) {
			bool is_grind;
			float reward = this.CalculateKillReward( mymod, npc, out is_grind );

			if( mymod.Config.DebugModeInfo ) {
				int kills = this.KilledNpcs.ContainsKey(npc.type) ? this.KilledNpcs[ npc.type ] : -1;
				Main.NewText( "GiveKillReward to: " + to_player.name + ", npc: " + npc.TypeName+" ("+npc.type+")" + ", #: " + kills + ", is_grind: " + is_grind + ", reward: " + reward );
				LogHelpers.Log( "GiveKillReward to: "+to_player.name + ", npc: " + npc.TypeName+" ("+npc.type+")" + ", #: " + kills + ", is_grind: " + is_grind + ", reward: " + reward );
			}

			this.AddRewardForPlayer( mymod, to_player, is_grind, reward );

			if( Main.netMode == 2 ) {
				KillRewardProtocol.SendRewardToClient( to_player.whoAmI, -1, npc.type, is_grind, reward );
			}
		}
	}
}
