using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Models;

namespace VaccinationSim {
	public class VacCenterState :  SimState {

		public Dictionary<RoomType, Room> Rooms { get; set; }

		public override void ResetBeforeReplication() {
			foreach (Room room in Rooms.Values) {
				room.Reset();
			}
		}

		public override void ResetAll() {
			ResetBeforeReplication();
		}
	}
}
