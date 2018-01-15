using HamstarHelpers.NPCHelpers;
using HamstarHelpers.WorldHelpers;
using Rewards.NetProtocol;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;


namespace Rewards {
	class RewardsLogic {
		internal IDictionary<string, float> WorldPoints = new Dictionary<string, float>();
		internal IDictionary<string, IDictionary<int, int>> WorldKills = new Dictionary<string, IDictionary<int, int>>();
		internal IDictionary<string, int> WorldGolinsConquered = new Dictionary<string, int>();
		internal IDictionary<string, int> WorldFrostLegionConquered = new Dictionary<string, int>();
		internal IDictionary<string, int> WorldPiratesConquered = new Dictionary<string, int>();
		internal IDictionary<string, int> WorldMartiansConquered = new Dictionary<string, int>();
		internal IDictionary<string, int> WorldPumpkinMoonWavesConquered = new Dictionary<string, int>();
		internal IDictionary<string, int> WorldFrostMoonWavesConquered = new Dictionary<string, int>();

		private IDictionary<int, int> KilledNpcs { get {
			return this.WorldKills[ WorldHelpers.GetUniqueId() ];
		} }
		
		public float ProgressPoints = 0;

		private VanillaInvasionType CurrentInvasion = VanillaInvasionType.None;



		////////////////

		public RewardsLogic() {
			this.CurrentInvasion = NPCInvasionHelpers.GetCurrentInvasionType();
		}


		public void Load( TagCompound tags ) {
			string curr_world_id = WorldHelpers.GetUniqueId();

			this.WorldKills[ curr_world_id ] = new Dictionary<int, int>();
			this.WorldGolinsConquered[ curr_world_id ] = 0;
			this.WorldFrostLegionConquered[ curr_world_id ] = 0;
			this.WorldPiratesConquered[ curr_world_id ] = 0;
			this.WorldMartiansConquered[ curr_world_id ] = 0;
			this.WorldPumpkinMoonWavesConquered[ curr_world_id ] = 0;
			this.WorldFrostMoonWavesConquered[ curr_world_id ] = 0;

			if( tags.ContainsKey( "world_uid_count" ) ) {
				int world_count = tags.GetInt( "world_uid_count" );

				for( int i=0; i<world_count; i++ ) {
					string world_uid = tags.GetString( "world_uid_" + i );
					float pp = tags.GetFloat( "world_pp_"+i );

					this.WorldPoints[ world_uid ] = pp;

					if( world_uid.Equals( curr_world_id ) ) {
						this.ProgressPoints = pp;
					}

					if( tags.ContainsKey( "world_kills_"+i+"_count" ) ) {
						int killed_types = tags.GetInt( "world_kills_" + i + "_count" );

						this.WorldKills[world_uid] = new Dictionary<int, int>();

						for( int j=0; j<killed_types; j++ ) {
							int npc_type = tags.GetInt( "world_kills_" + i + "_type_" + j );
							this.WorldKills[ world_uid ][ npc_type ] = tags.GetInt( "world_kills_" + i + "_type_" + j + "_killed" );
						}
					}
					if( tags.ContainsKey( "world_goblins_" + i ) ) {
						this.WorldGolinsConquered[ world_uid ] = tags.GetInt( "world_goblins_" + i );
					}
					if( tags.ContainsKey( "world_frostlegion_" + i ) ) {
						this.WorldFrostLegionConquered[ world_uid ] = tags.GetInt( "world_frostlegion_" + i );
					}
					if( tags.ContainsKey( "world_pirates_" + i ) ) {
						this.WorldPiratesConquered[ world_uid ] = tags.GetInt( "world_pirates_" + i );
					}
					if( tags.ContainsKey( "world_martians_" + i ) ) {
						this.WorldMartiansConquered[ world_uid ] = tags.GetInt( "world_martians_" + i );
					}
					if( tags.ContainsKey( "world_pumpkinmoon_waves_" + i ) ) {
						this.WorldPumpkinMoonWavesConquered[ world_uid ] = tags.GetInt( "world_pumpkinmoon_waves_" + i );
					}
					if( tags.ContainsKey( "world_frostmoon_waves_" + i ) ) {
						this.WorldFrostMoonWavesConquered[ world_uid ] = tags.GetInt( "world_frostmoon_waves_" + i );
					}
				}
			}
		}

