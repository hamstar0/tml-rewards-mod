using Microsoft.Xna.Framework;
using Rewards.Items;
using Terraria.ModLoader;


namespace Rewards.Commands {
	class ShopRemoveLastCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "rewardsshopremovelast"; } }
		public override string Usage { get { return "/rewardsshopremovelast"; } }
		public override string Description { get { return "Removes the last listed item in the Wayfarer's shop."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (RewardsMod)this.mod;
			if( !mymod.Config.DebugModeEnableCheats ) {
				throw new UsageException( "Cheat mode not enabled." );
			}

			ShopPackDefinition? def = RewardsAPI.ShopRemoveLastPack();
			
			if( def == null ) {
				caller.Reply( "No shop packs left to remove.", Color.Yellow );
			} else {
				mymod.JsonConfig.SaveFile();
				caller.Reply( "Removed shop pack "+((ShopPackDefinition)def).Name, Color.LimeGreen );
			}
		}
	}
}
