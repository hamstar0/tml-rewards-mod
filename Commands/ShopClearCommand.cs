using Microsoft.Xna.Framework;
using Terraria.ModLoader;


namespace Rewards.Commands {
	class ShopClearCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "rew-shop-clear"; } }
		public override string Usage { get { return "/" + this.Command; } }
		public override string Description { get { return "Clears the Wayfarer's shop of items."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (RewardsMod)this.mod;
			if( !mymod.Config.DebugModeEnableCheats ) {
				throw new UsageException( "Cheat mode not enabled." );
			}

			RewardsAPI.ShopClear();
			RewardsAPI.SaveModSettingsChanges();

			caller.Reply( "Wayfarer shop reset.", Color.LimeGreen );
		}
	}
}
