using HamstarHelpers.NPCHelpers;
using HamstarHelpers.WorldHelpers;
using Rewards.NetProtocol;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Rewards {
	class RewardsLogic {
		private IDictionary<string, float> WorldPoints = new Dictionary<string, float>();

		public float ProgressPoints = 0;


		////////////////

		public RewardsLogic() { }


		public void Load( TagCompound tags ) {
			if( tags.ContainsKey( "world_uid_count" ) ) {
				int world_count = tags.GetInt( "world_uid_count" );
				string curr_world_id = WorldHelpers.GetUniqueId();

				for( int i=0; i<world_count; i++ ) {
					string world_uid = tags.GetString( "world_uid_" + i );
					float pp = tags.GetFloat( "world_pp_"+i );

					this.WorldPoints[ world_uid ] = pp;

					if( world_uid.Equals( curr_world_id ) ) {
						this.ProgressPoints = pp;
					}
				}
			}
		}

		public TagCompound Save() {
			var tags = new TagCompound { { "world_uid_count", this.WorldPoints.Count } };

			this.WorldPoints[ WorldHelpers.GetUniqueId() ] = this.ProgressPoints;

			int i = 0;
			foreach( var kv in this.WorldPoints ) {
				tags.Set( "world_uid_" + i, kv.Key );
				tags.Set( "world_pp_" + i, kv.Value );
				i++;
			}

			return tags;
		}


		////////////////

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
			float reward = this.CalculateKillReward( mymod, npc );

			this.ProgressPoints += reward;
		}

		////////////////
		
		private float CalculateKillReward( RewardsMod mymod, NPC npc ) {
			float points = 0;

			if( Main.invasionProgressMax > 0 ) {
				if( NPCIdentityHelpers.VanillaGoblinArmyTypes.Contains( npc.type ) ) {
					points = mymod.Config.GoblinInvasionReward / Main.invasionSizeStart;
				} else if( NPCIdentityHelpers.VanillaFrostLegionTypes.Contains( npc.type ) ) {
					points = mymod.Config.FrostLegionInvasionReward / Main.invasionSizeStart;
				} else if( NPCIdentityHelpers.VanillaPirateTypes.Contains( npc.type ) ) {
					points = mymod.Config.PirateInvasionReward / Main.invasionSizeStart;
				} else if( NPCIdentityHelpers.VanillaMartianTypes.Contains( npc.type ) ) {
					points = mymod.Config.MartianInvasionReward / Main.invasionSizeStart;
				} else if( NPCIdentityHelpers.VanillaPumpkingMoonTypes.Contains( npc.type ) ) {
					points = mymod.Config.PumpkingMoonWaveReward / Main.invasionSizeStart;
				} else if( NPCIdentityHelpers.VanillaFrostMoonTypes.Contains( npc.type ) ) {
					points = mymod.Config.FrostMoonWaveReward / Main.invasionSizeStart;
				}
			}
			if( points == 0 ) {
				if( mymod.Config.NpcRewards.ContainsKey( npc.TypeName ) ) {
					points = mymod.Config.NpcRewards[npc.TypeName];
				}
			}

			return points;
		}
	}
}
