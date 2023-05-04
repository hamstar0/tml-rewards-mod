using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	class RewardsNPC : GlobalNPC {
		public override void OnKill( NPC npc ) {
			if( npc == null || Main.netMode == 1 ) { return; }  // Redundant?

			var myworld = ModContent.GetInstance<RewardsSystem>();
			if( myworld.Logic == null ) { return; }

			myworld.Logic.AddKillReward_Host( npc );
		}
	}
}
