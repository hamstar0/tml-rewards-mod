using HamstarHelpers.Components.Errors;
using Microsoft.Xna.Framework;
using Rewards.Logic;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.Commands {
	class AddPointsCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "rewardsaddpoints"; } }
		public override string Usage { get { return "/rewardsaddpoints 100"; } }
		public override string Description { get { return "Adds the specified amount of progress points."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (RewardsMod)this.mod;
			if( !mymod.Config.DebugModeEnableCheats ) {
				throw new UsageException( "Cheat mode not enabled." );
			}

			if( args.Length < 1 ) {
				throw new UsageException( "Insufficient arguments." );
			}

			float reward;

			if( !float.TryParse( args[0], out reward ) ) {
				throw new UsageException( "Invalid numeric parameter." );
			}
			
			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			if( data == null ) {
				throw new HamstarException( "Rewards - AddPointsCommand.Action() - No player data for " + Main.LocalPlayer.name );
			}

			data.AddRewardForPlayer( mymod, Main.LocalPlayer, false, false, reward );

			caller.Reply( "+"+reward+" PP added. Cheater!", Color.LimeGreen );
		}
	}
}
