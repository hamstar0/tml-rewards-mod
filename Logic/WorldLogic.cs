using HamstarHelpers.DebugHelpers;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.WorldHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader.IO;


namespace Rewards.Logic {
	partial class WorldLogic {
		internal IDictionary<int, int> KilledNpcs = new Dictionary<int, int>();
		internal int GoblinsConquered = 0;
		internal int FrostLegionConquered = 0;
		internal int PiratesConquered = 0;
		internal int MartiansConquered = 0;
		internal int PumpkinMoonWavesConquered = 0;
		internal int FrostMoonWavesConquered = 0;

		private VanillaInvasionType CurrentInvasion = VanillaInvasionType.None;



		////////////////

		public WorldLogic() {
			this.CurrentInvasion = NPCInvasionHelpers.GetCurrentInvasionType();
			this.KilledNpcs = new Dictionary<int, int>();
		}


		public void Load( RewardsMod mymod, TagCompound tags ) {
			string curr_world_uid = WorldHelpers.GetUniqueId();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "World Load - current world id: " + curr_world_uid );
			}

			if( tags.ContainsKey( "kills_count" ) ) {
				int killed_types = tags.GetInt( "kills_count" );

				for( int i = 0; i < killed_types; i++ ) {
					int npc_type = tags.GetInt( "kills_type_" + i );
					int killed = tags.GetInt( "kills_type_" + i + "_killed" );

					this.KilledNpcs[npc_type] = killed;

					if( mymod.Config.DebugModeInfo ) {
						LogHelpers.Log( "World Load - Npc kill of: " + npc_type + " (" + i + "), total: " + killed );
					}
				}
			}
			if( tags.ContainsKey( "goblins" ) ) {
				this.GoblinsConquered = tags.GetInt( "goblins" );
			}
			if( tags.ContainsKey( "frostlegion" ) ) {
				this.FrostLegionConquered = tags.GetInt( "frostlegion" );
			}
			if( tags.ContainsKey( "pirates" ) ) {
				this.PiratesConquered = tags.GetInt( "pirates" );
			}
			if( tags.ContainsKey( "martians" ) ) {
				this.MartiansConquered = tags.GetInt( "martians" );
			}
			if( tags.ContainsKey( "pumpkinmoon_waves" ) ) {
				this.PumpkinMoonWavesConquered = tags.GetInt( "pumpkinmoon_waves" );
			}
			if( tags.ContainsKey( "frostmoon_waves" ) ) {
				this.FrostMoonWavesConquered = tags.GetInt( "frostmoon_waves" );
			}
		}


		public TagCompound Save( RewardsMod mymod ) {
			string curr_world_uid = WorldHelpers.GetUniqueId();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "Player Save - current world id: " + curr_world_uid );
			}

			var tags = new TagCompound();

			tags.Set( "goblins", this.GoblinsConquered );
			tags.Set( "frostlegion", this.FrostLegionConquered );
			tags.Set( "pirates", this.PiratesConquered );
			tags.Set( "martians", this.MartiansConquered );
			tags.Set( "pumpkinmoon_waves", this.PumpkinMoonWavesConquered );
			tags.Set( "frostmoon_waves", this.FrostMoonWavesConquered );

			tags.Set( "kills_count", this.KilledNpcs.Count );

			int i = 0;
			foreach( var kv in this.KilledNpcs ) {
				int npc_type = kv.Key;
				int killed = kv.Value;

				tags.Set( "kills_type_" + i, npc_type );
				tags.Set( "kills_type_" + i + "_killed", killed );

				if( mymod.Config.DebugModeInfo ) {
					LogHelpers.Log( "World Save - Npc kill of: " + npc_type + " (" + i + "), total: " + killed );
				}
				i++;
			}

			return tags;
		}

		////////////////

		public void NetSend( BinaryWriter writer ) {
			writer.Write( (int)this.KilledNpcs.Count );

			foreach( var kv in this.KilledNpcs ) {
				writer.Write( (int)kv.Key );
				writer.Write( (int)kv.Value );
			}
		}

		public void NetReceive( BinaryReader reader ) {
			int kill_count = reader.ReadInt32();

			this.KilledNpcs = new Dictionary<int, int>();

			for( int i = 0; i < kill_count; i++ ) {
				int npc_type = reader.ReadInt32();
				int kills = reader.ReadInt32();

				this.KilledNpcs[npc_type] = kills;
			}
		}
	}
}
