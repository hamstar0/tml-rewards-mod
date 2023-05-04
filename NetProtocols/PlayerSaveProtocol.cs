using System;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using ModLibsGeneral.Libraries.Players;
using Terraria;


namespace Rewards.NetProtocols {
	[Serializable]
	class PlayerSaveProtocol : SimplePacketPayload {
		public string Uid;  // Just in case?



		////////////////

		public PlayerSaveProtocol() {
			this.Uid = PlayerIdentityLibraries.GetUniqueId();
		}

		////////////////

		public override void ReceiveOnClient() {
			
		}

		public override void ReceiveOnServer( int fromWho ) {
			Player player = Main.player[ fromWho ];
			if( player == null || !player.active ) {
				LogLibraries.Warn( "Could not save. Player id " + fromWho + " invalid?" );
				return;
			}

			var mymod = RewardsMod.Instance;
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			if( PlayerIdentityLibraries.GetUniqueId(player) != this.Uid ) {
				LogLibraries.Warn( "Could not save. Player UID mismatch for "+player.name+" ("+player.whoAmI+")" );
				return;
			}

			myplayer.SaveKillData();
		}
	}
}
