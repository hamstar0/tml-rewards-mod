using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using ModLibsCore.Services.Network.SimplePacket;
using NetSerializer;
using Terraria;
using Terraria.ModLoader.Config;


namespace Rewards.Items {
	public sealed class ShopPackItemDefinitionSerializer : IPacketSerialzer<ShopPackItemDefinition> {
		IEnumerable<Type> ITypeSerializer.GetSubtypes( Type type ) {
			yield return typeof(int);
			yield return typeof(bool?);
			yield return typeof(string);
		}

		public void Serialize( Serializer serializer, Stream stream, ShopPackItemDefinition obj ) {
			serializer.Serialize( stream, obj.ItemDef.ToString() );
			serializer.Serialize( stream, obj.Stack );
			serializer.Serialize( stream, obj.CrimsonWorldOnly );
		}

		public void Deserialize( Serializer serializer, Stream stream, out ShopPackItemDefinition obj ) {
			obj = new(
				ItemDefinition.FromString( (string)serializer.Deserialize( stream ) ),
				(int)serializer.Deserialize( stream ),
				(bool?)serializer.Deserialize( stream )
			);
		}
	}

	[Serializable]
	public sealed class ShopPackItemDefinition {
		public ItemDefinition ItemDef { get; set; }

		[Label("Item stack size")]
		[Range(1, 9999)]
		[DefaultValue(1)]
		public int Stack { get; set; }

		public bool? CrimsonWorldOnly { get; set; }



		////////////////

		public ShopPackItemDefinition() { }

		internal ShopPackItemDefinition( ShopPackItemDefinition clone ) {
			this.ItemDef = clone.ItemDef != null ?
				new ItemDefinition( clone.ItemDef.Mod, clone.ItemDef.Name ) :
				null;
			this.Stack = clone.Stack;
			this.CrimsonWorldOnly = clone.CrimsonWorldOnly;
		}

		public ShopPackItemDefinition( ItemDefinition itemDef, int stack, bool? crimsonOnly = null ) {
			this.ItemDef = itemDef;
			this.Stack = stack;
			this.CrimsonWorldOnly = crimsonOnly;
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

		public bool IsSameAs( ShopPackItemDefinition itemDef ) {
			if( this.ItemDef == null ) { return false; }
			if( !this.ItemDef.Equals(itemDef.ItemDef) ) { return false; }
			if( this.Stack != itemDef.Stack ) { return false; }
			if( this.CrimsonWorldOnly != itemDef.CrimsonWorldOnly ) { return false; }
			return true;
		}
	}
}
