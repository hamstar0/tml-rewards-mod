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
			bool has_uid;
			string uid = PlayerIdentityHelpers.GetUniqueId( Main.LocalPlayer, out has_uid );

			if( !has_uid ) {
				throw new HamstarException( "!Rewards.NetProtocols.PlayerSaveProtocol - No uid for local player " + Main.LocalPlayer.name );
			}

			this.Uid = uid;
		}


		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			Player player = Main.player[ from_who ];
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.SaveKillData();
		}
	}
}
