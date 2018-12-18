using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class KillRewardProtocol : PacketProtocolSentToEither {
		protected class MyFactory : Factory<KillRewardProtocol> {
			public int KillerWho;
			public int NpcType;

			public MyFactory( int killer_who, int npc_type ) {
				this.KillerWho = killer_who;
				this.NpcType = npc_type;
			}

			protected override void Initialize( KillRewardProtocol data ) {
				data.KillerWho = this.KillerWho;
				data.NpcType = this.NpcType;
			}
		}



		////////////////

		public static void SendRewardToClient( int to_who, int ignore_who, int npc_type ) {
			var factory = new MyFactory( to_who, npc_type );
			KillRewardProtocol protocol = factory.Create();

			protocol.SendToClient( to_who, ignore_who );
		}
			
		////////////////


		public override bool IsVerbose { get { return false; } }


		////////////////

		public int KillerWho;
		public int NpcType;


		////////////////

		protected KillRewardProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }
		
		////////////////

		protected override void ReceiveOnClient() {
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

		protected override void ReceiveOnServer( int fromWho ) {
			throw new System.NotImplementedException();
		}
	}
}
