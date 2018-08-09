using HamstarHelpers.Helpers.NPCHelpers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		public void Update() {
			VanillaInvasionType inv_which = NPCInvasionHelpers.GetInvasionType( Main.invasionType );

			if( this.CurrentEvents.ContainsKey( VanillaInvasionType.Goblins ) ) {
				if( inv_which != VanillaInvasionType.Goblins ) {
					this.CurrentEvents.Remove( VanillaInvasionType.Goblins );
					this.GoblinsConquered++;
				}
			}
			if( this.CurrentEvents.ContainsKey( VanillaInvasionType.FrostLegion ) ) {
				if( inv_which != VanillaInvasionType.FrostLegion ) {
					this.CurrentEvents.Remove( VanillaInvasionType.FrostLegion );
					this.FrostLegionConquered++;
				}
			}
			if( this.CurrentEvents.ContainsKey( VanillaInvasionType.Pirates ) ) {
				if( inv_which != VanillaInvasionType.Pirates ) {
					this.CurrentEvents.Remove( VanillaInvasionType.Pirates );
					this.PiratesConquered++;
				}
			}
			if( this.CurrentEvents.ContainsKey( VanillaInvasionType.Martians ) ) {
				if( inv_which != VanillaInvasionType.Martians ) {
					this.CurrentEvents.Remove( VanillaInvasionType.Martians );
					this.MartiansConquered++;
				}
			}
			if( this.CurrentEvents.ContainsKey( VanillaInvasionType.PumpkinMoon ) ) {
				if( Main.pumpkinMoon ) {
					if( NPC.waveNumber > this.PumpkinMoonWavesConquered ) {
						this.PumpkinMoonWavesConquered = NPC.waveNumber;
					}
				} else {
					this.CurrentEvents.Remove( VanillaInvasionType.PumpkinMoon );
				}
			}
			if( this.CurrentEvents.ContainsKey( VanillaInvasionType.FrostMoon ) ) {
				if( Main.snowMoon ) {
					if( NPC.waveNumber > this.FrostMoonWavesConquered ) {
						this.FrostMoonWavesConquered = NPC.waveNumber;
					}
				} else {
					this.CurrentEvents.Remove( VanillaInvasionType.FrostMoon );
				}
			}
			
			this.CurrentEvents = new ConcurrentDictionary<VanillaInvasionType, byte>(
				NPCInvasionHelpers.GetCurrentEventTypes().Select( t => new KeyValuePair<VanillaInvasionType, byte>(t, 0) )
			);
		}
	}
}
