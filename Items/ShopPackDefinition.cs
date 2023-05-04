using Microsoft.Xna.Framework;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using NetSerializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Rewards.Items {
	public sealed class ShopPackDefinitionSerializer : IPacketSerialzer<ShopPackDefinition> {
		IEnumerable<Type> ITypeSerializer.GetSubtypes( Type type ) {
			yield return typeof( string );
			yield return typeof( int );
			yield return typeof( ShopPackItemDefinition );
		}

		public void Serialize( Serializer serializer, Stream stream, ShopPackDefinition obj ) {
			serializer.Serialize( stream, obj.NeededBossKill.ToString() );
			serializer.Serialize( stream, obj.Name );
			serializer.Serialize( stream, obj.Price );
			serializer.Serialize( stream, obj.Items );
		}

		public void Deserialize( Serializer serializer, Stream stream, out ShopPackDefinition obj ) {
			obj = new(
				NPCDefinition.FromString( (string)serializer.Deserialize( stream ) ),
				(string)serializer.Deserialize( stream ),
				(int)serializer.Deserialize( stream ),
				(List<ShopPackItemDefinition>)serializer.Deserialize( stream )
			);
		}
	}

	[Serializable]
	public class ShopPackDefinition {
		public static Item[] OpenPack( Player player, ShopPackDefinition packDef ) {
			Item[] packItems = new Item[ packDef.Items.Count ];
			int i = 0;

			var itemSource = new EntitySource_ItemOpen ( player, ModContent.ItemType<ShopPackItem>() );

			foreach( ShopPackItemDefinition itemInfo in packDef.Items ) {
				if( !itemInfo.IsAvailable() ) { continue; }

				int itemType = itemInfo.ItemDef.Type;
				Item newItem = new Item();
				newItem.SetDefaults( itemType );

				Item.NewItem ( itemSource, player.position, itemType, itemInfo.Stack, newItem.width, newItem.height );

				packItems[i++] = newItem;
			}

			SoundEngine.PlaySound( SoundID.Coins );

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

		public NPCDefinition NeededBossKill { get; set; }

		public string Name { get; set; }

		[Range( 0, 10000 )]
		public int Price { get; set; }

		public List<ShopPackItemDefinition> Items { get; set; } = new List<ShopPackItemDefinition>();



		////////////////

		public ShopPackDefinition() { }

		internal ShopPackDefinition( ShopPackDefinition clone ) {
			var itemList = new List<ShopPackItemDefinition>();

			if( clone.Items == null ) {
				LogLibraries.WarnOnce( "No pack item definitions present for pack "+clone.Name );
			} else {
				foreach( ShopPackItemDefinition packItemDef in clone.Items ) {
					itemList.Add( new ShopPackItemDefinition( packItemDef ) );
				}
			}

			this.NeededBossKill = clone.NeededBossKill != null ?
				new NPCDefinition( clone.NeededBossKill.Mod, clone.NeededBossKill.Name ) :
				null;
			this.Name = clone.Name;
			this.Price = clone.Price;
			this.Items = itemList;
		}

		public ShopPackDefinition( NPCDefinition neededBoss, string name, int price, List<ShopPackItemDefinition> items ) {
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
			if( this.Items.Count == 0 ) {
				error = "no items";
				return false;
			}
			//foreach( ShopPackItemDefinition itemInfo in this.Items ) {
			//	if( !itemInfo.Validate() ) {
			//		error = itemInfo.ItemDef;
			//		return false;
			//	}
			//}
			error = null;
			return true;
		}

		public bool IsSameAs( ShopPackDefinition def ) {
			int count = this.Items.Count;

			if( !def.Name.Equals(this.Name) ) { return false; }
			if( def.Price != this.Price ) { return false; }
			if( !def.NeededBossKill?.Equals(this.NeededBossKill) ?? this.NeededBossKill != null ) { return false; }
			if( def.Items.Count != count ) { return false; }
			
			for( int i=0; i<count; i++ ) {
				if( !this.Items[i].IsSameAs( def.Items[i] ) ) { return false; }
			}
			return true;
		}

		
		public bool RequirementsMet() {
			var myworld = ModContent.GetInstance<RewardsSystem>();

			if( this.NeededBossKill == null ) {
				return true;
			}
			if( myworld.Logic.WorldData.GetKillsOfNpc(this.NeededBossKill.Type) > 0 ) {
				return true;
			}

			return false;
		}
	}
}
