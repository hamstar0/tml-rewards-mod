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
				ClientPacketHandlers.ReceiveModSettings( mymod, reader );
				break;
			case RewardsProtocolTypes.NpcKillReward:
				ClientPacketHandlers.ReceiveNpcKillReward( mymod, reader );
				break;
			default:
				LogHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////
		// Client Senders
		////////////////

		public static void SendRequestModSettings( RewardsMod mymod ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)RewardsProtocolTypes.RequestModSettings );

			packet.Send();
		}




		////////////////
		// Client Receivers
		////////////////

		private static void ReceiveModSettings( RewardsMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			mymod.JsonConfig.DeserializeMe( reader.ReadString() );
		}
		
		private static void ReceiveNpcKillReward( RewardsMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			int killer_who = reader.ReadInt32();
			int npc_type = reader.ReadInt32();
			bool is_grind = reader.ReadBoolean();
			float reward = reader.ReadSingle();
			var modplayer = Main.LocalPlayer.GetModPlayer<RewardsPlayer>();

			modplayer.Logic.AddKillReward( mymod, Main.LocalPlayer, npc_type, is_grind, reward );
		}
	}
}
