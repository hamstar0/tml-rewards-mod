using HamstarHelpers.TmlHelpers.CommandsHelpers;
using Rewards.Items;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.Commands {
	class ShopAddCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "rewardsshopadd"; } }
		public override string Usage { get { return "/rewardsshopadd \"My Pack\" 35 3521:1 1324:10"; } }
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

			int next_arg_idx = 0;
			string pack_name = CommandsHelpers.GetQuotedStringFromArgsAt( args, 0, out next_arg_idx );
			if( next_arg_idx == -1 ) {
				throw new UsageException( "Invalid pack name." );
			}

			int price;
			if( !int.TryParse( args[next_arg_idx++], out price ) ) {
				throw new UsageException( args[next_arg_idx-1] + " is not an integer" );
			}

			IList<ShopPackItemDefinition> item_defs = new List<ShopPackItemDefinition>();

			for( int i=next_arg_idx; i<args.Length; i++ ) {
				string[] int_segs = args[i].Split( ':' );
				if( int_segs.Length != 2 ) {
					throw new UsageException( args[i] + " is not formatted as <item id>:<stack size>" );
				}

				int item_id;
				if( !int.TryParse( int_segs[0], out item_id ) ) {
					throw new UsageException( args[i] + " is not formated correctly as <item id>:<stack size>" );
				}
				if( item_id < 0 || item_id >= Main.itemTexture.Length ) {
					throw new UsageException( args[i] + " is an invalid item type" );
				}

				int stack_size;
				if( !int.TryParse( int_segs[1], out stack_size ) ) {
					throw new UsageException( args[i] + " is not formated correctly as <item id>:<stack size>" );
				}

				Item new_item = new Item();
				new_item.SetDefaults( item_id );

				var item_def = new ShopPackItemDefinition( new_item.Name, stack_size );
				if( !item_def.Validate() ) {
					throw new UsageException( args[i] + " is an invalid item definition" );
				}

				item_defs.Add( item_def );
			}

			var def = new ShopPackDefinition( "", pack_name, price, item_defs.ToArray() );
			string fail;

			if( !def.Validate(out fail) ) {
				throw new UsageException( "Invalid pack definition (" + fail + ")" );
			}

			RewardsAPI.ShopAddPack( def );

			caller.Reply( "Pack "+pack_name+" added successfully." );
		}
	}
}
