using Microsoft.Xna.Framework;
using Rewards.NetProtocol;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.Commands {
	class CheatToggleCommand : ModCommand {
		public override CommandType Type { get {
				if( Main.netMode == 0 ) {
					return CommandType.Chat;
				}
				return CommandType.Console;
			}
		}
		public override string Command { get { return "rewardscheatstoggle"; } }
		public override string Usage { get { return "/rewardscheatstoggle"; } }
		public override string Description { get { return "Toggles cheat mode on or off. Applies for all players."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (RewardsMod)this.mod;

			if( mymod.Config.DebugModeEnableCheats ) {
				mymod.Config.DebugModeEnableCheats = false;
				caller.Reply( "Cheat mode off.", Color.YellowGreen );
			} else {
				mymod.Config.DebugModeEnableCheats = true;
				caller.Reply( "Cheat mode on.", Color.YellowGreen );
			}

			if( Main.netMode == 2 ) {
				ServerPacketHandlers.SendModSettingsFromServer( mymod, -1 );
			}
		}
	}
}
