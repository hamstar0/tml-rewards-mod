using Microsoft.Xna.Framework;
using ModLibsGeneral.Libraries.TModLoader.Configs;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.Commands {
	class CheatToggleCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console;
			}
		}
		public override string Command => "rew-cheats-toggle";
		public override string Usage => "/"+this.Command;
		public override string Description => "Toggles cheat mode on or off. Applies for all players.";



		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (RewardsMod)this.Mod;

			if( mymod.SettingsConfig.DebugModeEnableCheats ) {
				mymod.SettingsConfig.DebugModeEnableCheats = false;
				caller.Reply( "Cheat mode off.", Color.LimeGreen );
			} else {
				mymod.SettingsConfig.DebugModeEnableCheats = true;
				caller.Reply( "Cheat mode on.", Color.LimeGreen );
			}

			if( Main.netMode != 0 ) {
				ConfigLibraries.SyncConfig( mymod.SettingsConfig );
			}
		}
	}
}
