using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Terraria;


namespace Rewards.NetProtocols {
	class PlayerSaveProtocol : PacketProtocolSendToServer {
		public string Uid;  // Just in case?



		////////////////

		private PlayerSaveProtocol() { }

		////

		protected override void InitializeClientSendData() {
			this.Uid = PlayerIdentityHelpers.GetMyProperUniqueId();
		}


		////////////////

		protected override void Receive( int fromWho ) {
			Player player = Main.player[ fromWho ];
			if( player == null || !player.active ) {
				throw new HamstarException( "!Rewards.PlayerSaveProtocol.ReceiveWithServer - Player id " + fromWho + " invalid?" );
			}

			var myplayer = player.GetModPlayer<RewardsPlayer>();

			if( PlayerIdentityHelpers.GetProperUniqueId(player) != this.Uid ) {
				throw new HamstarException( "!Rewards.PlayerSaveProtocol.ReceiveWithServer - Player UID mismatch for "+player.name+" ("+player.whoAmI+")" );
			}

			myplayer.SaveKillData();
		}
	}
}
