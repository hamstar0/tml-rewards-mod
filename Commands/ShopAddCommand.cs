using HamstarHelpers.Helpers.TmlHelpers.CommandsHelpers;
using Microsoft.Xna.Framework;
using Rewards.Items;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.Commands {
	class ShopAddCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "rew-shop-add"; } }
		public override string Usage { get { return "/"+this.Command+" \"My Pack\" 35 3521:1 1324:10"; } }
		public override string Description { get { return "Adds an item pack to the Wayfarer's shop."+
			"\n   Parameters: <quote-wrapped pack name> <PP cost> <item id>:<stack quantity> ..."+
			"\n   Tip: Use this to find out an item's id:  /hhgetitemid \"Gold Pickaxe\""; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (RewardsMod)this.mod;
			if( !mymod.Config.DebugModeEnableCheats ) {
				throw new UsageException( "Cheat mode not enabled." );
			}

			if( args.Length < 3 ) {
				throw new UsageException( "Insufficient arguments." );
			}

			if( args[0].Length == 0 || args[0][0] != '\"' ) {
				throw new UsageException( "Invalid pack name." );
			}

			int nextArgIdx = 0;
			string packName = CommandsHelpers.GetQuotedStringFromArgsAt( args, 0, out nextArgIdx );
			if( nextArgIdx == -1 ) {
				throw new UsageException( "Invalid pack name." );
			}

			int price;
			if( !int.TryParse( args[nextArgIdx++], out price ) ) {
				throw new UsageException( args[nextArgIdx-1] + " is not an integer" );
			}

			IList<ShopPackItemDefinition> itemDefs = new List<ShopPackItemDefinition>();

			for( int i=nextArgIdx; i<args.Length; i++ ) {
				string[] intSegs = args[i].Split( ':' );
				if( intSegs.Length != 2 ) {
					throw new UsageException( args[i] + " is not formatted as <item id>:<stack size>" );
				}

				int itemId;
				if( !int.TryParse( intSegs[0], out itemId ) ) {
					throw new UsageException( args[i] + " is not formated correctly as <item id>:<stack size>" );
				}
				if( itemId < 0 || itemId >= Main.itemTexture.Length ) {
					throw new UsageException( args[i] + " is an invalid item type" );
				}

				int stackSize;
				if( !int.TryParse( intSegs[1], out stackSize ) ) {
					throw new UsageException( args[i] + " is not formated correctly as <item id>:<stack size>" );
				}

				Item newItem = new Item();
				newItem.SetDefaults( itemId );

				var itemDef = new ShopPackItemDefinition( newItem.Name, stackSize );
				if( !itemDef.Validate() ) {
					throw new UsageException( args[i] + " is an invalid item definition" );
				}

				itemDefs.Add( itemDef );
			}

			var def = new ShopPackDefinition( "", packName, price, itemDefs.ToArray() );
			string fail;

			if( !def.Validate(out fail) ) {
				throw new UsageException( "Invalid pack definition (" + fail + ")" );
			}

			RewardsAPI.ShopAddPack( def );
			
			mymod.ConfigJson.SaveFile();

			caller.Reply( "Pack "+packName+" added successfully.", Color.LimeGreen );
		}
	}
}
