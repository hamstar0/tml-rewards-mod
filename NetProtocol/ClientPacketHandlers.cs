using HamstarHelpers.DebugHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.NetProtocol {
	static class ClientPacketHandlers {
		public static void HandlePacket( RewardsMod mymod, BinaryReader reader ) {
			RewardsProtocolTypes protocol = (RewardsProtocolTypes)reader.ReadByte();
			
			switch( protocol ) {
			case RewardsProtocolTypes.ModSettings:
				ClientPacketHandlers.ReceiveModSettingsOnClient( mymod, reader );
				break;
			case RewardsProtocolTypes.NpcKillReward:
				ClientPacketHandlers.ReceiveNpcKillRewardOnClient( mymod, reader );
				break;
			default:
				LogHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////
		// Client Senders
		////////////////

		public static void SendRequestModSettingsFromClient( RewardsMod mymod ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)RewardsProtocolTypes.RequestModSettings );

			packet.Send();
		}

		public static void SendSignalNpcKillRewardFromClient( RewardsMod mymod, int npc_type ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)RewardsProtocolTypes.SignalNpcKillRewardReceipt );
			packet.Write( (int)npc_type );

			packet.Send();
		}




		////////////////
		// Client Receivers
		////////////////

		private static void ReceiveModSettingsOnClient( RewardsMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			mymod.JsonConfig.DeserializeMe( reader.ReadString() );
		}
		
		private static void ReceiveNpcKillRewardOnClient( RewardsMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			int npc_type = reader.ReadInt32();
			var modplayer = Main.LocalPlayer.GetModPlayer<RewardsPlayer>();

			modplayer.Logic.AddKillReward( mymod, npc_type, Main.LocalPlayer );
		}
	}
}
