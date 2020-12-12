using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Messages.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		public float AddRewardForPlayer( Player player, bool isGrind, bool isExpired, float reward ) {
			if( isExpired ) {
				return 0f;
			}

			var mymod = RewardsMod.Instance;
			float finalReward = reward;
			float oldPP = this.ProgressPoints;

			if( isGrind ) {
				finalReward *= mymod.PointsConfig.GrindKillMultiplier;
			}

			if( Main.netMode != 2 && mymod.SettingsConfig.ShowPointsPopups ) {
				Color color = isGrind ? Color.DarkGray : Color.GreenYellow;

				if( Math.Abs(finalReward) >= 0.01f ) {
					string msg = "+" + Math.Round( finalReward, 2 ) + " PP";
					PlayerMessages.AddPlayerLabel( player, msg, color, 60 * 3, true, false );
				} else if( Math.Abs( finalReward ) > 0 ) {
					PlayerMessages.AddPlayerLabel( player, "+PP", color, 60, true, false );
				}
			}

			this.ProgressPoints += finalReward;

			foreach( var hook in mymod.OnPointsGainedHooks ) {
				hook( player, finalReward );
			}

			if( mymod.SettingsConfig.DebugModeKillInfo ) {
				LogHelpers.Log( "  AddRewardForPlayer - Player: " + player.name + " ("+player.whoAmI+ ")"
					+ ", reward: " + finalReward + ((finalReward != reward) ? (" (was " + reward + ")") : "" )
					+ ", isGrind:" + isGrind + ", isExpired:" + isExpired
					+ ", PP: " + this.ProgressPoints + " (was " + oldPP + ")"
				);
			}
			if( mymod.SettingsConfig.DebugModePPInfo && finalReward != 0 ) {
				LogHelpers.Alert( "PP added: " + finalReward + " (now "+this.ProgressPoints
					+" for " + ( player?.name ?? "world" ) + ")" );
			}

			return finalReward;
		}


		////////////////

		public bool Spend( int points, Player forPlayer ) {
			if( this.ProgressPoints < points ) {
				return false;
			}

			var mymod = RewardsMod.Instance;
			
			this.ProgressPoints -= points;
			
			if( mymod.SettingsConfig.DebugModePPInfo && points != 0 ) {
				LogHelpers.Alert( "PP spent: " + points + " (now "+this.ProgressPoints
					+" for " + ( forPlayer.name ) + ")" );
			}

			return true;
		}


		////////////////

		public bool CanDrawPoints() {
			var mymod = RewardsMod.Instance;

			if( !mymod.SettingsConfig.ShowPoints ) { return false; }
			if( !mymod.SettingsConfig.PointsDisplayWithoutInventory ) {
				return Main.playerInventory;
			}
			return true;
		}


		public void DrawPointScore( SpriteBatch sb ) {
			var mymod = RewardsMod.Instance;
			float posX = mymod.SettingsConfig.PointsDisplayX;
			float posY = mymod.SettingsConfig.PointsDisplayY;
			posX = posX < 0 ? Main.screenWidth + posX : posX;
			posY = posY < 0 ? Main.screenHeight + posY : posY;
			Vector2 pos = new Vector2( posX, posY );

			string ppStr = "PP: " + (int)this.ProgressPoints;

			byte[] c = mymod.SettingsConfig.PointsDisplayColorRGB;
			var color = new Color( c[0], c[1], c[2] );

			//sb.DrawString( Main.fontMouseText, "PP: " + (int)this.ProgressPoints, pos, mymod.Config.PointsDisplayColor );
			Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, ppStr, pos.X, pos.Y, color, Color.Black, default( Vector2 ), 1f );

			if( Main.mouseX >= posX && Main.mouseY >= posY && Main.mouseX < (posX + 40) && Main.mouseY < (posY + 16) ) {
				var mousePos = new Vector2( Main.mouseX - 160, Main.mouseY + 10 );
				sb.DrawString( Main.fontMouseText, "Progress Points (see Wayfarer)", mousePos, Color.White, 0f, default(Vector2), 0.75f, SpriteEffects.None, 1f );
			}
		}
	}
}
