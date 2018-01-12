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
				ServerPacketHandlers.ReceiveModSettingsRequestOnServer( mymod, reader, player_who );
				break;
			case RewardsProtocolTypes.SignalNpcKillRewardReceipt:
				ServerPacketHandlers.ReceiveNpcKillRewardSignalOnServer( mymod, reader, player_who );
				break;
			default:
				LogHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}


		
		////////////////
		// Server Senders
		////////////////

		public static void SendModSettingsFromServer( RewardsMod mymod, int to_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)RewardsProtocolTypes.ModSettings );
			packet.Write( (string)mymod.JsonConfig.SerializeMe() );

			packet.Send( to_who );
		}

		public static void SendNpcKillRewardFromServer( RewardsMod mymod, int to_who, int ignore_who, int npc_type ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }
			
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)RewardsProtocolTypes.NpcKillReward );
			packet.Write( (int)npc_type );

			packet.Send( to_who, ignore_who );
		}



		////////////////
		// Server Receivers
		////////////////

		private static void ReceiveModSettingsRequestOnServer( RewardsMod mymod, BinaryReader reader, int player_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }

			ServerPacketHandlers.SendModSettingsFromServer( mymod, player_who );
		}
		
		private static void ReceiveNpcKillRewardSignalOnServer( RewardsMod mymod, BinaryReader reader, int player_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }

			int npc_type = reader.ReadInt32();
			var modplayer = Main.player[player_who].GetModPlayer<RewardsPlayer>();
			
			if( modplayer.Logic.CanReceiveOtherPlayerKillRewards( mymod ) ) {
				ServerPacketHandlers.SendNpcKillRewardFromServer( mymod, -1, player_who, npc_type );
			}
		}
	}
}
