using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Terraria;


namespace Rewards.NetProtocols {
	class PlayerSaveProtocol : PacketProtocol {
		public string Uid;  // Just in case?


		////////////////

		private PlayerSaveProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		protected override void SetClientDefaults() {
			this.Uid = PlayerIdentityHelpers.GetMyProperUniqueId();
		}


		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			Player player = Main.player[ from_who ];
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.SaveKillData();
		}
	}
}
