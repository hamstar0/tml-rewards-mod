using Microsoft.Xna.Framework;
using Terraria.ModLoader;


namespace Rewards.Commands {
	class ShopClearCommand : ModCommand {
		public override CommandType Type => CommandType.Chat;
		public override string Command => "rew-shop-clear";
		public override string Usage => "/" + this.Command;
		public override string Description => "Clears the Wayfarer's shop of items.";



		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (RewardsMod)this.mod;
			if( !mymod.SettingsConfig.DebugModeEnableCheats ) {
				throw new UsageException( "Cheat mode not enabled." );
			}

			RewardsAPI.ShopClear();
			RewardsAPI.SaveModSettingsChanges();

			caller.Reply( "Wayfarer shop reset.", Color.LimeGreen );
		}
	}
}
