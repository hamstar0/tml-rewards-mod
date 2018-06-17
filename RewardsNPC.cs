using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	class RewardsNPC : GlobalNPC {
		public override void NPCLoot( NPC npc ) {
			if( Main.netMode == 1 ) { return; }	// Redundant?
			
			var myworld = this.mod.GetModWorld<RewardsWorld>();
			
			if( myworld.Logic != null ) {
				myworld.Logic.AddKillReward( (RewardsMod)this.mod, npc );
			}
		}
	}
}
