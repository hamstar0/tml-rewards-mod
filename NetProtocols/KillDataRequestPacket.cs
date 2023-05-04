using System;
using ModLibsCore.Services.Network.SimplePacket;


namespace Rewards.NetProtocols {
	[Serializable]
	class KillDataRequestPacket : SimplePacketPayload {
		public KillDataRequestPacket() { }

		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			KillDataReplyPacket.TryReplyToClientRequest( fromWho );
		}

		public override void ReceiveOnClient() {
			
		}
	}
}
