using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Rewards {
	class RewardsNPC : GlobalNPC {
		public override void OnKill( NPC npc ) {
			if( npc == null || Main.netMode == NetmodeID.MultiplayerClient ) { return; }  // Redundant?

			var myworld = ModContent.GetInstance<RewardsSystem>();
			if( myworld.Logic == null ) { return; }

			myworld.Logic.AddKillReward_Host( npc );
		}
	}
}
