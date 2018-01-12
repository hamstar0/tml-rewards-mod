using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.Commands {
	class CheatRewardsCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "rewardscheat"; } }
		public override string Usage { get { return "/rewardscheat"; } }
		public override string Description { get { return "Adds +100 progress points."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (RewardsMod)this.mod;
			if( !mymod.Config.DebugModeEnableCheats ) {
				throw new UsageException( "Cheat mode not enabled.", Color.Red );
			}
			
			var myplayer = Main.LocalPlayer.GetModPlayer<RewardsPlayer>();
			myplayer.Logic.ProgressPoints += 100;

			caller.Reply( "Cheater!", Color.YellowGreen );
		}
	}
}
