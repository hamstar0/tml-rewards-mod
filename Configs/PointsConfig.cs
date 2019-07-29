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


		public IDictionary<string, float> NpcRewards = new Dictionary<string, float>();

		public IDictionary<string, int> NpcRewardTogetherSets = new Dictionary<string, int>();

		public ISet<string> NpcRewardRequiredAsBoss = new HashSet<string>();

		public IDictionary<string, string> NpcRewardNotGivenAfterNpcKilled = new Dictionary<string, string>();

		//public bool NpcRewardPrediction = true;


		////////////////

		public void Reset() {
			d
		}
	}
}
