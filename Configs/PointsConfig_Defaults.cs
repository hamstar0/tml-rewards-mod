using System.Collections.Generic;
using System.Runtime.Serialization;
using Terraria.ID;
using Terraria.ModLoader.Config;


namespace Rewards.Configs {
	public partial class RewardsPointsConfig : ModConfig {
		[OnDeserialized]
		internal void OnDeserializedMethod( StreamingContext context ) {
			if( this.NpcRewards != null ) {
				return;
			}

			var wofDef = new NPCDefinition( NPCID.WallofFlesh );
			var planteraDef = new NPCDefinition( NPCID.Plantera );
			var golemDef = new NPCDefinition( NPCID.Golem );
			var moonlordDef = new NPCDefinition( NPCID.MoonLordCore );  //NPCID.MoonLordHead?

			this.NpcRewards = new Dictionary<NPCDefinition, float> {
				{ new NPCDefinition( NPCID.KingSlime ), 10f },
				{ new NPCDefinition( NPCID.EyeofCthulhu ), 10f },
				{ new NPCDefinition( NPCID.EaterofWorldsHead ), 25f },
				{ new NPCDefinition( NPCID.BrainofCthulhu ), 25f },
				{ new NPCDefinition( NPCID.QueenBee ), 20f },
				{ new NPCDefinition( NPCID.SkeletronHead ), 30f },
				{ new NPCDefinition( NPCID.WallofFlesh ), 50f },
				{ new NPCDefinition( NPCID.TheDestroyer ), 50f },
				{ new NPCDefinition( NPCID.Retinazer ), 50f / 2 },
				{ new NPCDefinition( NPCID.Spazmatism ), 50f / 2 },
				{ new NPCDefinition( NPCID.SkeletronPrime ), 50f },
				{ new NPCDefinition( NPCID.Plantera ), 100f },
				{ new NPCDefinition( NPCID.Golem ), 100f },
				{ new NPCDefinition( NPCID.DukeFishron ), 100f },
				{ new NPCDefinition( NPCID.CultistBoss ), 50f },
				{ new NPCDefinition( NPCID.DD2Betsy ), 100f },
				{ new NPCDefinition( NPCID.LunarTowerSolar ), 35f },
				{ new NPCDefinition( NPCID.LunarTowerVortex ), 35f },
				{ new NPCDefinition( NPCID.LunarTowerNebula ), 35f },
				{ new NPCDefinition( NPCID.LunarTowerStardust ), 35f },
				{ moonlordDef, 250f }
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
