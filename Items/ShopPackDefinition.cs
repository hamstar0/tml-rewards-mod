using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace Rewards.Items {
	public struct ShopPackDefinition {
		public static Item[] OpenPack( Player player, ShopPackDefinition packDef ) {
			Item[] packItems = new Item[ packDef.Items.Length ];
			int i = 0;

			foreach( ShopPackItemDefinition itemInfo in packDef.Items ) {
				if( !itemInfo.Validate() || !itemInfo.IsAvailable() ) { continue; }

				Item newItem = new Item();
				newItem.SetDefaults( itemInfo.ItemType );

				ItemHelpers.CreateItem( player.position, itemInfo.ItemType, itemInfo.Stack, newItem.width, newItem.height );

				packItems[i++] = newItem;
			}

			Main.PlaySound( SoundID.Coins );

			return packItems;
		}

		public static ShopPackDefinition[] GetValidatedLoadout( bool outputValidationErrors ) {
			var mymod = RewardsMod.Instance;
			var defs = new List<ShopPackDefinition>();

			for( int i = 0; i < mymod.ShopConfig.ShopLoadout.Count; i++ ) {
				ShopPackDefinition def = mymod.ShopConfig.ShopLoadout[i];
				string fail;

				if( !def.Validate( out fail ) ) {
					if( outputValidationErrors ) {
						Main.NewText( "Could not load shop item " + def.Name + " (" + fail + ")", Color.Red );
					}
					continue;
				}
				if( !def.RequirementsMet() ) {
					continue;
				}

				defs.Add( def );
			}

			return defs.ToArray();
		}



		////////////////

		public string NeededBossKill;
		public string Name;
		public int Price;
		public ShopPackItemDefinition[] Items;



		////////////////

		internal ShopPackDefinition( string neededBoss, string name, int price, ShopPackItemDefinition[] items ) {
			this.NeededBossKill = neededBoss;
			this.Name = name;
			this.Price = price;
			this.Items = items;
		}

		////////////////

		public bool Validate( out string error ) {
			if( string.IsNullOrEmpty( this.Name ) ) {
				error = "bad name";
				return false;
			}
			if( this.Items.Length == 0 ) {
				error = "no items";
				return false;
			}
			foreach( ShopPackItemDefinition itemInfo in this.Items ) {
				if( !itemInfo.Validate() ) {
					error = itemInfo.Name;
					return false;
				}
			}
			error = null;
			return true;
		}

		public bool IsSameAs( ShopPackDefinition def ) {
			if( !def.Name.Equals(this.Name) ) { return false; }
			if( def.Price != this.Price ) { return false; }
			if( def.NeededBossKill != this.NeededBossKill ) { return false; }
			if( def.Items.Length != this.Items.Length ) { return false; }
			for( int i=0; i<this.Items.Length; i++ ) {
				if( this.Items[i].IsSameAs( def.Items[i] ) ) { return false; }
			}
			return true;
		}

		
		public bool RequirementsMet() {
			if( string.IsNullOrEmpty(this.NeededBossKill) ) {
				return true;
			}

			ISet<int> npcTypes;
			if( !NPCIdentityHelpers.NamesToIds.TryGetValues( this.NeededBossKill, out npcTypes ) ) {
				Main.NewText( "Required kill npc " + this.NeededBossKill + " for "+this.Name+" not found." );
				LogHelpers.Log( " Required kill npc " + this.NeededBossKill + " for " + this.Name + " not found." );
				return false;
			}

			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();

			foreach( int npcType in npcTypes ) {
				if( myworld.Logic.WorldData.GetKillsOfNpc(npcType) > 0 ) {
					return true;
				}
			}
			return false;
		}
	}



	public struct ShopPackItemDefinition {
		public string Name;
		public int Stack;
		public bool? CrimsonWorldOnly;

		private int _ItemType;
		internal int ItemType {
			get {
				if( this._ItemType <= 0 ) {
					string name = this.Name == null ? "" : this.Name;

					if( ItemIdentityHelpers.NamesToIds.ContainsKey( name ) ) {
						this._ItemType = ItemIdentityHelpers.NamesToIds[ name ];
					}
				}

				return this._ItemType;
			}
		}


		////////////////

		internal ShopPackItemDefinition( string name, int stack, bool? crimsonOnly = null ) {
			this.Name = name;
			this.Stack = stack;
			this.CrimsonWorldOnly = crimsonOnly;
			this._ItemType = -1;
		}

		////////////////

		public bool IsAvailable() {
			if( this.CrimsonWorldOnly != null ) {
				if( this.CrimsonWorldOnly == false ) {
					if( WorldGen.crimson ) { return false; }
				} else {
					if( !WorldGen.crimson ) { return false; }
				}
			}
			return true;
		}

		public bool Validate() {
			return this.ItemType > 0;
		}

		public bool IsSameAs( ShopPackItemDefinition itemDef ) {
			if( this.Name.Equals( itemDef.Name ) ) { return false; }
			if( this.Stack != itemDef.Stack ) { return false; }
			if( this.CrimsonWorldOnly != itemDef.CrimsonWorldOnly ) { return false; }
			return true;
		}
	}
}
