using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SimulationCore;
using SimulationCore.Stats;
using VaccinationSim.Models;

namespace VaccinationSim.Stats {
	public class RoomStat : Resettable {

		public RoomStat() {
			WaitingTime = new ReplicationStat();
			QueueLength = new ReplicationStat();
			ServiceOccupancy = new ReplicationStat();
		}

		public ReplicationStat WaitingTime { get; set; }

		public ReplicationStat QueueLength { get; set; }

		public ReplicationStat ServiceOccupancy { get; set; }

		public void UpdateStats(Room room) {
			WaitingTime.AddValue(room.QueueStat.AverageWaitingTime());
			QueueLength.AddValue(room.QueueStat.AverageQueueLength());
			ServiceOccupancy.AddValue(room.AverageServiceOccupancy());
		}

		public void Reset() {
			WaitingTime.Reset();
			QueueLength.Reset();
			ServiceOccupancy.Reset();
		}
	}
}
