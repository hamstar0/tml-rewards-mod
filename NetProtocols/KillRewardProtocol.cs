using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class KillRewardProtocol : PacketProtocolSentToEither {
		public static void SendRewardToClient( int toWho, int ignoreWho, int npcType ) {
			var protocol = new KillRewardProtocol( toWho, npcType );
			protocol.SendToClient( toWho, ignoreWho );
		}
		


		////////////////

		public override bool IsVerbose { get { return false; } }
		
		////////////////

		public int KillerWho;
		public int NpcType;



		////////////////

		private KillRewardProtocol() { }

		private KillRewardProtocol( int killerWho, int npcType ) {
			this.KillerWho = killerWho;
			this.NpcType = npcType;
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

			bool isGrind, isExpired;
			float reward = data.RecordKill_NoSync( npc, out isGrind, out isExpired );

			data.AddRewardForPlayer( Main.LocalPlayer, isGrind, isExpired, reward );
		}

		protected override void ReceiveOnServer( int fromWho ) {
			throw new System.NotImplementedException();
		}
	}
}
