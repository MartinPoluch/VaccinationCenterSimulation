using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationCore.Stats {

	/// <summary>
	/// Zber statistik ktore sa tykaju lubovolnej obsluhy.
	/// </summary>
	public class ServiceStat : Resettable {

		private double _durationOfOccupiedService;

		public ServiceStat(SimCore simulation) {
			Simulation = simulation;
		}

		public void Reset() {
			_durationOfOccupiedService = 0;
		}

		public SimCore Simulation { get; private set; }

		public void AddServiceOccupancy(double duration) {
			_durationOfOccupiedService += duration;
		}

		public double GetServiceOccupancy() {
			double durationOfSimulation = Simulation.CurrentTime;
			if (durationOfSimulation == 0) {
				return 0.0;
			}
			return (_durationOfOccupiedService / durationOfSimulation) * 100;
		}

		public override string ToString() {
			return GetServiceOccupancy().ToString();
		}
	}
}
