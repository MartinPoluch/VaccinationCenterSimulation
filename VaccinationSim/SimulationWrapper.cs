using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VaccinationSim {
	public class SimulationWrapper {

		private bool _stop;

		public SimulationWrapper() {
			_stop = false;
		}

		public void Stop() {
			if (simulation != null) {
				_stop = true;
				simulation.Stop = true;
			}
		}

		public void Pause() {
			if (simulation != null) {
				simulation.Pause = true;
			}
		}

		public void Continue() {
			if (simulation != null) {
				simulation.Pause = false;
			}
		}

		public VacCenterSim simulation { get; set; }

		public void Simulate(BackgroundWorker worker, int replications, int numOfPatients, int numOfWorkers, 
			int minNumOfDoctors, int maxNumOfDoctors, int numOfNurses, 
			int minMissingPatients, int maxMissingPatients) {
			_stop = false;
			for (int doctors = minNumOfDoctors; (doctors < maxNumOfDoctors) && !_stop; doctors++) {
				VacCenterSim simulation = new VacCenterSim(
					numOfPatients, 
					minMissingPatients, maxMissingPatients, 
					numOfWorkers, doctors, numOfNurses);
				simulation.Simulate(int.MaxValue, replications);
				worker.ReportProgress(1, simulation.GetCurrentState());
			}
		}
	}
}
