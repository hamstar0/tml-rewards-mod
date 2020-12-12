using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Helpers.TModLoader;
using Terraria;


namespace Rewards.NetProtocols {
	class PlayerSaveProtocol : PacketProtocolSendToServer {
		public static void QuickSend() {
			PacketProtocolSendToServer.QuickSendToServer<PlayerSaveProtocol>();
		}



		////////////////

		public string Uid;  // Just in case?



		////////////////

		private PlayerSaveProtocol() { }

		////

		protected override void InitializeClientSendData() {
			this.Uid = PlayerIdentityHelpers.GetUniqueId();
		}


		////////////////

		protected override void Receive( int fromWho ) {
			Player player = Main.player[ fromWho ];
			if( player == null || !player.active ) {
				throw new ModHelpersException( "Could not save. Player id " + fromWho + " invalid?" );
			}

			var mymod = RewardsMod.Instance;
			var myplayer = (RewardsPlayer)TmlHelpers.SafelyGetModPlayer( player, mymod, "RewardsPlayer" );

			if( PlayerIdentityHelpers.GetUniqueId(player) != this.Uid ) {
				throw new ModHelpersException( "Could not save. Player UID mismatch for "+player.name+" ("+player.whoAmI+")" );
			}

			myplayer.SaveKillData();
		}
	}
}
