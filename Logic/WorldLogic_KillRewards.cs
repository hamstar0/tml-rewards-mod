using HamstarHelpers.DebugHelpers;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.WorldHelpers;
using Rewards.NetProtocol;
using System.Collections.Generic;
using Terraria;


namespace Rewards.Logic {
	partial class WorldLogic {
		public bool CanReceiveOtherPlayerKillRewards( RewardsMod mymod ) {
			return mymod.Config.CommunismMode;
		}

		
		public void AddKillReward( RewardsMod mymod, NPC npc ) {
			IDictionary<int, int> killed_npcs = this.KilledNpcs;

			if( killed_npcs.ContainsKey( npc.type ) ) {
				killed_npcs[ npc.type ]++;
			} else {
				killed_npcs[ npc.type ] = 1;
			}

			bool is_grind;
			float reward = this.CalculateKillReward( mymod, npc, out is_grind );
			Player player = Main.player[ npc.lastInteraction ];
			
			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "AddKillReward npc type: " + npc.type + ", total: " + this.KilledNpcs[npc.type] );
			}

			if( Main.netMode == 0 ) {
				var myplayer = player.GetModPlayer<RewardsPlayer>();
				myplayer.Logic.AddKillReward( mymod, player, npc.type, is_grind, reward );
			} else {
				int to_who = this.CanReceiveOtherPlayerKillRewards( mymod ) ? -1 : player.whoAmI;
				ServerPacketHandlers.SendNpcKillReward( mymod, player.whoAmI, npc.type, is_grind, reward, to_who, -1 );
			}
		}


		////////////////
		
		private float CalculateKillReward( RewardsMod mymod, NPC npc, out bool is_grind ) {
			string world_uid = WorldHelpers.GetUniqueId();
			float points = 0;
			is_grind = false;
			
			if( this.CurrentInvasion != VanillaInvasionType.None ) {
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
				IDictionary<int, int> killed_npcs = this.KilledNpcs;

				if( mymod.Config.NpcRewards.ContainsKey( npc.TypeName ) ) {
					points = mymod.Config.NpcRewards[ npc.TypeName ];
				}

				if( killed_npcs.ContainsKey( npc.type ) ) {
					if( mymod.Config.NpcRewardRequiredMinimumKills.ContainsKey( npc.TypeName ) ) {
						is_grind = killed_npcs[ npc.type ] >= mymod.Config.NpcRewardRequiredMinimumKills[ npc.TypeName ];
					} else {
						if( killed_npcs[npc.type] > 1 ) {
							is_grind = true;
						}
					}
				}
			}

			if( is_grind ) {
				points *= mymod.Config.GrindKillMultiplier;
			}

			return points;
		}
	}
}
