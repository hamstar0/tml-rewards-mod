using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using Rewards.NetProtocols;
using System;
using System.Collections.Generic;
using Terraria;


namespace Rewards.Logic {
	partial class WorldLogic {
		public void UpdateEvents() {
			if( Main.netMode == 1 ) { return; }

			VanillaEventFlag currentEventFlags = NPCInvasionHelpers.GetCurrentEventTypeSet();
			bool eventsChanged = false;

			eventsChanged = this.UpdateForEventChangesAndEndings( currentEventFlags );
			eventsChanged = this.UpdateForEventsBeginnings( currentEventFlags ) || eventsChanged;

			if( eventsChanged ) {
				if( Main.netMode == 2 ) {
					PacketProtocolSendToClient.QuickSend<EventsSyncProtocol>( -1, -1 );
				}
			}
		}


		////////////////

		internal bool UpdateForEventsBeginnings( VanillaEventFlag eventFlags ) {
			bool eventsChanged = false;
			IEnumerable<VanillaEventFlag> eventFlagSet = DotNetHelpers.FlagsToList<VanillaEventFlag>( (int)eventFlags );

			foreach( VanillaEventFlag flag in eventFlagSet ) {
				if( this.CurrentEvents.Contains( flag ) ) { continue; }

				switch( flag ) {
				case VanillaEventFlag.Sandstorm:
				case VanillaEventFlag.BloodMoon:
				case VanillaEventFlag.SlimeRain:
				case VanillaEventFlag.SolarEclipse:
				case VanillaEventFlag.LunarApocalypse:
					break;
				default:
					if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
						LogHelpers.Alert( "Event added: "+Enum.GetName(typeof(VanillaEventFlag), flag) );
					}
					eventsChanged = true;
					break;
				}

				this.CurrentEvents.Add( flag );
			}

			return eventsChanged;
		}


		internal bool UpdateForEventChangesAndEndings( VanillaEventFlag eventFlags ) {
			bool eventsChanged = false;

			if( this.CurrentEvents.Contains( VanillaEventFlag.Goblins ) ) {
				if( (eventFlags & VanillaEventFlag.Goblins) == 0 ) {
					if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
						LogHelpers.Alert( "Goblin event ended." );
					}
					eventsChanged = true;

					this.CurrentEvents.Remove( VanillaEventFlag.Goblins );
					this.GoblinsConquered++;
				}
			}
			if( this.CurrentEvents.Contains( VanillaEventFlag.FrostLegion ) ) {
				if( (eventFlags & VanillaEventFlag.FrostLegion) == 0 ) {
					if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
						LogHelpers.Alert( "Frost Legion event ended." );
					}
					eventsChanged = true;

					this.CurrentEvents.Remove( VanillaEventFlag.FrostLegion );
					this.FrostLegionConquered++;
				}
			}
			if( this.CurrentEvents.Contains( VanillaEventFlag.Pirates ) ) {
				if( (eventFlags & VanillaEventFlag.Pirates) == 0 ) {
					if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
						LogHelpers.Alert( "Pirates event ended." );
					}
					eventsChanged = true;

					this.CurrentEvents.Remove( VanillaEventFlag.Pirates );
					this.PiratesConquered++;
				}
			}
			if( this.CurrentEvents.Contains( VanillaEventFlag.Martians ) ) {
				if( (eventFlags & VanillaEventFlag.Martians) == 0 ) {
					if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
						LogHelpers.Alert( "Martians event ended." );
					}
					eventsChanged = true;

					this.CurrentEvents.Remove( VanillaEventFlag.Martians );
					this.MartiansConquered++;
				}
			}

			if( this.CurrentEvents.Contains( VanillaEventFlag.PumpkinMoon ) ) {
				//if( Main.pumpkinMoon ) {
				if( (eventFlags & VanillaEventFlag.PumpkinMoon) != 0 ) {
					if( NPC.waveNumber > this.PumpkinMoonWavesConquered ) { // Change wave
						if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
							LogHelpers.Alert( "Pumpkin Moon event wave: "+NPC.waveNumber );
						}
						eventsChanged = true;

						this.PumpkinMoonWavesConquered = NPC.waveNumber;
					}
				} else {    // End event
					if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
						LogHelpers.Alert( "Pumpkin Moon event ended." );
					}
					eventsChanged = true;

					this.CurrentEvents.Remove( VanillaEventFlag.PumpkinMoon );
				}
			}
			if( this.CurrentEvents.Contains( VanillaEventFlag.FrostMoon ) ) {
				//if( Main.snowMoon ) {
				if( (eventFlags & VanillaEventFlag.FrostMoon) != 0 ) {
					if( NPC.waveNumber > this.FrostMoonWavesConquered ) {   // Change wave
						if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
							LogHelpers.Alert( "Frost Moon event wave: " + NPC.waveNumber );
						}
						eventsChanged = true;

						this.FrostMoonWavesConquered = NPC.waveNumber;
					}
				} else {    // End event
					if( RewardsMod.Instance.SettingsConfig.DebugModeInfo ) {
						LogHelpers.Alert( "Frost Moon event ended." );
					}
					eventsChanged = true;

					this.CurrentEvents.Remove( VanillaEventFlag.FrostMoon );
				}
			}

			if( this.CurrentEvents.Contains( VanillaEventFlag.Sandstorm ) ) {
				//if( !Sandstorm.Happening ) {
				if( (eventFlags & VanillaEventFlag.Sandstorm ) != 0 ) {
					//eventsChanged = true;
					this.CurrentEvents.Remove( VanillaEventFlag.Sandstorm );
				}
			}
			if( this.CurrentEvents.Contains( VanillaEventFlag.BloodMoon ) ) {
				//if( !Main.bloodMoon ) {
				if( ( eventFlags & VanillaEventFlag.BloodMoon ) != 0 ) {
					//eventsChanged = true;
					this.CurrentEvents.Remove( VanillaEventFlag.BloodMoon );
				}
			}
			if( this.CurrentEvents.Contains( VanillaEventFlag.SlimeRain ) ) {
				//if( !Main.slimeRain ) {
				if( ( eventFlags & VanillaEventFlag.SlimeRain ) != 0 ) {
					//eventsChanged = true;
					this.CurrentEvents.Remove( VanillaEventFlag.SlimeRain );
				}
			}
			if( this.CurrentEvents.Contains( VanillaEventFlag.SolarEclipse ) ) {
				//if( !Main.eclipse ) {
				if( ( eventFlags & VanillaEventFlag.SolarEclipse ) != 0 ) {
					//eventsChanged = true;
					this.CurrentEvents.Remove( VanillaEventFlag.SolarEclipse );
				}
			}
			if( this.CurrentEvents.Contains( VanillaEventFlag.LunarApocalypse ) ) {
				//if( !NPC.LunarApocalypseIsUp ) {
				if( ( eventFlags & VanillaEventFlag.LunarApocalypse ) != 0 ) {
					//eventsChanged = true;
					this.CurrentEvents.Remove( VanillaEventFlag.LunarApocalypse );
				}
			}

			return eventsChanged;
		}
	}
}
