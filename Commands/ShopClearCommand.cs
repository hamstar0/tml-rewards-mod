using Terraria.ModLoader;


namespace Rewards.Commands {
	class ShopClearCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "rewardsshopclear"; } }
		public override string Usage { get { return "/rewardsshopclear"; } }
		public override string Description { get { return "Clears the Wayfarer's shop of items."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (RewardsMod)this.mod;
			if( !mymod.Config.DebugModeEnableCheats ) {
				throw new UsageException( "Cheat mode not enabled." );
			}

			RewardsAPI.ShopClear();
		}
	}
}
