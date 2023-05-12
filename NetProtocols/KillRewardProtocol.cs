using System;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using Rewards.Logic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Rewards.NetProtocols {
	[Serializable]
	class KillRewardProtocol : SimplePacketPayload {
		public static void SendRewardToClient( int toWho, int ignoreWho, NPC npc ) {
			SimplePacket.SendToClient( new KillRewardProtocol( toWho, npc.type, npc.boss ), toWho, ignoreWho );
		}



		////////////////

		//public override bool IsVerbose => false;
		
		////////////////

		public int KillerWho;
		public int NpcType;
		public bool IsBoss;



		////////////////

		public KillRewardProtocol( int killerWho, int npcType, bool isBoss ) {
			this.KillerWho = killerWho;
			this.NpcType = npcType;
			this.IsBoss = isBoss;
		}


		////////////////

		public override void ReceiveOnClient() {
			var mymod = RewardsMod.Instance;
			var myworld = ModContent.GetInstance<RewardsSystem>();
			KillData data = myworld.Logic.GetPlayerData( Main.LocalPlayer );

			if( data == null ) {
				LogLibraries.Warn( "No player data for " + Main.LocalPlayer.name );
				return;
			}

			NPC npc = new NPC();
			npc.SetDefaults( this.NpcType );
			npc.boss = this.IsBoss;

			bool isGrind, isExpired;
			float reward = data.RecordKill( npc, out isGrind, out isExpired );

			if( mymod.SettingsConfig.DebugModeKillInfo ) {
				int kills = data.KilledNpcs.ContainsKey( npc.type ) ? data.KilledNpcs[npc.type] : -1;
				var npcDef = new NPCDefinition( npc.type );
				bool needsBoss = mymod.PointsConfig.NpcRewardRequiredAsBoss.Contains( npcDef );

				string msg = "ReceiveOnClient npc: " + npc.TypeName + " (" + npc.type + ")" + ", #: " + kills
						+ ", isGrind: " + isGrind + ", isExpired: "+isExpired+", reward: " + reward
						+ ", needsBoss:" + needsBoss + " (is? " + npc.boss + ")";
				Main.NewText( msg );
				LogLibraries.Log( " " + msg );
			}

			data.AddRewardForPlayer( Main.LocalPlayer, isGrind, isExpired, reward );
		}

		public override void ReceiveOnServer( int fromWho ) {
			
		}
	}
}
