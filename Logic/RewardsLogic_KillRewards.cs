using HamstarHelpers.NPCHelpers;
using HamstarHelpers.Utilities.Messages;
using HamstarHelpers.WorldHelpers;
using Microsoft.Xna.Framework;
using Rewards.NetProtocol;
using System;
using System.Collections.Generic;
using Terraria;


namespace Rewards.Logic {
	partial class RewardsLogic {
		public bool CanReceiveOtherPlayerKillRewards( RewardsMod mymod ) {
			return mymod.Config.CommunismMode;
		}


		public void BeginRewardForKill( RewardsMod mymod, NPC npc, Player player ) {
			if( Main.netMode == 0 ) {
				this.AddKillReward( mymod, npc, player );
			} else if( Main.netMode == 1 ) {
				this.AddKillReward( mymod, npc, player );
				ClientPacketHandlers.SendSignalNpcKillRewardFromClient( mymod, npc.type );
			} else if( Main.netMode == 2 ) {
				throw new Exception("Server should not signal rewards itself.");
			}
		}


		public void AddKillReward( RewardsMod mymod, int npc_type, Player player ) {
			var npc = new NPC();
			npc.SetDefaults( npc_type );

			this.AddKillReward( mymod, npc, player );
		}

		public void AddKillReward( RewardsMod mymod, NPC npc, Player player ) {
			IDictionary<int, int> killed_npcs = this.KilledNpcs;

			if( killed_npcs.ContainsKey( npc.type ) ) {
				killed_npcs[ npc.type ]++;
			} else {
				killed_npcs[ npc.type ] = 1;
			}

			bool is_grind;
			float reward = this.CalculateKillReward( mymod, npc, out is_grind );
			
			if( reward > 0.01f ) {
				string msg = "+" + Math.Round( reward, 2 ) + " PP";
				Color color = !is_grind ? Color.GreenYellow : Color.DarkGray;

				PlayerMessages.AddPlayerLabel( player, msg, color, 60 * 3, true, false );
			}

			if( mymod.Config.DebugModeInfo ) {
				Main.NewText( "AddKillReward npc type: " + npc.type + ", total: " + this.KilledNpcs[npc.type] );
			}

			this.ProgressPoints += reward;
		}


		////////////////
		
		private float CalculateKillReward( RewardsMod mymod, NPC npc, out bool is_grind ) {
			string world_uid = WorldHelpers.GetUniqueId();
			float points = 0;
			is_grind = false;
			
			if( this.CurrentInvasion != VanillaInvasionType.None ) {
				if( NPCIdentityHelpers.VanillaGoblinArmyTypes.Contains( npc.type ) ) {
					points = mymod.Config.GoblinInvasionReward / Main.invasionSizeStart;
					is_grind = this.WorldGolinsConquered[world_uid] > 0;
				} else if( NPCIdentityHelpers.VanillaFrostLegionTypes.Contains( npc.type ) ) {
					points = mymod.Config.FrostLegionInvasionReward / Main.invasionSizeStart;
					is_grind = this.WorldFrostLegionConquered[world_uid] > 0;
				} else if( NPCIdentityHelpers.VanillaPirateTypes.Contains( npc.type ) ) {
					points = mymod.Config.PirateInvasionReward / Main.invasionSizeStart;
					is_grind = this.WorldPiratesConquered[world_uid] > 0;
				} else if( NPCIdentityHelpers.VanillaMartianTypes.Contains( npc.type ) ) {
					points = mymod.Config.MartianInvasionReward / Main.invasionSizeStart;
					is_grind = this.WorldMartiansConquered[world_uid] > 0;
				} else if( NPCIdentityHelpers.VanillaPumpkingMoonTypes.Contains( npc.type ) ) {
					points = mymod.Config.PumpkingMoonWaveReward / Main.invasionSizeStart;
					is_grind = NPC.waveNumber < this.WorldPumpkinMoonWavesConquered[world_uid];
				} else if( NPCIdentityHelpers.VanillaFrostMoonTypes.Contains( npc.type ) ) {
					points = mymod.Config.FrostMoonWaveReward / Main.invasionSizeStart;
					is_grind = NPC.waveNumber < this.WorldFrostMoonWavesConquered[world_uid];
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
