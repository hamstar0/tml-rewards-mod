using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Misc;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		public IDictionary<int, int> KilledNpcs = new ConcurrentDictionary<int, int>();
		public int GoblinsConquered = 0;
		public int FrostLegionConquered = 0;
		public int PiratesConquered = 0;
		public int MartiansConquered = 0;
		public int PumpkinMoonWavesConquered = 0;
		public int FrostMoonWavesConquered = 0;

		public float ProgressPoints = 0f;



		////////////////

		public KillData() {
		}

		////

		public void ClearKills() {
			this.KilledNpcs = new Dictionary<int, int>();
			this.GoblinsConquered = 0;
			this.FrostLegionConquered = 0;
			this.PiratesConquered = 0;
			this.MartiansConquered = 0;
			this.PumpkinMoonWavesConquered = 0;
			this.FrostMoonWavesConquered = 0;
		}

		public void ResetAll( Player forPlayer=null ) {
			var mymod = RewardsMod.Instance;

			this.ClearKills();
			this.ProgressPoints = 0;

			if( mymod.SettingsConfig.DebugModePPInfo ) {
				LogHelpers.Alert( "PP reset (for " + ( forPlayer?.name ?? "world" ) + ")" );
			}
		}

		////

		public void AddToMe( KillData data, Player forPlayer=null ) {
			var mymod = RewardsMod.Instance;

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

			if( mymod.SettingsConfig.DebugModePPInfo && data.ProgressPoints != 0 ) {
				LogHelpers.Alert( "PP added: "+data.ProgressPoints + " (now "+this.ProgressPoints
					+", for " + ( forPlayer?.name ?? "world?" ) + ")" );
			}
		}


		////////////////

		public bool Load( string baseFileName, Player forPlayer=null ) {
			var mymod = RewardsMod.Instance;
			KillData data;
			bool success = false;

			try {
				if( mymod.SettingsConfig.DebugModeSaveKillsAsJson ) {
					data = ModCustomDataFileHelpers.LoadJson<KillData>( mymod, baseFileName );
				} else {
					data = ModCustomDataFileHelpers.LoadBinaryJson<KillData>( mymod, baseFileName );
					success = data != null;
				}
			} catch( IOException e ) {
				throw new ModHelpersException( "Failed to load file: "+ baseFileName, e );
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
				
				if( mymod.SettingsConfig.DebugModePPInfo ) {
					LogHelpers.Alert( "PP set: "+this.ProgressPoints+" (for "+(forPlayer?.name??"world")+")" );
				}
			}

			return success;
		}

		public void Save( string baseFileName ) {
			var mymod = RewardsMod.Instance;

			try {
				if( mymod.SettingsConfig.DebugModeSaveKillsAsJson ) {
					ModCustomDataFileHelpers.SaveAsJson( mymod, baseFileName, true, this ); // false?
				} else {
					ModCustomDataFileHelpers.SaveAsBinaryJson( mymod, baseFileName, true, this );	// false?
				}
			} catch( IOException e ) {
				throw new ModHelpersException( "Failed to save file: "+ baseFileName, e );
			}
		}


		////////////////

		public override string ToString() {
			return JsonConvert.SerializeObject( this ).Replace( "\r\n  ", "" ).Replace( "\r\n", "" );
		}
	}
}
