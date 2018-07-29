using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.DebugHelpers;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class KillRewardProtocol : PacketProtocol {
		public static void SendRewardToClient( int to_who, int ignore_who, int npc_type ) {
			var protocol = new KillRewardProtocol( to_who, npc_type );
			protocol.SendToClient( to_who, ignore_who );
		}
			
		////////////////


		public override bool IsVerbose { get { return false; } }


		////////////////

		public int KillerWho;
		public int NpcType;


		////////////////

		public KillRewardProtocol() { }

		internal KillRewardProtocol( int killer_who, int npc_type ) {
			this.KillerWho = killer_who;
			this.NpcType = npc_type;
		}

		////////////////

		protected override void ReceiveWithClient() {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			if( data == null ) {
				throw new HamstarException( "RewardsModNpcKillRewardProtocol.ReceiveOnClient() - No player data for " + Main.LocalPlayer.name );
			}

			NPC npc = new NPC();
			npc.SetDefaults( this.NpcType );

			bool is_grind, is_expired;
			float reward = data.RecordKill_NoSync( mymod, npc, out is_grind, out is_expired );

			data.AddRewardForPlayer( mymod, Main.LocalPlayer, is_grind, is_expired, reward );
		}
	}
}
