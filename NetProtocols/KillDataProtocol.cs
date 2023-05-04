using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using Rewards.Logic;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.NetProtocols {
	class KillDataProtocol : PacketProtocolRequestToServer {
		public static void QuickRequest() {
			PacketProtocolRequestToServer.QuickRequestToServer<KillDataProtocol>( -1 );
		}



		////////////////

		public KillData WorldData = null;
		public KillData PlayerData = null;



		////////////////

		private KillDataProtocol() { }

		////

		protected override void InitializeServerSendData( int toWho ) {
			Player player = Main.player[ toWho ];
			if( player == null /*|| !player.active*/ ) {
				LogHelpers.Warn( "Invalid player "+player.name+" (" + toWho+")" );
				return;
			}

			var mymod = RewardsMod.Instance;
			var myworld = ModContent.GetInstance<RewardsWorld>();

			var plrKillData = myworld.Logic.GetPlayerData( player );
			if( plrKillData == null ) {
				LogHelpers.Warn( "Could not get player " + player.name + "'s (" + toWho + ") kill data." );
				return;
			}

			//kill_data.AddToMe( mymod, myworld.Logic.WorldData );	// Why was this here?!
			this.WorldData = myworld.Logic.WorldData;
			this.PlayerData = plrKillData;
		}


		////////////////

		protected override bool ReceiveRequestWithServer( int fromWho ) {
			if( this.WorldData == null || this.PlayerData == null ) {
				LogHelpers.Alert( "Could not reply to request; no player id available ("
						+"WorldData:" + ( this.WorldData != null ) + ", PlayerData:" + ( this.PlayerData != null ) + ")." );
				return true;
			}

			var mymod = RewardsMod.Instance;
			Player player = Main.player[ fromWho ];
			var myplayer = (RewardsPlayer)TmlHelpers.SafelyGetModPlayer( player, mymod, "RewardsPlayer" );

			bool isSynced;
			myplayer.OnFinishPlayerEnterWorldForServer( out isSynced );

			return !isSynced;
		}

		protected override void ReceiveReply() {
			var mymod = RewardsMod.Instance;
			var myworld = ModContent.GetInstance<RewardsWorld>();
			var myplayer = (RewardsPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, mymod, "RewardsPlayer" );

			KillData plrData = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			KillData wldData = myworld.Logic.WorldData;
			if( plrData == null || wldData == null ) {
				LogHelpers.Alert( "Could not process reply. "+(plrData==null)+", "+(wldData==null) );
				return;
			}

			wldData.ResetAll( Main.LocalPlayer );
			wldData.AddToMe( this.WorldData );

			plrData.ResetAll( Main.LocalPlayer );
			plrData.AddToMe( this.PlayerData, Main.LocalPlayer );

			myplayer.FinishLocalSync();
		}
	}
}
