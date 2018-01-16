using HamstarHelpers.DebugHelpers;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.WorldHelpers;
using System.Collections.Generic;
using Terraria.ModLoader.IO;


namespace Rewards.Logic {
	partial class RewardsLogic {
		internal IDictionary<string, IDictionary<int, int>> WorldKills = new Dictionary<string, IDictionary<int, int>>();
		internal IDictionary<string, float> WorldPoints = new Dictionary<string, float>();
		internal IDictionary<string, int> WorldGolinsConquered = new Dictionary<string, int>();
		internal IDictionary<string, int> WorldFrostLegionConquered = new Dictionary<string, int>();
		internal IDictionary<string, int> WorldPiratesConquered = new Dictionary<string, int>();
		internal IDictionary<string, int> WorldMartiansConquered = new Dictionary<string, int>();
		internal IDictionary<string, int> WorldPumpkinMoonWavesConquered = new Dictionary<string, int>();
		internal IDictionary<string, int> WorldFrostMoonWavesConquered = new Dictionary<string, int>();

		private IDictionary<int, int> KilledNpcs { get {
			return this.WorldKills[ WorldHelpers.GetUniqueId() ];
		} }
		
		public float ProgressPoints {
			get {
				string world_uid = WorldHelpers.GetUniqueId();
				if( this.WorldPoints.ContainsKey( world_uid ) ) {
					return this.WorldPoints[WorldHelpers.GetUniqueId()];
				}
				return 0;
			}
			set {
				string world_uid = WorldHelpers.GetUniqueId();
				this.WorldPoints[world_uid] = value;
			}
		}

		private VanillaInvasionType CurrentInvasion = VanillaInvasionType.None;



		////////////////

		public RewardsLogic() {
			this.CurrentInvasion = NPCInvasionHelpers.GetCurrentInvasionType();
			this.Initialize();
		}


		public void Initialize() {
			string curr_world_uid = WorldHelpers.GetUniqueId();

			this.WorldKills[curr_world_uid] = new Dictionary<int, int>();
			this.WorldPoints[curr_world_uid] = 0;
			this.WorldGolinsConquered[curr_world_uid] = 0;
			this.WorldFrostLegionConquered[curr_world_uid] = 0;
			this.WorldPiratesConquered[curr_world_uid] = 0;
			this.WorldMartiansConquered[curr_world_uid] = 0;
			this.WorldPumpkinMoonWavesConquered[curr_world_uid] = 0;
			this.WorldFrostMoonWavesConquered[curr_world_uid] = 0;
		}


		public void Load( RewardsMod mymod, TagCompound tags ) {
			string curr_world_uid = WorldHelpers.GetUniqueId();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "Player Load - current world id: " + curr_world_uid );
			}

			if( !this.WorldKills.ContainsKey( curr_world_uid ) ) {
				this.Initialize();
			}

			if( tags.ContainsKey( "world_uid_count" ) ) {
				int world_count = tags.GetInt( "world_uid_count" );

				for( int i=0; i<world_count; i++ ) {
					string world_uid = tags.GetString( "world_uid_" + i );
					float pp = tags.GetFloat( "world_pp_"+i );

					this.WorldPoints[ world_uid ] = pp;

					if( tags.ContainsKey( "world_kills_"+i+"_count" ) ) {
						int killed_types = tags.GetInt( "world_kills_" + i + "_count" );

						this.WorldKills[world_uid] = new Dictionary<int, int>();

						for( int j=0; j<killed_types; j++ ) {
							int npc_type = tags.GetInt( "world_kills_" + i + "_type_" + j );
							int killed = tags.GetInt( "world_kills_" + i + "_type_" + j + "_killed" );

							this.WorldKills[ world_uid ][ npc_type ] = killed;

							if( mymod.Config.DebugModeInfo ) {
								LogHelpers.Log( "Player Load - world id: " + world_uid + " (" + i + "), npc kill of: " + npc_type + " (" + j + "), total: " + killed );
							}
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

		public TagCompound Save( RewardsMod mymod) {
			string curr_world_uid = WorldHelpers.GetUniqueId();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "Player Save - current world id: " + curr_world_uid );
			}

			if( !string.IsNullOrEmpty( curr_world_uid ) ) {
				if( !this.WorldKills.ContainsKey( curr_world_uid ) ) {
					this.Initialize();
				}
			}

			var tags = new TagCompound { { "world_uid_count", this.WorldPoints.Count } };

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
				foreach( var kv2 in this.WorldKills[world_uid] ) {
					int npc_type = kv2.Key;
					int killed = kv2.Value;

					tags.Set( "world_kills_" + i + "_type_" + j, npc_type );
					tags.Set( "world_kills_" + i + "_type_" + j + "_killed", killed );

					if( mymod.Config.DebugModeInfo ) {
						LogHelpers.Log( "Player Save - world id: " + world_uid + " (" + i + "), npc kill of: " + npc_type + " (" + j + "), total: " + killed );
					}
					j++;
				}
				i++;
			}

			return tags;
		}


		public void OnEnterWorld() {
			string curr_world_uid = WorldHelpers.GetUniqueId();

			if( !this.WorldKills.ContainsKey( curr_world_uid ) ) {
				this.Initialize();
			}
		}
	}
}
