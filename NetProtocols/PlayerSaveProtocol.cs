using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Terraria;


namespace Rewards.NetProtocols {
	class PlayerSaveProtocol : PacketProtocolSendToServer {
		public string Uid;  // Just in case?



		////////////////

		protected PlayerSaveProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		protected override void InitializeClientSendData() {
			this.Uid = PlayerIdentityHelpers.GetMyProperUniqueId();
		}


		////////////////

		protected override void Receive( int from_who ) {
			Player player = Main.player[ from_who ];
			if( player == null || !player.active ) {
				throw new HamstarException( "!Rewards.PlayerSaveProtocol.ReceiveWithServer - Player id " + from_who + " invalid?" );
			}

			var myplayer = player.GetModPlayer<RewardsPlayer>();

			if( PlayerIdentityHelpers.GetProperUniqueId(player) != this.Uid ) {
				throw new HamstarException( "!Rewards.PlayerSaveProtocol.ReceiveWithServer - Player UID mismatch for "+player.name+" ("+player.whoAmI+")" );
			}

			myplayer.SaveKillData();
		}
	}
}
