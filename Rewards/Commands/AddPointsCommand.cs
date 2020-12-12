using HamstarHelpers.Classes.Errors;
using Microsoft.Xna.Framework;
using Rewards.Logic;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.Commands {
	class AddPointsCommand : ModCommand {
		public override CommandType Type => CommandType.Chat;
		public override string Command => "rew-add-points";
		public override string Usage => "/" +this.Command+" 100";
		public override string Description => "Adds the specified amount of progress points.";



		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (RewardsMod)this.mod;
			if( !mymod.SettingsConfig.DebugModeEnableCheats ) {
				caller.Reply( "Cheat mode not enabled.", Color.Yellow );
				return;
			}

			if( args.Length < 1 ) {
				caller.Reply( "Insufficient arguments.", Color.Red );
				return;
			}

			float reward;

			if( !float.TryParse( args[0], out reward ) ) {
				caller.Reply( "Invalid numeric parameter.", Color.Red );
				return;
			}
			
			var myworld = ModContent.GetInstance<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			if( data == null ) {
				throw new ModHelpersException( "Rewards - AddPointsCommand.Action() - No player data for " + Main.LocalPlayer.name );
			}

			data.AddRewardForPlayer( Main.LocalPlayer, false, false, reward );

			caller.Reply( "+"+reward+" PP added. Cheater!", Color.LimeGreen );
		}
	}
}
