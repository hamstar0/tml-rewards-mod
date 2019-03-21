using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using Rewards.NetProtocols;
using System.Collections.Generic;
using Terraria;


namespace Rewards.Logic {
	partial class KillData {
		public void Update() {
			if( Main.netMode == 1 ) { return; }

			VanillaEventFlag invasionFlag = NPCInvasionHelpers.GetEventTypeOfInvasionType( Main.invasionType );
			VanillaEventFlag eventFlags = NPCInvasionHelpers.GetCurrentEventTypeSet();
			IEnumerable<VanillaEventFlag> eventFlagSet = DotNetHelpers.FlagsToList<VanillaEventFlag>( (int)eventFlags );
			bool eventsChanged = false;

			eventsChanged = this.UpdateForInvasionEndings( invasionFlag );
			eventsChanged = this.UpdateForEventAdditions( eventFlagSet ) || eventsChanged;

			if( eventsChanged ) {
				if( Main.netMode == 2 ) {
					PacketProtocolSendToClient.QuickSend<EventsSyncProtocol>( -1, -1 );
				}
			}
		}


		////////////////

		internal bool UpdateForInvasionEndings( VanillaEventFlag invasionFlag ) {
			bool eventsChanged = false;

			if( this.CurrentEvents.Contains( VanillaEventFlag.Goblins ) ) {
				if( invasionFlag != VanillaEventFlag.Goblins ) {
					eventsChanged = true;
					this.CurrentEvents.Remove( VanillaEventFlag.Goblins );
					this.GoblinsConquered++;
				}
			}
			if( this.CurrentEvents.Contains( VanillaEventFlag.FrostLegion ) ) {
				if( invasionFlag != VanillaEventFlag.FrostLegion ) {
					eventsChanged = true;
					this.CurrentEvents.Remove( VanillaEventFlag.FrostLegion );
					this.FrostLegionConquered++;
				}
			}
			if( this.CurrentEvents.Contains( VanillaEventFlag.Pirates ) ) {
				if( invasionFlag != VanillaEventFlag.Pirates ) {
					eventsChanged = true;
					this.CurrentEvents.Remove( VanillaEventFlag.Pirates );
					this.PiratesConquered++;
				}
			}
			if( this.CurrentEvents.Contains( VanillaEventFlag.Martians ) ) {
				if( invasionFlag != VanillaEventFlag.Martians ) {
					eventsChanged = true;
					this.CurrentEvents.Remove( VanillaEventFlag.Martians );
					this.MartiansConquered++;
				}
			}
			if( this.CurrentEvents.Contains( VanillaEventFlag.PumpkinMoon ) ) {
				if( Main.pumpkinMoon ) {
					if( NPC.waveNumber > this.PumpkinMoonWavesConquered ) {
						eventsChanged = true;
						this.PumpkinMoonWavesConquered = NPC.waveNumber;
					}
				} else {
					eventsChanged = true;
					this.CurrentEvents.Remove( VanillaEventFlag.PumpkinMoon );
				}
			}
			if( this.CurrentEvents.Contains( VanillaEventFlag.FrostMoon ) ) {
				if( Main.snowMoon ) {
					if( NPC.waveNumber > this.FrostMoonWavesConquered ) {
						eventsChanged = true;
						this.FrostMoonWavesConquered = NPC.waveNumber;
					}
				} else {
					eventsChanged = true;
					this.CurrentEvents.Remove( VanillaEventFlag.FrostMoon );
				}
			}

			return eventsChanged;
		}


		internal bool UpdateForEventAdditions( IEnumerable<VanillaEventFlag> eventFlagSet ) {
			bool eventsChanged = false;

			foreach( var flag in eventFlagSet ) {
				if( !this.CurrentEvents.Contains( flag ) ) {
					eventsChanged = true;
					this.CurrentEvents.Add( flag );
				}
			}

			return eventsChanged;
		}
	}
}
