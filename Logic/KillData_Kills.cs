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
				if( mymod.Config.NpcRewards.ContainsKey( npc.TypeName ) ) {
					points = mymod.Config.NpcRewards[npc.TypeName];
				}
				
				if( this.KilledNpcs.ContainsKey( npc.type ) ) {
					if( mymod.Config.NpcRewardRequiredMinimumKills.ContainsKey( npc.TypeName ) ) {
						is_grind = this.KilledNpcs[npc.type] >= mymod.Config.NpcRewardRequiredMinimumKills[npc.TypeName];
					} else {
						is_grind = true;
					}
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

		public void RecordKillAndGiveReward( RewardsMod mymod, Player to_player, NPC npc ) {
			bool is_grind;
			float reward = this.RecordKill( mymod, npc, out is_grind );

			if( mymod.Config.DebugModeInfo ) {
				Main.NewText( "GiveKillReward to: " + to_player.name + ", npc: " + npc.TypeName+" ("+npc.type+")" + ", #: " + this.KilledNpcs[npc.type] + ", is_grind: " + is_grind + ", reward: " + reward );
				LogHelpers.Log( "GiveKillReward to: "+to_player.name + ", npc: " + npc.TypeName+" ("+npc.type+")" + ", #: " + this.KilledNpcs[npc.type] + ", is_grind: " + is_grind + ", reward: " + reward );
			}

			this.AddRewardForPlayer( mymod, to_player, is_grind, reward );

			if( Main.netMode == 2 ) {
				KillRewardProtocol.SendRewardToClient( to_player.whoAmI, -1, npc.type, is_grind, reward );
			}

			foreach( var hook in mymod.OnRewardHooks ) {
				hook( to_player, reward );
			}
		}
	}
}
