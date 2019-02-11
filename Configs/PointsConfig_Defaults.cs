using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.NPCHelpers;
using System.Collections.Generic;
using Terraria.ID;


namespace Rewards.Configs {
	public partial class RewardsPointsConfigData : ConfigurationDataBase {
		public void SetDefaults() {
			string wofName = NPCIdentityHelpers.GetQualifiedName( NPCID.WallofFlesh );
			string planteraName = NPCIdentityHelpers.GetQualifiedName( NPCID.Plantera );
			string golemName = NPCIdentityHelpers.GetQualifiedName( NPCID.Golem );
			string moonlordName = NPCIdentityHelpers.GetQualifiedName( NPCID.MoonLordCore );  //NPCID.MoonLordHead?

			this.NpcRewards = new Dictionary<string, float> {
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.KingSlime ), 10f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.EyeofCthulhu ), 10f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.EaterofWorldsHead ), 25f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.BrainofCthulhu ), 25f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.QueenBee ), 20f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.SkeletronHead ), 30f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.WallofFlesh ), 50f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.TheDestroyer ), 50f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Retinazer ), 50f / 2 },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Spazmatism ), 50f / 2 },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.SkeletronPrime ), 50f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Plantera ), 100f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Golem ), 100f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.DukeFishron ), 100f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.CultistBoss ), 50f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.DD2Betsy ), 100f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.LunarTowerSolar ), 35f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.LunarTowerVortex ), 35f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.LunarTowerNebula ), 35f },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.LunarTowerStardust ), 35f },
				{ moonlordName, 250f }
			};

			this.NpcRewardRequiredAsBoss = new HashSet<string> {
				NPCIdentityHelpers.GetQualifiedName( NPCID.EaterofWorldsHead )
			};

			this.NpcRewardNotGivenAfterNpcKilled = new Dictionary<string, string> {
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.KingSlime ), wofName },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.EyeofCthulhu ), wofName },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.EaterofWorldsHead ), wofName },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.BrainofCthulhu ), wofName },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.QueenBee ), wofName },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.SkeletronHead ), wofName },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.WallofFlesh ), planteraName },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.TheDestroyer ), golemName },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Retinazer ), golemName },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Spazmatism ), golemName },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.SkeletronPrime ), golemName },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Plantera ), moonlordName },
				{ NPCIdentityHelpers.GetQualifiedName( NPCID.Golem ), moonlordName }
			};
		}
	}
}