		public TagCompound Save() {
			var tags = new TagCompound { { "world_uid_count", this.WorldPoints.Count } };
			string curr_world_uid = WorldHelpers.GetUniqueId();

			if( !string.IsNullOrEmpty(curr_world_uid) ) {
				this.WorldPoints[curr_world_uid] = this.ProgressPoints;
			}

			int i = 0;
			foreach( var kv in this.WorldPoints ) {
				string world_uid = kv.Key;
				float points = kv.Value;

				tags.Set( "world_uid_" + i, world_uid );
				tags.Set( "world_pp_" + i, points );

				tags.Set( "world_goblins_" + i, this.WorldGolinsConquered[ world_uid ] );
				tags.Set( "world_frostlegion_" + i, this.WorldFrostLegionConquered[ world_uid ] );
				tags.Set( "world_pirates_" + i, this.WorldPiratesConquered[ world_uid ] );
				tags.Set( "world_martians_" + i, this.WorldMartiansConquered[ world_uid ] );
				tags.Set( "world_pumpkinmoon_waves_" + i, this.WorldPumpkinMoonWavesConquered[ world_uid ] );
				tags.Set( "world_frostmoon_waves_" + i, this.WorldFrostMoonWavesConquered[ world_uid ] );

				tags.Set( "world_kills_" + i + "_count", this.WorldKills.Count );

				int j = 0;
				foreach( var kv2 in this.WorldKills[ curr_world_uid ] ) {
					int npc_type = kv2.Key;
					int killed = kv2.Value;

					tags.Set( "world_kills_" + i + "_type_" + j, npc_type );
					tags.Set( "world_kills_" + i + "_type_" + j + "_killed", killed );
					j++;
				}
				i++;
			}

			return tags;
		}

		public void OnEnterWorld() {
			string curr_world_uid = WorldHelpers.GetUniqueId();

			if( !this.WorldKills.ContainsKey( curr_world_uid ) ) {
				this.WorldKills[ curr_world_uid ] = new Dictionary<int, int>();
				this.WorldGolinsConquered[ curr_world_uid ] = 0;
				this.WorldFrostLegionConquered[ curr_world_uid ] = 0;
				this.WorldPiratesConquered[ curr_world_uid ] = 0;
				this.WorldMartiansConquered[ curr_world_uid ] = 0;
				this.WorldPumpkinMoonWavesConquered[ curr_world_uid ] = 0;
				this.WorldFrostMoonWavesConquered[ curr_world_uid ] = 0;
			}
		}


		////////////////

