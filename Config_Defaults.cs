﻿using HamstarHelpers.Utilities.Config;
using Rewards.Items;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace Rewards {
	public partial class RewardsConfigData : ConfigurationDataBase {
		public override void OnLoad( bool success ) {
			if( RewardsMod.Instance.IsContentSetup ) {
				string fail;

				foreach( var info in this.ShopLoadout ) {
					if( !info.Validate(out fail) ) {
						ErrorLogger.Log( "Could not validate shop item " + info.Name + " (" + fail + ")" );
					}
				}
			}
			
			if( !success ) {
				this.SetDefaults();
			}
		}


		public bool SetDefaults() {
			this.NpcRewards = new Dictionary<string, float> {
				{ "King Slime", 10f },
				{ "Eye of Cthulhu", 10f },
				{ "Eater of Worlds", 25f / 50f },	// per segment
				{ "Brain of Cthulhu", 25f },
				{ "Queen Bee", 20f },
				{ "Skeletron", 35f },
				{ "Wall of Flesh", 75f },
				{ "The Destroyer", 50f },
				{ "Retinazer", 50f / 2 },
				{ "Spazmatism", 50f / 2 },
				{ "Skeletron Prime", 50f },
				{ "Plantera", 100f },
				{ "Golem", 100f },
				{ "Duke Fishron", 100f },
				{ "Lunatic Cultist", 50f },
				{ "Betsy", 100f },
				{ "Solar Pillar", 50f },
				{ "Vortex Pillar", 50f },
				{ "Nebula Pillar", 50f },
				{ "Stardust Pillar", 50f },
				{ "Moon Lord", 250f }
			};
			this.NpcRewardRequiredMinimumKills = new Dictionary<string, int> {
				{ "Eater of Worlds", 50 }
			};

			this.ShopLoadout = new List<ShopPackDefinition> {
				new ShopPackDefinition( "", "Money Purse", 5, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Gold Coin", 99 )
					} ),
				new ShopPackDefinition( "", "Iron Age Pack", 5, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Iron Bar", 99 )
					} ),
				new ShopPackDefinition( "", "Nature Watcher's Pack", 10, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Flower Boots", 1 ),
						new ShopPackItemDefinition( "Binoculars", 1 )
					} ),
				new ShopPackDefinition( "", "Bindings Pack", 10, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Band of Regeneration", 1 ),
						new ShopPackItemDefinition( "Band of Starpower", 1 ),
						new ShopPackItemDefinition( "Celestial Magnet", 1 )
					} ),
				new ShopPackDefinition( "", "Traveler's Pack", 20, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Aglet", 1 ),
						new ShopPackItemDefinition( "Ice Skates", 1 ),
						new ShopPackItemDefinition( "Tiger Climbing Gear", 1 ),
						new ShopPackItemDefinition( "Water Walking Boots", 1 ),
						new ShopPackItemDefinition( "Hand Warmer", 1 )
					} ),
				new ShopPackDefinition( "", "Rough Traveler's Pack", 20, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Anklet of the Wind", 1 ),
						new ShopPackItemDefinition( "Feral Claws", 1 ),
						new ShopPackItemDefinition( "Jellyfish Diving Gear", 1 ),
						new ShopPackItemDefinition( "Lava Charm", 1 ),
						new ShopPackItemDefinition( "Magma Stone", 1 )
					} ),
				new ShopPackDefinition( "", "Ground Hater's Pack", 15, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Sandstorm in a Bottle", 1 ),
						new ShopPackItemDefinition( "Shiny Red Balloon", 1 ),
						new ShopPackItemDefinition( "Lucky Horseshoe", 1 )
					} ),
				new ShopPackDefinition( "", "Stoicist's Pack", 15, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Cobalt Shield", 1 )
					} ),
				new ShopPackDefinition( "", "Alucard's Pack", 20, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Arkhalis", 1 )
					} ),
				new ShopPackDefinition( "", "Gizmo Pack", 20, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Toolbox", 1 ),
						new ShopPackItemDefinition( "Architect Gizmo Pack", 1 ),
						new ShopPackItemDefinition( "Presserator", 1 )
					} ),
				new ShopPackDefinition( "", "Information Monger's Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "GPS", 1 ),
						new ShopPackItemDefinition( "Goblin Tech", 1 ),
						new ShopPackItemDefinition( "R.E.K. 3000", 1 )
					} ),
				new ShopPackDefinition( "", "Fisherman's Pack", 50, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Angler Tackle Bag", 1 ),
						new ShopPackItemDefinition( "Fish Finder", 1 )
					} ),

				// Hard mode only:
				new ShopPackDefinition( "Wall of Flesh", "Mimic's Lament Pack", 20, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Magic Dagger", 1 ),
						new ShopPackItemDefinition( "Titan Glove", 1 ),
						new ShopPackItemDefinition( "Philosopher's Stone", 1 ),
						new ShopPackItemDefinition( "Cross Necklace", 1 ),
						new ShopPackItemDefinition( "Star Cloak", 1 )
					} ),
				new ShopPackDefinition( "Wall of Flesh", "Gangster's Pack", 35, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Uzi", 1 ),
						new ShopPackItemDefinition( "Rifle Scope", 1 ),
						new ShopPackItemDefinition( "Endless Musket Pouch", 1 ),
						new ShopPackItemDefinition( "Gangsta Hat", 1 )
					} ),
				new ShopPackDefinition( "Wall of Flesh", "Avenger Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Avenger Emblem", 1 )
					} ),
				new ShopPackDefinition( "Wall of Flesh", "Life Pack", 75, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Ankh Charm", 1 )
					} ),
				new ShopPackDefinition( "Wall of Flesh", "Lucky Pack", 100, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Coin Ring", 1 )
					} ),
				new ShopPackDefinition( "Wall of Flesh", "Dimensionalist's Pack", 100, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Rod of Discord", 1 ),
						new ShopPackItemDefinition( "Teleportation Potion", 30 )
					} ),

				// Post-plantera:
				new ShopPackDefinition( "Plantera", "Whack Pack", 45, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "The Axe", 1 ),
						new ShopPackItemDefinition( "Bananarang", 10 ),
						new ShopPackItemDefinition( "Slap Hand", 1 )
					} ),
				new ShopPackDefinition( "Plantera", "Defender's Pack", 60, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Celestial Shell", 1 ),
						new ShopPackItemDefinition( "Paladin's Shield", 1 ),
						new ShopPackItemDefinition( "Frozen Turtle Shell", 1 )
					} ),

				// Post-golem:
				new ShopPackDefinition( "Golem", "Golem Eye Pack", 25, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Eye of the Golem", 1 )
					} ),

				// Post-moonlord:
				new ShopPackDefinition( "Moon Lord", "Eldritch Pack", 300, new ShopPackItemDefinition[] {
						new ShopPackItemDefinition( "Meowmere", 1 ),
						new ShopPackItemDefinition( "Terrarian", 1 ),
						new ShopPackItemDefinition( "Star Wrath", 1 ),
						new ShopPackItemDefinition( "S.D.M.G.", 1 ),
						new ShopPackItemDefinition( "Celebration", 1 ),
						new ShopPackItemDefinition( "Last Prism", 1 ),
						new ShopPackItemDefinition( "Lunar Flare", 1 ),
						new ShopPackItemDefinition( "Rainbow Crystal Staff", 1 ),
						new ShopPackItemDefinition( "Lunar Portal Staff", 1 )
					} )
			};
			/*this.ShopLoadout = new List<ShopPackDefinition> {
				new ShopPackDefinition( "Money Purse", 10, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Gold Coin", 99 )
				} ),
				new ShopPackDefinition( "Iron Age Pack", 10, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Iron Bar", 99 )
				} ),
				new ShopPackDefinition( "Life Pack", 15, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Life Crystal", 10 ),
					new ShopPackItemDefinition( "Healing Potion", 30 )
				} ),
				new ShopPackDefinition( "Evil Essence Pack", 25, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Tissue Sample", 99, true ),
					new ShopPackItemDefinition( "Shadow Orb", 99, false )
				} ),
				new ShopPackDefinition( "Traveler's Pack", 35, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Spectre Boots", 1 ),
					new ShopPackItemDefinition( "Ivy Whip", 1 ),
					new ShopPackItemDefinition( "Obsidian Shield", 1 ),
					new ShopPackItemDefinition( "Blue Horseshoe Balloon", 1 ),
					new ShopPackItemDefinition( "Javelin", 99 )
				} ),
				new ShopPackDefinition( "Monster Pack", 25, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Slime Crown", 3 ),
					new ShopPackItemDefinition( "Suspicious Looking Eye", 3 ),
					new ShopPackItemDefinition( "Abeemination", 3 ),
					new ShopPackItemDefinition( "Goblin Battle Standard", 1 ),
					new ShopPackItemDefinition( "Clothier Voodoo Doll", 1 ),
					new ShopPackItemDefinition( "Guide Voodoo Doll", 1 )
				} ),
				new ShopPackDefinition( "Fire & Brimstone Pack", 50, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Molotov Cocktail", 99 ),
					new ShopPackItemDefinition( "Hellstone Bar", 99 )
				} ),
				new ShopPackDefinition( "Jungle Delver's Pack", 100, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Pickaxe Axe", 1 ),
					new ShopPackItemDefinition( "Cutlass", 1 ),
					new ShopPackItemDefinition( "Flamethrower", 1 )
				} ),
				new ShopPackDefinition( "Cleanup Kit", 50, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Clentaminator", 1 ),
					new ShopPackItemDefinition( "Green Solution", 999 )
				} ),
				new ShopPackDefinition( "Lihzahrd Embassador Kit", 150, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Temple Key", 1 ),
					new ShopPackItemDefinition( "Life Fruit", 10 ),
					new ShopPackItemDefinition( "Venus Magnum", 1 )
				} ),
				new ShopPackDefinition( "Cultist's Pack", 150, new ShopPackItemDefinition[] {
					new ShopPackItemDefinition( "Golem Fist", 1 ),
					new ShopPackItemDefinition( "Pirate Map", 2 ),
					new ShopPackItemDefinition( "Solar Tablet", 1 ),
					new ShopPackItemDefinition( "Pumpkin Moon Medallion", 2 ),
					new ShopPackItemDefinition( "Naughty Present", 2 )
				} )
				//new ShopPackDefinition( "Time Traveler's Pack", 35, new ShopItemInfo[] {
				//	new ShopPackItemDefinition( "Time Capsule", 1 )
				//} )
			};*/

			return true;
		}
	}
}