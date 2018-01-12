using System;
using Terraria;


namespace Rewards {
	public static class RewardsAPI {
		public static RewardsConfigData GetModSettings() {
			return RewardsMod.Instance.Config;
		}
	}
}
