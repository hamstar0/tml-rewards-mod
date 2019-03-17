using Microsoft.Xna.Framework;
using Rewards.Items;
using Terraria.ModLoader;


namespace Rewards.Commands {
	class ShopRemoveLastCommand : ModCommand {
		public override CommandType Type => CommandType.Chat;
		public override string Command => "rew-shop-remove-last";
		public override string Usage => "/" + this.Command;
		public override string Description => "Removes the last listed item in the Wayfarer's shop.";



		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (RewardsMod)this.mod;
			if( !mymod.SettingsConfig.DebugModeEnableCheats ) {
				throw new UsageException( "Cheat mode not enabled." );
			}

			ShopPackDefinition? def = RewardsAPI.ShopRemoveLastPack();
			
			if( def == null ) {
				caller.Reply( "No shop packs left to remove.", Color.Yellow );
			} else {
				mymod.SettingsConfigJson.SaveFile();
				caller.Reply( "Removed shop pack " + ( (ShopPackDefinition)def ).Name, Color.LimeGreen );
			}
		}
	}
}