		public void UpdateInvasions() {
			VanillaInvasionType now_inv = NPCInvasionHelpers.GetCurrentInvasionType();

			switch( this.CurrentInvasion ) {
			case VanillaInvasionType.Goblins:
				if( Main.invasionType != (int)this.CurrentInvasion ) {
					this.CurrentInvasion = now_inv;
					this.WorldGolinsConquered[WorldHelpers.GetUniqueId()]++;
				}
				break;
			case VanillaInvasionType.FrostLegion:
				if( Main.invasionType != (int)this.CurrentInvasion ) {
					this.CurrentInvasion = now_inv;
					this.WorldFrostLegionConquered[WorldHelpers.GetUniqueId()]++;
				}
				break;
			case VanillaInvasionType.Pirates:
				if( Main.invasionType != (int)this.CurrentInvasion ) {
					this.CurrentInvasion = now_inv;
					this.WorldPiratesConquered[WorldHelpers.GetUniqueId()]++;
				}
				break;
			case VanillaInvasionType.Martians:
				if( Main.invasionType != (int)this.CurrentInvasion ) {
					this.CurrentInvasion = now_inv;
					this.WorldMartiansConquered[WorldHelpers.GetUniqueId()]++;
				}
				break;
			case VanillaInvasionType.PumpkinMoon:
				if( Main.pumpkinMoon ) {
					var world_uid = WorldHelpers.GetUniqueId();

					if( NPC.waveNumber > this.WorldPumpkinMoonWavesConquered[world_uid] ) {
						this.WorldPumpkinMoonWavesConquered[world_uid] = NPC.waveNumber;
					}
				} else {
					this.CurrentInvasion = now_inv;
				}
				break;
			case VanillaInvasionType.FrostMoon:
				if( Main.snowMoon ) {
					var world_uid = WorldHelpers.GetUniqueId();

					if( NPC.waveNumber > this.WorldFrostMoonWavesConquered[world_uid] ) {
						this.WorldFrostMoonWavesConquered[world_uid] = NPC.waveNumber;
					}
				} else {
					this.CurrentInvasion = now_inv;
				}
				break;
			default:
				this.CurrentInvasion = now_inv;
				break;
			}
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
			if( this.KilledNpcs.ContainsKey( npc.type ) ) {
				this.KilledNpcs[ npc.type ]++;
			} else {
				this.KilledNpcs[ npc.type ] = 1;
			}
			
			float reward = this.CalculateKillReward( mymod, npc );

			this.ProgressPoints += reward;
		}


		////////////////
		
		private float CalculateKillReward( RewardsMod mymod, NPC npc ) {
			string world_uid = WorldHelpers.GetUniqueId();
			float points = 0;
			bool grind = false;

			if( this.CurrentInvasion != VanillaInvasionType.None ) {
				if( NPCIdentityHelpers.VanillaGoblinArmyTypes.Contains( npc.type ) ) {
					points = mymod.Config.GoblinInvasionReward / Main.invasionSizeStart;
					grind = this.WorldGolinsConquered[world_uid] > 0;
				} else if( NPCIdentityHelpers.VanillaFrostLegionTypes.Contains( npc.type ) ) {
					points = mymod.Config.FrostLegionInvasionReward / Main.invasionSizeStart;
					grind = this.WorldFrostLegionConquered[world_uid] > 0;
				} else if( NPCIdentityHelpers.VanillaPirateTypes.Contains( npc.type ) ) {
					points = mymod.Config.PirateInvasionReward / Main.invasionSizeStart;
					grind = this.WorldPiratesConquered[world_uid] > 0;
				} else if( NPCIdentityHelpers.VanillaMartianTypes.Contains( npc.type ) ) {
					points = mymod.Config.MartianInvasionReward / Main.invasionSizeStart;
					grind = this.WorldMartiansConquered[world_uid] > 0;
				} else if( NPCIdentityHelpers.VanillaPumpkingMoonTypes.Contains( npc.type ) ) {
					points = mymod.Config.PumpkingMoonWaveReward / Main.invasionSizeStart;
					grind = NPC.waveNumber < this.WorldPumpkinMoonWavesConquered[world_uid];
				} else if( NPCIdentityHelpers.VanillaFrostMoonTypes.Contains( npc.type ) ) {
					points = mymod.Config.FrostMoonWaveReward / Main.invasionSizeStart;
					grind = NPC.waveNumber < this.WorldFrostMoonWavesConquered[world_uid];
				}
			}

			if( points == 0 ) {
				if( mymod.Config.NpcRewards.ContainsKey( npc.TypeName ) ) {
					points = mymod.Config.NpcRewards[ npc.TypeName ];
				}
			}

			if( this.KilledNpcs.ContainsKey( npc.type ) ) {
				if( mymod.Config.NpcRewardRequiredMinimumKills.ContainsKey( npc.TypeName ) ) {
					grind = this.KilledNpcs[npc.type] > mymod.Config.NpcRewardRequiredMinimumKills[ npc.TypeName ];
				} else if( this.KilledNpcs[ npc.type ] > 1 ) {
					grind = true;
				}
			}

			if( grind ) {
				points *= mymod.Config.GrindKillMultiplier;
			}

			return points;
		}
	}
}
