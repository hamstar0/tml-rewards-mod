﻿using HamstarHelpers.Helpers.TModLoader.Configs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace Rewards.Configs {
	public partial class RewardsPointsConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[DefaultValue( 0.1f )]
		public float GrindKillMultiplier = 0.1f;


		[DefaultValue( 15f )]
		public float GoblinInvasionReward = 15f;

		[DefaultValue( 15f )]
		public float FrostLegionInvasionReward = 15f;

		[DefaultValue( 25f )]
		public float PirateInvasionReward = 25f;

		[DefaultValue( 200f )]
		public float MartianInvasionReward = 200f;

		[DefaultValue( 10f )]
		public float PumpkingMoonWaveReward = 10f;

		[DefaultValue( 10f )]
		public float FrostMoonWaveReward = 10f;


		public IDictionary<NPCDefinition, float> NpcRewards = new Dictionary<NPCDefinition, float>();

		public IDictionary<NPCDefinition, int> NpcRewardTogetherSets = new Dictionary<NPCDefinition, int>();

		public ISet<NPCDefinition> NpcRewardRequiredAsBoss = new HashSet<NPCDefinition>();

		public IDictionary<NPCDefinition, NPCDefinition> NpcRewardNotGivenAfterNpcKilled = new Dictionary<NPCDefinition, NPCDefinition>();

		//public bool NpcRewardPrediction = true;


		////////////////

		public void Reset() {
			JsonConvert.PopulateObject( "{}", this, ConfigManager.serializerSettings );
			this.NpcRewards = new Dictionary<NPCDefinition, float>();
			this.NpcRewardTogetherSets = new Dictionary<NPCDefinition, int>();
			this.NpcRewardRequiredAsBoss = new HashSet<NPCDefinition>();
			this.NpcRewardNotGivenAfterNpcKilled = new Dictionary<NPCDefinition, NPCDefinition>();

			ConfigHelpers.SyncConfig( this );
		}
	}
}
