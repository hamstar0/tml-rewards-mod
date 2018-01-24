using HamstarHelpers.DebugHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.NetProtocol {
	static class ServerPacketHandlers {
		public static void HandlePacket( RewardsMod mymod, BinaryReader reader, int player_who ) {
			RewardsProtocolTypes protocol = (RewardsProtocolTypes)reader.ReadByte();
			
			switch( protocol ) {
			case RewardsProtocolTypes.RequestModSettings:
				ServerPacketHandlers.ReceiveModSettingsRequest( mymod, reader, player_who );
				break;
			default:
				LogHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}


		
		////////////////
		// Server Senders
		////////////////

		public static void SendModSettings( RewardsMod mymod, int to_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)RewardsProtocolTypes.ModSettings );
			packet.Write( (string)mymod.JsonConfig.SerializeMe() );

			packet.Send( to_who );
		}

		public static void SendNpcKillReward( RewardsMod mymod, int killer_who, int npc_type, bool is_grind, float reward, int to_who, int ignore_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }
			
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)RewardsProtocolTypes.NpcKillReward );
			packet.Write( (int)killer_who );
			packet.Write( (int)npc_type );
			packet.Write( (bool)is_grind );
			packet.Write( (float)reward );

			packet.Send( to_who, ignore_who );
		}



		////////////////
		// Server Receivers
		////////////////

		private static void ReceiveModSettingsRequest( RewardsMod mymod, BinaryReader reader, int player_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }

			ServerPacketHandlers.SendModSettings( mymod, player_who );
		}
	}
}
