using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.ModLoader.IO;


namespace Rewards.Logic {
	class PlayerLogic {
		public float ProgressPoints = 0f;



		////////////////

		public void Load( TagCompound tags ) {
			if( tags.ContainsKey("pp") ) {
				this.ProgressPoints = tags.GetFloat( "pp" );
			}
		}

		public TagCompound Save() {
			return new TagCompound { { "pp", this.ProgressPoints } };
		}


		////////////////

		public void AddKillReward( RewardsMod mymod, Player player, int npc_type, bool is_grind, float reward ) {
			if( Math.Abs(reward) > 0.01f ) {
				string msg = "+" + Math.Round( reward, 2 ) + " PP";
				Color color = reward > 0 ?
					is_grind ? Color.DarkGray : Color.GreenYellow :
					Color.Red;

				PlayerMessages.AddPlayerLabel( player, msg, color, 60 * 3, true, false );
			}

			this.ProgressPoints += reward;
		}


		////////////////

		public bool Spend( int points ) {
			if( this.ProgressPoints < points ) {
				return false;
			}

			this.ProgressPoints -= points;

			return true;
		}


		////////////////

		public void DrawPointScore( RewardsMod mymod, SpriteBatch sb ) {
			if( !mymod.Config.PointsDisplayWithoutInventory && Main.playerInventory ) { return; }

			float pos_x = mymod.Config.PointsDisplayX;
			float pos_y = mymod.Config.PointsDisplayY;
			pos_x = pos_x < 0 ? Main.screenWidth + pos_x : pos_x;
			pos_y = pos_y < 0 ? Main.screenHeight + pos_y : pos_y;

			sb.DrawString( Main.fontMouseText, "PP: " + (int)this.ProgressPoints, new Vector2( pos_x, pos_y ), mymod.Config.PointsDisplayColor );
		}
	}
}
