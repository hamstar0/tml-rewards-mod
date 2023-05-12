using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;


namespace Rewards.NPCs {
	[AutoloadHead]
	partial class WayfarerTownNPC : ModNPC {
		public readonly static List<string> PossibleNames = new List<string>( new string[] {
			"Croft", "Grylls", "Jack", "Jones", "Shiren", "Yukino", "Wander"
		} );
		public readonly static List<string> ChatReplies = new List<string>( new string[] {
			"The winds shall guide you, and always be at your back.",
			"Fortune and glory, kid. Fortune and glory.",
			"With fate guiding my every move, I strode valiantly toward my destiny, and felt the gods smiling upon me.",
			"Survival can be summed up in three words - never give up.",
			"The line between life or death is determined by what we are willing to do.",
			"A famous explorer once said, that the extraordinary is in what we do, not who we are."
		} );

		////////////////

		private static int CurrentShop = 0;



		////////////////

		public override string Texture => "Rewards/NPCs/WayfarerTownNPC";
		public override string HeadTexture => "Rewards/NPCs/WayfarerTownNPC_Head";
		

		////////////////

		private bool IsFiring = false;



		////////////////

		public override void SetStaticDefaults() {
			int npcType = this.NPC.type;

			this.DisplayName.SetDefault( "Wayfarer" );

			Main.npcFrameCount[npcType] = 26;
			NPCID.Sets.AttackFrameCount[npcType] = 5;
			NPCID.Sets.DangerDetectRange[npcType] = 700;
			NPCID.Sets.AttackType[npcType] = 1;
			NPCID.Sets.AttackTime[npcType] = 30;
			NPCID.Sets.AttackAverageChance[npcType] = 30;
			NPCID.Sets.HatOffsetY[npcType] = 4;
		}

		public override void SetDefaults() {
			int npcType = this.NPC.type;
			
			this.NPC.townNPC = true;
			this.NPC.friendly = true;
			this.NPC.width = 18;
			this.NPC.height = 40;
			this.NPC.aiStyle = 7;
			this.NPC.damage = 10;
			this.NPC.defense = 15;
			this.NPC.lifeMax = 250;
			this.NPC.HitSound = SoundID.NPCHit1;
			this.NPC.DeathSound = SoundID.NPCDeath1;
			this.NPC.knockBackResist = 0.5f;
			this.AnimationType = NPCID.TravellingMerchant;

			WayfarerTownNPC.CurrentShop = 0;
		}



		////////////////

		public override void ModifyNPCLoot( NPCLoot npcLoot ) {
			npcLoot.Add( ItemDropRule.Common( ItemID.ArchaeologistsHat ) );
		}

		////////////////
		
		public override void AI() {
			if( this.NPC.ai[0] == 12 ) {
				if( !this.IsFiring ) {
					this.IsFiring = true;
					if( !Main.hardMode ) {
						SoundEngine.PlaySound( SoundID.Item11, this.NPC.position );
					} else {
						SoundEngine.PlaySound( SoundID.Item12, this.NPC.position );
					}
				}
			} else {
				if( this.IsFiring ) {
					this.IsFiring = false;
				}
			}
		}

		public override void DrawTownAttackGun( ref float scale, ref int item, ref int closeness ) {
			if( Main.hardMode ) {
				closeness = 18;
				item = ItemID.PulseBow;
			} else {
				if( this.NPC.ai[2] < -0.1f ) {
					closeness = 28;
				}
				scale = 0.75f;
				item = ItemID.Revolver;
			}
		}

		public override void TownNPCAttackStrength( ref int damage, ref float knockback ) {
			if( Main.hardMode ) {
				damage = 50;
				knockback = 4f;
			} else {
				damage = 20;
				knockback = 4f;
			}
		}

		public override void TownNPCAttackCooldown( ref int cooldown, ref int randExtraCooldown ) {
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj( ref int projType, ref int attackDelay ) {
			if( Main.hardMode ) {
				projType = ProjectileID.PulseBolt;
				attackDelay = 1;
			} else {
				projType = ProjectileID.Bullet;
				attackDelay = 1;
			}
		}

		public override void TownNPCAttackProjSpeed( ref float multiplier, ref float gravityCorrection, ref float randomOffset ) {
			multiplier = 12f;
			randomOffset = 0f;
		}

		////////////////

		public override List<string> SetNPCNameList()/* tModPorter Suggestion: Return a list of names */ {
			return WayfarerTownNPC.PossibleNames;
		}

		public override bool UsesPartyHat() {
			return false;   // :(
		}


		////////////////

		public override string GetChat() {
			return WayfarerTownNPC.ChatReplies[ Main.rand.Next( WayfarerTownNPC.ChatReplies.Count ) ];
		}
	}
}
