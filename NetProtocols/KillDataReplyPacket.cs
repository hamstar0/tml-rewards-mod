using System;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using Rewards.Logic;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.NetProtocols {
	[Serializable]
	class KillDataReplyPacket : SimplePacketPayload {
		public static void TryReplyToClientRequest( int toWho ) {
			Player player = Main.player[ toWho ];
			if( player == null /*|| !player.active*/ ) {
				LogLibraries.Warn( "Invalid player "+player.name+" (" + toWho+")" );
				return;
			}

			var mymod = RewardsMod.Instance;
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.OnFinishPlayerEnterWorldForServer( out bool isSynced );

			if( isSynced ) {
				return;
			}

			var myworld = ModContent.GetInstance<RewardsSystem>();

			var plrKillData = myworld.Logic.GetPlayerData( player );
			if( plrKillData == null ) {
				LogLibraries.Warn( "Could not get player " + player.name + "'s (" + toWho + ") kill data." );
				return;
			}

			//kill_data.AddToMe( mymod, myworld.Logic.WorldData );	// Why was this here?!

			var packet = new KillDataReplyPacket() {
				WorldData = myworld.Logic.WorldData,
				PlayerData = plrKillData,
			};

			SimplePacket.SendToClient( packet, toWho );
		}

		////////////////

		public KillData WorldData = null;
		public KillData PlayerData = null;



		////////////////

		private KillDataReplyPacket() { }

		////////////////

		public override void ReceiveOnClient() {
			var mymod = RewardsMod.Instance;
			var myworld = ModContent.GetInstance<RewardsSystem>();

			if( !Main.LocalPlayer.TryGetModPlayer( out RewardsPlayer myplayer ) ) {
				return;
			}

			KillData plrData = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			KillData wldData = myworld.Logic.WorldData;
			if( plrData == null || wldData == null ) {
				LogLibraries.Alert( "Could not process reply. "+(plrData==null)+", "+(wldData==null) );
				return;
			}

			wldData.ResetAll( Main.LocalPlayer );
			wldData.AddToMe( this.WorldData );

			plrData.ResetAll( Main.LocalPlayer );
			plrData.AddToMe( this.PlayerData, Main.LocalPlayer );

			myplayer.FinishLocalSync();
		}

		public override void ReceiveOnServer( int fromWho ) {
			
		}
	}
}
