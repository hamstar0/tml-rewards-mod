using HamstarHelpers.NPCHelpers;
using HamstarHelpers.WorldHelpers;
using Terraria;


namespace Rewards.Logic {
	partial class RewardsLogic {
		public void UpdateInvasions() {
			VanillaInvasionType now_inv = NPCInvasionHelpers.GetCurrentInvasionType();

			switch( this.CurrentInvasion ) {
			case VanillaInvasionType.Goblins:
				if( Main.invasionType != (int)this.CurrentInvasion ) {
					this.CurrentInvasion = now_inv;
					this.WorldGolinsConquered[ WorldHelpers.GetUniqueId() ]++;
				}
				break;
			case VanillaInvasionType.FrostLegion:
				if( Main.invasionType != (int)this.CurrentInvasion ) {
					this.CurrentInvasion = now_inv;
					this.WorldFrostLegionConquered[ WorldHelpers.GetUniqueId() ]++;
				}
				break;
			case VanillaInvasionType.Pirates:
				if( Main.invasionType != (int)this.CurrentInvasion ) {
					this.CurrentInvasion = now_inv;
					this.WorldPiratesConquered[ WorldHelpers.GetUniqueId() ]++;
				}
				break;
			case VanillaInvasionType.Martians:
				if( Main.invasionType != (int)this.CurrentInvasion ) {
					this.CurrentInvasion = now_inv;
					this.WorldMartiansConquered[ WorldHelpers.GetUniqueId() ]++;
				}
				break;
			case VanillaInvasionType.PumpkinMoon:
				if( Main.pumpkinMoon ) {
					var world_uid = WorldHelpers.GetUniqueId();

					if( NPC.waveNumber > this.WorldPumpkinMoonWavesConquered[world_uid] ) {
						this.WorldPumpkinMoonWavesConquered[world_uid] = NPC.waveNumber;
					}
				} else {
					this.CurrentInvasion = now_inv;
				}
				break;
			case VanillaInvasionType.FrostMoon:
				if( Main.snowMoon ) {
					var world_uid = WorldHelpers.GetUniqueId();

					if( NPC.waveNumber > this.WorldFrostMoonWavesConquered[world_uid] ) {
						this.WorldFrostMoonWavesConquered[world_uid] = NPC.waveNumber;
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
