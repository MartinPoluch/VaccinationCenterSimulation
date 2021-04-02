using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using SimulationCore.Stats;

namespace VaccinationSim.Stats {

	/**
	 * Adapter for QueueStat
	 */
	public class WaitRoomStat : QueueStat {

		public WaitRoomStat(SimCore simulation) : base(simulation) {
			WaitingPatients = 0;
		}

		public override void Reset() {
			base.Reset();
			WaitingPatients = 0;
		}

		public int WaitingPatients { get; private set; }

		public void AddWaitingPatient() {
			AddQueueLength(WaitingPatients);
			WaitingPatients++;
		}

		public void RemoveWaitingPatient() {
			AddQueueLength(WaitingPatients);
			WaitingPatients--;
		}

		public double GetAverageWaitingPatients() {
			return AverageQueueLength();
		}
	}
}
