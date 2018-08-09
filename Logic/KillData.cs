using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		private static string DataFileFolder = "Rewards";



		////////////////
		
		public IDictionary<int, int> KilledNpcs = new ConcurrentDictionary<int, int>();
		public int GoblinsConquered = 0;
		public int FrostLegionConquered = 0;
		public int PiratesConquered = 0;
		public int MartiansConquered = 0;
		public int PumpkinMoonWavesConquered = 0;
		public int FrostMoonWavesConquered = 0;

		public float ProgressPoints = 0f;

		internal IDictionary<VanillaInvasionType, byte> CurrentEvents = new ConcurrentDictionary<VanillaInvasionType, byte>();



		////////////////

		public KillData() {
			this.CurrentEvents = new ConcurrentDictionary<VanillaInvasionType, byte>(
				NPCInvasionHelpers.GetCurrentEventTypes().Select( t => new KeyValuePair<VanillaInvasionType, byte>(t, 0) )
			);
		}

		public void ClearKills() {
			this.KilledNpcs = new Dictionary<int, int>();
			this.GoblinsConquered = 0;
			this.FrostLegionConquered = 0;
			this.PiratesConquered = 0;
			this.MartiansConquered = 0;
			this.PumpkinMoonWavesConquered = 0;
			this.FrostMoonWavesConquered = 0;
		}

		public void ResetAll() {
			this.ClearKills();
			this.ProgressPoints = 0;
		}

		public void AddToMe( RewardsMod mymod, KillData data ) {
			foreach( var kv in data.KilledNpcs ) {
				if( this.KilledNpcs.ContainsKey( kv.Key ) ) {
					this.KilledNpcs[kv.Key] += kv.Value;
				} else {
					this.KilledNpcs[kv.Key] = kv.Value;
				}
			}
			this.GoblinsConquered += data.GoblinsConquered;
			this.FrostLegionConquered += data.FrostLegionConquered;
			this.PiratesConquered += data.PiratesConquered;
			this.MartiansConquered += data.MartiansConquered;
			this.PumpkinMoonWavesConquered += data.PumpkinMoonWavesConquered;
			this.FrostMoonWavesConquered += data.FrostMoonWavesConquered;
			this.ProgressPoints += data.ProgressPoints;
		}


		////////////////

		public bool Load( RewardsMod mymod, string base_file_name ) {
			KillData data;
			bool success = false;

			try {
				if( mymod.Config.DebugModeSaveKillsAsJson ) {
					data = DataFileHelpers.LoadJson<KillData>( mymod, base_file_name, out success );
				} else {
					data = DataFileHelpers.LoadBinary<KillData>( mymod, base_file_name, false );
					success = data != null;
				}

				if( success ) {
					this.KilledNpcs = data.KilledNpcs;
					this.GoblinsConquered = data.GoblinsConquered;
					this.FrostLegionConquered = data.FrostLegionConquered;
					this.PiratesConquered = data.PiratesConquered;
					this.MartiansConquered = data.MartiansConquered;
					this.PumpkinMoonWavesConquered = data.PumpkinMoonWavesConquered;
					this.FrostMoonWavesConquered = data.FrostMoonWavesConquered;
					this.ProgressPoints = data.ProgressPoints;
				}
			} catch( IOException e ) {
				throw new IOException( "Failed to load file: "+ base_file_name, e );
			}

			return success;
		}

		public void Save( RewardsMod mymod, string base_file_name ) {
			try {
				if( mymod.Config.DebugModeSaveKillsAsJson ) {
					DataFileHelpers.SaveAsJson<KillData>( mymod, base_file_name, this );
				} else {
					DataFileHelpers.SaveAsBinary<KillData>( mymod, base_file_name, false, this );
				}
			} catch( IOException e ) {
				throw new IOException( "Failed to save file: "+ base_file_name, e );
			}
		}


		////////////////

		public override string ToString() {
			return JsonConfig<KillData>.Serialize( this ).Replace( "\r\n  ", "" ).Replace( "\r\n", "" );
		}
	}
}
