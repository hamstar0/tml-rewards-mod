using HamstarHelpers.Helpers.NPCs;
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

			string wofName = NPCIdentityHelpers.GetUniqueKey( NPCID.WallofFlesh );
			string planteraName = NPCIdentityHelpers.GetUniqueKey( NPCID.Plantera );
			string golemName = NPCIdentityHelpers.GetUniqueKey( NPCID.Golem );
			string moonlordName = NPCIdentityHelpers.GetUniqueKey( NPCID.MoonLordCore );  //NPCID.MoonLordHead?

			this.NpcRewards = new Dictionary<string, float> {
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.KingSlime ), 10f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.EyeofCthulhu ), 10f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.EaterofWorldsHead ), 25f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.BrainofCthulhu ), 25f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.QueenBee ), 20f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.SkeletronHead ), 30f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.WallofFlesh ), 50f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.TheDestroyer ), 50f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.Retinazer ), 50f / 2 },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.Spazmatism ), 50f / 2 },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.SkeletronPrime ), 50f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.Plantera ), 100f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.Golem ), 100f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.DukeFishron ), 100f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.CultistBoss ), 50f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.DD2Betsy ), 100f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.LunarTowerSolar ), 35f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.LunarTowerVortex ), 35f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.LunarTowerNebula ), 35f },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.LunarTowerStardust ), 35f },
				{ moonlordName, 250f }
			};

			this.NpcRewardRequiredAsBoss = new HashSet<string> {
				NPCIdentityHelpers.GetUniqueKey( NPCID.EaterofWorldsHead )
			};

			this.NpcRewardNotGivenAfterNpcKilled = new Dictionary<string, string> {
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.KingSlime ), wofName },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.EyeofCthulhu ), wofName },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.EaterofWorldsHead ), wofName },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.BrainofCthulhu ), wofName },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.QueenBee ), wofName },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.SkeletronHead ), wofName },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.WallofFlesh ), planteraName },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.TheDestroyer ), golemName },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.Retinazer ), golemName },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.Spazmatism ), golemName },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.SkeletronPrime ), golemName },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.Plantera ), moonlordName },
				{ NPCIdentityHelpers.GetUniqueKey( NPCID.Golem ), moonlordName }
			};
		}
	}
}
