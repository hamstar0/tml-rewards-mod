using HamstarHelpers.NPCHelpers;
using HamstarHelpers.WorldHelpers;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		public void Update() {
			VanillaInvasionType now_inv = NPCInvasionHelpers.GetCurrentInvasionType();
			VanillaInvasionType then_inv = this.CurrentInvasion;

			switch( this.CurrentInvasion ) {
			case VanillaInvasionType.Goblins:
				if( Main.invasionType != (int)this.CurrentInvasion ) {
					this.CurrentInvasion = now_inv;
					this.GoblinsConquered++;
				}
				break;
			case VanillaInvasionType.FrostLegion:
				if( Main.invasionType != (int)this.CurrentInvasion ) {
					this.CurrentInvasion = now_inv;
					this.FrostLegionConquered++;
				}
				break;
			case VanillaInvasionType.Pirates:
				if( Main.invasionType != (int)this.CurrentInvasion ) {
					this.CurrentInvasion = now_inv;
					this.PiratesConquered++;
				}
				break;
			case VanillaInvasionType.Martians:
				if( Main.invasionType != (int)this.CurrentInvasion ) {
					this.CurrentInvasion = now_inv;
					this.MartiansConquered++;
				}
				break;
			case VanillaInvasionType.PumpkinMoon:
				if( Main.pumpkinMoon ) {
					if( NPC.waveNumber > this.PumpkinMoonWavesConquered ) {
						this.PumpkinMoonWavesConquered = NPC.waveNumber;
					}
				} else {
					this.CurrentInvasion = now_inv;
				}
				break;
			case VanillaInvasionType.FrostMoon:
				if( Main.snowMoon ) {
					var world_uid = WorldHelpers.GetUniqueId();

					if( NPC.waveNumber > this.FrostMoonWavesConquered ) {
						this.FrostMoonWavesConquered = NPC.waveNumber;
					}
				} else {
					this.CurrentInvasion = now_inv;
				}
				break;

			default:
				switch( now_inv ) {
				case VanillaInvasionType.Goblins:
				case VanillaInvasionType.FrostLegion:
				case VanillaInvasionType.Pirates:
				case VanillaInvasionType.Martians:
				case VanillaInvasionType.PumpkinMoon:
				case VanillaInvasionType.FrostMoon:
					this.CurrentInvasion = now_inv;
					break;
				default:
					this.CurrentInvasion = VanillaInvasionType.None;
					break;
				}
				break;
			}
		}
	}
}
