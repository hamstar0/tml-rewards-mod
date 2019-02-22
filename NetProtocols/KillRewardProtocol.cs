﻿using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class KillRewardProtocol : PacketProtocolSentToEither {
		public static void SendRewardToClient( int toWho, int ignoreWho, NPC npc ) {
			var protocol = new KillRewardProtocol( toWho, npc.type, npc.boss );
			protocol.SendToClient( toWho, ignoreWho );
		}



		////////////////

		public override bool IsVerbose => false;
		
		////////////////

		public int KillerWho;
		public int NpcType;
		public bool IsBoss;



		////////////////

		private KillRewardProtocol() { }

		private KillRewardProtocol( int killerWho, int npcType, bool isBoss ) {
			this.KillerWho = killerWho;
			this.NpcType = npcType;
			this.IsBoss = isBoss;
		}


		////////////////

		protected override void ReceiveOnClient() {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			if( data == null ) {
				throw new HamstarException( "No player data for " + Main.LocalPlayer.name );
			}

			NPC npc = new NPC();
			npc.SetDefaults( this.NpcType );
			npc.boss = this.IsBoss;

			bool isGrind, isExpired;
			float reward = data.RecordKill( npc, out isGrind, out isExpired );

			data.AddRewardForPlayer( Main.LocalPlayer, isGrind, isExpired, reward );
		}

		protected override void ReceiveOnServer( int fromWho ) {
			throw new System.NotImplementedException();
		}
	}
}
