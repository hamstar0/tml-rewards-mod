using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader.Config;


namespace Rewards.Configs {
	public partial class RewardsPointsConfig : ModConfig {
		public RewardsPointsConfig() {
			var wofDef = new NPCDefinition( NPCID.WallofFlesh );
			var planteraDef = new NPCDefinition( NPCID.Plantera );
			var golemDef = new NPCDefinition( NPCID.Golem );
			var moonlordDef = new NPCDefinition( NPCID.MoonLordCore );  //NPCID.MoonLordHead?

			this.NpcRewardsOnKill = new Dictionary<NPCDefinition, NPCKillRewardValue> {
				{ new NPCDefinition( NPCID.KingSlime ), new NPCKillRewardValue(10) },
				{ new NPCDefinition( NPCID.EyeofCthulhu ), new NPCKillRewardValue(10) },
				{ new NPCDefinition( NPCID.EaterofWorldsHead ), new NPCKillRewardValue(25) },
				{ new NPCDefinition( NPCID.BrainofCthulhu ), new NPCKillRewardValue(25) },
				{ new NPCDefinition( NPCID.QueenBee ), new NPCKillRewardValue(20) },
				{ new NPCDefinition( NPCID.SkeletronHead ), new NPCKillRewardValue(30) },
				{ new NPCDefinition( NPCID.WallofFlesh ), new NPCKillRewardValue(50) },
				{ new NPCDefinition( NPCID.TheDestroyer ), new NPCKillRewardValue(50) },
				{ new NPCDefinition( NPCID.Retinazer ), new NPCKillRewardValue(50 / 2) },
				{ new NPCDefinition( NPCID.Spazmatism ), new NPCKillRewardValue(50 / 2) },
				{ new NPCDefinition( NPCID.SkeletronPrime ), new NPCKillRewardValue(50) },
				{ new NPCDefinition( NPCID.Plantera ), new NPCKillRewardValue(100) },
				{ new NPCDefinition( NPCID.Golem ), new NPCKillRewardValue(100) },
				{ new NPCDefinition( NPCID.DukeFishron ), new NPCKillRewardValue(100) },
				{ new NPCDefinition( NPCID.CultistBoss ), new NPCKillRewardValue(50) },
				{ new NPCDefinition( NPCID.DD2Betsy ), new NPCKillRewardValue(100) },
				{ new NPCDefinition( NPCID.LunarTowerSolar ), new NPCKillRewardValue(35) },
				{ new NPCDefinition( NPCID.LunarTowerVortex ), new NPCKillRewardValue(35) },
				{ new NPCDefinition( NPCID.LunarTowerNebula ), new NPCKillRewardValue(35) },
				{ new NPCDefinition( NPCID.LunarTowerStardust ), new NPCKillRewardValue(35) },
				{ moonlordDef, new NPCKillRewardValue(250) }
			};

			this.NpcRewardRequiredAsBoss = new HashSet<NPCDefinition> {
				new NPCDefinition( NPCID.EaterofWorldsHead )
			};

			this.NpcRewardNotGivenAfterNpcKilled = new Dictionary<NPCDefinition, NPCDefinition> {
				{ new NPCDefinition( NPCID.KingSlime ), wofDef },
				{ new NPCDefinition( NPCID.EyeofCthulhu ), wofDef },
				{ new NPCDefinition( NPCID.EaterofWorldsHead ), wofDef },
				{ new NPCDefinition( NPCID.BrainofCthulhu ), wofDef },
				{ new NPCDefinition( NPCID.QueenBee ), wofDef },
				{ new NPCDefinition( NPCID.SkeletronHead ), wofDef },
				{ new NPCDefinition( NPCID.WallofFlesh ), planteraDef },
				{ new NPCDefinition( NPCID.TheDestroyer ), golemDef },
				{ new NPCDefinition( NPCID.Retinazer ), golemDef },
				{ new NPCDefinition( NPCID.Spazmatism ), golemDef },
				{ new NPCDefinition( NPCID.SkeletronPrime ), golemDef },
				{ new NPCDefinition( NPCID.Plantera ), moonlordDef },
				{ new NPCDefinition( NPCID.Golem ), moonlordDef }
			};
		}
	}
}
