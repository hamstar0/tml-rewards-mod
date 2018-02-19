using Microsoft.Xna.Framework;
using Rewards.Items;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Rewards.NPCs {
	[AutoloadHead]
	class WayfarerTownNPC : ModNPC {
		public readonly static IList<string> PossibleNames = new List<string>( new string[] {
			"Croft", "Grylls", "Jack", "Jones", "Shiren", "Yukino", "Wander"
		} );
		public readonly static IList<string> ChatReplies = new List<string>( new string[] {
			"The winds shall guide you, and always be at your back.",
			"Fortune and glory, kid. Fortune and glory.",
			"With fate guiding my every move, I strode valiantly toward my destiny, and felt the gods smiling upon me.",
			"Survival can be summed up in three words – never give up.",
			"The line between life or death is determined by what we are willing to do.",
			"A famous explorer once said, that the extraordinary is in what we do, not who we are."
		} );

		////////////////

		private static int CurrentShop = 0;


		////////////////

		public override string Texture { get { return "Rewards/NPCs/WayfarerTownNPC"; } }
		public override string HeadTexture { get { return "Rewards/NPCs/WayfarerTownNPC_Head"; } }


		////////////////

		public override bool Autoload( ref string name ) {
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults() {
			int npc_type = this.npc.type;

			this.DisplayName.SetDefault( "Wayfarer" );

			Main.npcFrameCount[npc_type] = 26;
			NPCID.Sets.AttackFrameCount[npc_type] = 5;
			NPCID.Sets.DangerDetectRange[npc_type] = 700;
			NPCID.Sets.AttackType[npc_type] = 1;
			NPCID.Sets.AttackTime[npc_type] = 30;
			NPCID.Sets.AttackAverageChance[npc_type] = 30;
			NPCID.Sets.HatOffsetY[npc_type] = 4;
		}

		public override void SetDefaults() {
			int npc_type = this.npc.type;
			
			this.npc.townNPC = true;
			this.npc.friendly = true;
			this.npc.width = 18;
			this.npc.height = 40;
			this.npc.aiStyle = 7;
			this.npc.damage = 10;
			this.npc.defense = 15;
			this.npc.lifeMax = 250;
			this.npc.HitSound = SoundID.NPCHit1;
			this.npc.DeathSound = SoundID.NPCDeath1;
			this.npc.knockBackResist = 0.5f;
			this.animationType = NPCID.TravellingMerchant;

			WayfarerTownNPC.CurrentShop = 0;
		}


		////////////////

		public override bool CanTownNPCSpawn( int num_town_npcs, int money ) {
			if( num_town_npcs == 0 ) { return true; }

			int npc_type = this.mod.NPCType<WayfarerTownNPC>();
			int counted_town_npcs = 0;

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc == null || !npc.active || !npc.townNPC ) { continue; }

				if( npc.type == npc_type ) {
					return false;
				}

				counted_town_npcs++;
				if( counted_town_npcs >= num_town_npcs ) { break; }
			}
			return true;
		}
		
		public static bool CanTownNPCSpawn( RewardsMod mymod ) {
			int npc_type = mymod.NPCType<WayfarerTownNPC>();

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc == null || !npc.active || !npc.townNPC ) { continue; }

				if( npc.type == npc_type ) {
					return false;
				}
			}
			return true;
		}


		////////////////

		public override void NPCLoot() {
			Item.NewItem( npc.getRect(), ItemID.ArchaeologistsHat );
		}


		////////////////

		private bool IsFiring = false;
		
		public override void AI() {
			if( this.npc.ai[0] == 12 ) {
				if( !this.IsFiring ) {
					this.IsFiring = true;
					if( !Main.hardMode ) {
						Main.PlaySound( SoundID.Item11, this.npc.position );
					} else {
						Main.PlaySound( SoundID.Item12, this.npc.position );
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
				if( this.npc.ai[2] < -0.1f ) {
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

		public override string TownNPCName() {
			return WayfarerTownNPC.PossibleNames[ Main.rand.Next(WayfarerTownNPC.PossibleNames.Count) ];
		}

		public override bool UsesPartyHat() {
			return false;   // :(
		}


		////////////////

		public override string GetChat() {
			return WayfarerTownNPC.ChatReplies[ Main.rand.Next( WayfarerTownNPC.ChatReplies.Count ) ];
		}


		////////////////
		
		public override void SetChatButtons( ref string button1, ref string button2 ) {
			var mymod = (RewardsMod)this.mod;
			int item_count = this.CountShopItems();
			bool has_button2 = false;

			button1 = "Shop";

			if( mymod.Config.ShopLoadout.Count > 40 ) {
				has_button2 = this.CountShopItems() > 40;
			}

			if( has_button2 ) {
				int shops = (int)Math.Ceiling( (float)item_count / 40f );
				int next_shop = (WayfarerTownNPC.CurrentShop + 1) >= shops ? 0 : ( WayfarerTownNPC.CurrentShop + 1);

				button2 = "Scroll to shop "+(next_shop+1)+" of "+shops;
			}
		}

		public override void OnChatButtonClicked( bool first_button, ref bool shop ) {
			if( first_button ) {
				shop = first_button;
			} else {
				int item_count = this.CountShopItems();
				int shops = (int)Math.Ceiling( (float)item_count / 40f );

				if( shops >= 1 ) {
					WayfarerTownNPC.CurrentShop = ( WayfarerTownNPC.CurrentShop + 1) >= shops ? 0 : ( WayfarerTownNPC.CurrentShop + 1);
				}
			}
		}


		////////////////

		public override void SetupShop( Chest shop, ref int next_slot ) {
			var mymod = (RewardsMod)this.mod;
			int shop_start = WayfarerTownNPC.CurrentShop * 40;
			
			for( int i = shop_start; i < mymod.Config.ShopLoadout.Count; i++ ) {
				if( next_slot >= 40 ) {
					break;
				}

				ShopPackDefinition def = mymod.Config.ShopLoadout[i];
				string fail;

				if( !def.Validate(out fail) ) {
					Main.NewText( "Could not load shop item " + def.Name + " ("+fail+")", Color.Red );
					continue;
				}
				if( !def.RequirementsMet() ) {
					continue;
				}
				
				shop.item[ next_slot++ ] = ShopPackItem.CreateItem( def );
			}
		}


		////////////////

		public int CountShopItems() {
			var mymod = (RewardsMod)this.mod;
			int count = 0;

			string _;
			foreach( ShopPackDefinition def in mymod.Config.ShopLoadout ) {
				if( def.Validate( out _ ) && def.RequirementsMet() ) {
					count++;
				}
			}

			return count;
		}
	}
}