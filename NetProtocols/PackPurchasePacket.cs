using System.Collections.Generic;
using System.IO;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.ManualPackets;
using Rewards.Items;
using Rewards.Logic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Rewards.NetProtocols {
	public sealed class PackPurchasePacket : ManualPacket {
		public PackPurchasePacket( ShopPackDefinition pack ) {
			this.Writer.Write( pack.Name );
			this.Writer.Write7BitEncodedInt( pack.Price );
			this.Writer.Write( pack.NeededBossKill != null );

			if( pack.NeededBossKill != null ) {
				this.Writer.Write( pack.NeededBossKill.ToString() );
			}

			this.Writer.Write7BitEncodedInt( pack.Items.Count );

			foreach( var item in pack.Items ) {
				this.Writer.Write( item.ItemDef.ToString() );
				this.Writer.Write7BitEncodedInt( item.Stack );
				this.Writer.Write( (byte)(item.CrimsonWorldOnly switch { false => 0, true => 1, _ => 2 }) );
			}
		}

		public override void Read( BinaryReader reader, int sender ) {
			string name = reader.ReadString();
			int price = reader.Read7BitEncodedInt();
			var neededBossKill = reader.ReadBoolean() ? NPCDefinition.FromString(reader.ReadString()) : null;
			int numItems = reader.Read7BitEncodedInt();
			var items = new List<ShopPackItemDefinition>(numItems);

			for( int i = 0; i < numItems; i++ ) {
				var item = new ShopPackItemDefinition(
					ItemDefinition.FromString(reader.ReadString()),
					reader.Read7BitEncodedInt(),
					reader.ReadByte() switch { 0 => false, 1 => true, _ => null }
				);

				items.Add( item );
			}

			var player = Main.netMode == NetmodeID.Server ? Main.player[sender] : Main.LocalPlayer;
			var shopPack = new ShopPackDefinition( neededBossKill, name, price, items );

			this.ReceiveMe( player, shopPack );
		}


		private void ReceiveMe( Player player, ShopPackDefinition shopPack ) {
			var mymod = RewardsMod.Instance;
			var myworld = ModContent.GetInstance<RewardsSystem>();

			KillData data = myworld.Logic.GetPlayerData( player );
			if( data == null ) {
				LogLibraries.Warn( "No player data for " + player.name );
				return;
			}

			if( !data.Spend( (int)shopPack.Price, player ) ) {
				LogLibraries.Warn( "Not enough PP. PP out of sync." );
				//return;	// TODO: Add validation of purchases
			}

			Item[] items = ShopPackDefinition.OpenPack( player, shopPack );

			foreach( var hook in RewardsMod.Instance.OnPointsSpentHooks ) {
				hook( player, shopPack.Name, shopPack.Price, items );
			}

			if( mymod.SettingsConfig.DebugModeInfo ) {
				LogLibraries.Alert( "Purchase made for "+player.name+" of "+shopPack.Name+" ("+shopPack.Price+")" );
			}
		}
	}
}
