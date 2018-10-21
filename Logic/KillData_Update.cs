using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		public void Update() {
			VanillaEventFlag inv_which = NPCInvasionHelpers.GetEventTypeOfInvasionType( Main.invasionType );

			if( this.CurrentEvents.ContainsKey( VanillaEventFlag.Goblins ) ) {
				if( inv_which != VanillaEventFlag.Goblins ) {
					this.CurrentEvents.Remove( VanillaEventFlag.Goblins );
					this.GoblinsConquered++;
				}
			}
			if( this.CurrentEvents.ContainsKey( VanillaEventFlag.FrostLegion ) ) {
				if( inv_which != VanillaEventFlag.FrostLegion ) {
					this.CurrentEvents.Remove( VanillaEventFlag.FrostLegion );
					this.FrostLegionConquered++;
				}
			}
			if( this.CurrentEvents.ContainsKey( VanillaEventFlag.Pirates ) ) {
				if( inv_which != VanillaEventFlag.Pirates ) {
					this.CurrentEvents.Remove( VanillaEventFlag.Pirates );
					this.PiratesConquered++;
				}
			}
			if( this.CurrentEvents.ContainsKey( VanillaEventFlag.Martians ) ) {
				if( inv_which != VanillaEventFlag.Martians ) {
					this.CurrentEvents.Remove( VanillaEventFlag.Martians );
					this.MartiansConquered++;
				}
			}
			if( this.CurrentEvents.ContainsKey( VanillaEventFlag.PumpkinMoon ) ) {
				if( Main.pumpkinMoon ) {
					if( NPC.waveNumber > this.PumpkinMoonWavesConquered ) {
						this.PumpkinMoonWavesConquered = NPC.waveNumber;
					}
				} else {
					this.CurrentEvents.Remove( VanillaEventFlag.PumpkinMoon );
				}
			}
			if( this.CurrentEvents.ContainsKey( VanillaEventFlag.FrostMoon ) ) {
				if( Main.snowMoon ) {
					if( NPC.waveNumber > this.FrostMoonWavesConquered ) {
						this.FrostMoonWavesConquered = NPC.waveNumber;
					}
				} else {
					this.CurrentEvents.Remove( VanillaEventFlag.FrostMoon );
				}
			}

			var flags = NPCInvasionHelpers.GetCurrentEventTypeSet();

			this.CurrentEvents = new ConcurrentDictionary<VanillaEventFlag, byte>(
				DotNetHelpers.FlagsToList<VanillaEventFlag>( (int)flags ).Select(
					t => new KeyValuePair<VanillaEventFlag, byte>( t, 0 )
				)
			);
		}
	}
}
