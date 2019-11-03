using HamstarHelpers.Classes.PlayerData;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Messages.Inbox;
using Terraria;


namespace Rewards {
	partial class RewardsCustomPlayer : CustomPlayerData {
		protected override void OnEnter( object data ) {
			if( this.PlayerWho != Main.myPlayer ) {
				return;
			}

			var myplayer = TmlHelpers.SafelyGetModPlayer<RewardsPlayer>( this.Player );

			if( Main.netMode == 0 ) {
				myplayer.OnConnectSingle();
			}
			if( Main.netMode == 1 ) {
				myplayer.OnConnectCurrentClient();
			}

			InboxMessages.SetMessage( "RewardsModConfigUpdate",
				"Rewards config files updated to use ModConfig (tML v0.11+). The old config files "+
				"(Rewards Config.json, Rewards Points Config.json, Rewards Shop Config.json) are now obsolete. "+
				"If any mod settings have been changed from their defaults in the past, you'll need to import them "+
				"manually (preferably via. the menu's Mod Configuration).",
				false
			);
		}
	}
}
