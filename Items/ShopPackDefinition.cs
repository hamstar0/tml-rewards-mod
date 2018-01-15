using HamstarHelpers.ItemHelpers;
using Terraria;


namespace Rewards.Items {
	public struct ShopPackDefinition {
		public bool PostKingSlime;
		public bool PostEyeOfCthulhu;
		public bool PostGoblins;
		public bool PostQueenBee;
		public bool PostEvilBiome;
		public bool PostSkeletron;
		public bool PostWof;
		public bool PostDestroyer;
		public bool PostTwins;
		public bool PostSkeletronPrime;
		public bool PostPlantera;
		public bool PostGolem;
		public bool PostMoonLord;

		public string Name;
		public int Price;
		public ShopPackItemDefinition[] Items;


		////////////////

		internal ShopPackDefinition(
				bool post_king_slime, bool post_eye, bool post_goblin, bool post_queen, bool post_evil, bool post_skele, bool post_wof,
				bool post_destroyer, bool post_twins, bool post_skele_prime, bool post_plantera, bool post_golem, bool post_moonlord,
				string name, int price, ShopPackItemDefinition[] items ) {
			this.Name = name;
			this.Price = price;
			this.Items = items;

			this.PostKingSlime = post_king_slime;
			this.PostEyeOfCthulhu = post_eye;
			this.PostGoblins = post_goblin;
			this.PostQueenBee = post_queen;
			this.PostEvilBiome = post_evil;
			this.PostSkeletron = post_skele;
			this.PostWof = post_wof;
			this.PostDestroyer = post_destroyer;
			this.PostTwins = post_twins;
			this.PostSkeletronPrime = post_skele_prime;
			this.PostPlantera = post_plantera;
			this.PostGolem = post_golem;
			this.PostMoonLord = post_moonlord;
		}

		internal ShopPackDefinition( string name, int price, ShopPackItemDefinition[] items ) {
			this.Name = name;
			this.Price = price;
			this.Items = items;

			this.PostKingSlime = false;
			this.PostEyeOfCthulhu = false;
			this.PostGoblins = false;
			this.PostQueenBee = false;
			this.PostEvilBiome = false;
			this.PostSkeletron = false;
			this.PostWof = false;
			this.PostDestroyer = false;
			this.PostTwins = false;
			this.PostSkeletronPrime = false;
			this.PostPlantera = false;
			this.PostGolem = false;
			this.PostMoonLord = false;
		}

		////////////////

		public bool Validate() {
			if( string.IsNullOrEmpty( this.Name ) || this.Items.Length == 0 ) { return false; }
			foreach( ShopPackItemDefinition item_info in this.Items ) {
				if( !item_info.Validate() ) { return false; }
			}
			return true;
		}

		public bool IsSameAs( ShopPackDefinition def ) {
			if( !def.Name.Equals(this.Name) ) { return false; }
			if( def.Price != this.Price ) { return false; }
			if( def.PostKingSlime != this.PostKingSlime ) { return false; }
			if( def.PostEyeOfCthulhu != this.PostEyeOfCthulhu ) { return false; }
			if( def.PostQueenBee != this.PostQueenBee ) { return false; }
			if( def.PostEvilBiome != this.PostEvilBiome ) { return false; }
			if( def.PostSkeletron != this.PostSkeletron ) { return false; }
			if( def.PostWof != this.PostWof ) { return false; }
			if( def.PostDestroyer != this.PostDestroyer ) { return false; }
			if( def.PostTwins != this.PostTwins ) { return false; }
			if( def.PostSkeletronPrime != this.PostSkeletronPrime ) { return false; }
			if( def.PostPlantera != this.PostPlantera ) { return false; }
			if( def.PostGolem != this.PostGolem ) { return false; }
			if( def.PostMoonLord != this.PostMoonLord ) { return false; }
			if( def.Items.Length != this.Items.Length ) { return false; }
			for( int i=0; i<this.Items.Length; i++ ) {
				if( this.Items[i].IsSameAs( def.Items[i] ) ) { return false; }
			}
			return true;
		}

		
		public bool IsValidMode() {
			if( this.PostKingSlime && !NPC.downedSlimeKing ) { return false; }
			if( this.PostEyeOfCthulhu && !NPC.downedBoss1 ) { return false; }
			if( this.PostEvilBiome && !NPC.downedBoss2 ) { return false; }
			if( this.PostSkeletron && !NPC.downedBoss3 ) { return false; }
			if( this.PostQueenBee && !NPC.downedQueenBee ) { return false; }
			if( this.PostWof && !Main.hardMode ) { return false; }
			if( this.PostDestroyer && !NPC.downedMechBoss1 ) { return false; }
			if( this.PostTwins && !NPC.downedMechBoss2 ) { return false; }
			if( this.PostSkeletronPrime && !NPC.downedMechBoss3 ) { return false; }
			if( this.PostPlantera && !NPC.downedPlantBoss ) { return false; }
			if( this.PostGolem && !NPC.downedGolemBoss ) { return false; }
			if( this.PostMoonLord && !NPC.downedMoonlord ) { return false; }
			return true;
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
					if( ItemIdentityHelpers.NamesToIds.ContainsKey( this.Name ) ) {
						this._ItemType = ItemIdentityHelpers.NamesToIds[this.Name];
					}
				}
				return this._ItemType;
			}
		}


		////////////////

		internal ShopPackItemDefinition( string name, int stack, bool? crimson_only = null ) {
			this.Name = name;
			this.Stack = stack;
			this.CrimsonWorldOnly = crimson_only;
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

		public bool IsSameAs( ShopPackItemDefinition item_def ) {
			if( this.Name.Equals( item_def.Name ) ) { return false; }
			if( this.Stack != item_def.Stack ) { return false; }
			if( this.CrimsonWorldOnly != item_def.CrimsonWorldOnly ) { return false; }
			return true;
		}
	}
}
