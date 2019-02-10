using HamstarHelpers.Services.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		public void AddRewardForPlayer( Player player, bool isGrind, bool isExpired, float reward ) {
			if( isExpired ) {
				return;
			}

			var mymod = RewardsMod.Instance;

			if( isGrind ) {
				reward *= mymod.Config.GrindKillMultiplier;
			}

			if( Main.netMode != 2 && mymod.Config.ShowPointsPopups ) {
				if( Math.Abs(reward) >= 0.01f ) {
					string msg = "+" + Math.Round( reward, 2 ) + " PP";
					Color color = reward > 0 ?
						isGrind ? Color.DarkGray : Color.GreenYellow :
						Color.Red;

					PlayerMessages.AddPlayerLabel( player, msg, color, 60 * 3, true, false );
				}
			}

			this.ProgressPoints += reward;

			foreach( var hook in mymod.OnPointsGainedHooks ) {
				hook( player, reward );
			}
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

		public bool CanDrawPoints() {
			var mymod = RewardsMod.Instance;

			if( !mymod.Config.ShowPoints ) { return false; }
			if( !mymod.Config.PointsDisplayWithoutInventory ) {
				return Main.playerInventory;
			}
			return true;
		}


		public void DrawPointScore( SpriteBatch sb ) {
			var mymod = RewardsMod.Instance;
			float posX = mymod.Config.PointsDisplayX;
			float posY = mymod.Config.PointsDisplayY;
			posX = posX < 0 ? Main.screenWidth + posX : posX;
			posY = posY < 0 ? Main.screenHeight + posY : posY;
			Vector2 pos = new Vector2( posX, posY );

			string ppStr = "PP: " + (int)this.ProgressPoints;
			Color color = mymod.Config.PointsDisplayColor;

			//sb.DrawString( Main.fontMouseText, "PP: " + (int)this.ProgressPoints, pos, mymod.Config.PointsDisplayColor );
			Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, ppStr, pos.X, pos.Y, color, Color.Black, default( Vector2 ), 1f );

			if( Main.mouseX >= posX && Main.mouseY >= posY && Main.mouseX < (posX + 40) && Main.mouseY < (posY + 16) ) {
				var mousePos = new Vector2( Main.mouseX - 160, Main.mouseY + 10 );
				sb.DrawString( Main.fontMouseText, "Progress Points (see Wayfarer)", mousePos, Color.White, 0f, default(Vector2), 0.75f, SpriteEffects.None, 1f );
			}
		}
	}
}
