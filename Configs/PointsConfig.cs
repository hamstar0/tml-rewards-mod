﻿using HamstarHelpers.Classes.UI.ModConfig;
using HamstarHelpers.Helpers.TModLoader.Configs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Terraria.ModLoader.Config;


namespace Rewards.Configs {
	public class NPCKillRewardValue {
		[Range( 0, 99999 )]
		public int PP;


		public NPCKillRewardValue() { }
		public NPCKillRewardValue( int pp ) {
			this.PP = pp;
		}
	}




	public partial class RewardsPointsConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////
		
		[Label("PP multiplier for subsequent kills (anti-grinding measure)")]
		[Range(0f, 100f)]
		[DefaultValue( 0.1f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float GrindKillMultiplier = 0.1f;


		[Label("PP awarded from a goblin invasion")]
		[Range( 0f, 1000f )]
		[DefaultValue( 15f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float GoblinInvasionReward = 15f;

		[Label( "PP awarded from a frost legion invasion" )]
		[Range( 0f, 1000f )]
		[DefaultValue( 15f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float FrostLegionInvasionReward = 15f;

		[Label( "PP awarded from a pirate invasion" )]
		[Range( 0f, 1000f )]
		[DefaultValue( 25f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float PirateInvasionReward = 25f;

		[Label( "PP awarded from a martian invasion" )]
		[Range( 0f, 1000f )]
		[DefaultValue( 200f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float MartianInvasionReward = 200f;

		[Label( "PP awarded from a given wave of the pumpkin moon" )]
		[Range( 0f, 1000f )]
		[DefaultValue( 10f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float PumpkingMoonWaveReward = 10f;

		[Label( "PP awarded from a given wave of the frost moon" )]
		[Range( 0f, 1000f )]
		[DefaultValue( 10f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float FrostMoonWaveReward = 10f;


		[Label( "PP awarded from kills of the given NPCs" )]
		public Dictionary<NPCDefinition, NPCKillRewardValue> NpcRewardsOnKill = new Dictionary<NPCDefinition, NPCKillRewardValue>();

		[Label( "NPCs required as bosses (npc.boss=true) to get PP awards" )]
		public HashSet<NPCDefinition> NpcRewardRequiredAsBoss = new HashSet<NPCDefinition>();

		[Label( "NPC kill that blocks another NPC kill from awarding PP" )]
		public Dictionary<NPCDefinition, NPCDefinition> NpcRewardNotGivenAfterNpcKilled = new Dictionary<NPCDefinition, NPCDefinition>();

		//public bool NpcRewardPrediction = true;



		[Header("OLD SETTINGS; DO NOT USE")]
		[Label( "(Obsolete) PP awarded from kills of the given NPCs" )]
		public Dictionary<NPCDefinition, float> NpcRewards = new Dictionary<NPCDefinition, float>();



		////////////////

		public override ModConfig Clone() {
			var clone = (RewardsPointsConfig)base.Clone();

			clone.NpcRewardsOnKill = this.NpcRewardsOnKill.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.NpcRewardRequiredAsBoss = new HashSet<NPCDefinition>( this.NpcRewardRequiredAsBoss );
			clone.NpcRewardNotGivenAfterNpcKilled = this.NpcRewardNotGivenAfterNpcKilled
				.ToDictionary( kv => kv.Key, kv => kv.Value );

			return clone;
		}


		////////////////

		public void Reset() {
			JsonConvert.PopulateObject( "{}", this, ConfigManager.serializerSettings );
			this.NpcRewardsOnKill = new Dictionary<NPCDefinition, NPCKillRewardValue>();
			this.NpcRewardRequiredAsBoss = new HashSet<NPCDefinition>();
			this.NpcRewardNotGivenAfterNpcKilled = new Dictionary<NPCDefinition, NPCDefinition>();

			ConfigHelpers.SyncConfig( this );
		}
	}
}
