using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Utilities.Errors;
using HamstarHelpers.Utilities.Network;
using Terraria;


namespace Rewards.NetProtocols {
	class ModSaveProtocol : PacketProtocol {
		public string Uid;	// Just in case?


		////////////////
		
		public override void SetClientDefaults() {
			bool has_uid;
			string uid = PlayerIdentityHelpers.GetUniqueId( Main.LocalPlayer, out has_uid );

			if( !has_uid ) {
				throw new HamstarException( "No uid for local player " + Main.LocalPlayer.name );
			}

			this.Uid = uid;
		}

		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			if( RewardsMod.Instance.Config.DebugModeInfo ) {
				LogHelpers.Log( "RewardsModSaveProtocol.ReceiveOnServer - who: " + from_who+", uid: "+this.Uid );
			}

			Player player = Main.player[ from_who ];
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.SaveKillData();
		}
	}
}
