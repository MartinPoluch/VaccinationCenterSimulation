using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationCore.Stats {

	/// <summary>
	/// Zber statistik ktore sa tykaju lubovolnej fronty.
	/// </summary>
	public class QueueStat : Resettable {

		private double _sumOfWaitingTime;
		private double _sumOfQueueLength;
		private double _lastChange;

		public QueueStat(SimCore simulation) {
			Simulation = simulation;
		}

		public virtual void Reset() {
			_sumOfWaitingTime = 0;
			NumberOfCustomers = 0;
			_sumOfQueueLength = 0;
			DurationOfQueueChecks = 0;
			_lastChange = 0;
		}

		public SimCore Simulation { get; set; }

		public int NumberOfCustomers { get; private set; }

		private double DurationOfQueueChecks { get; set; }

		public void AddWaitingTime(double waitingTime) {
			_sumOfWaitingTime += waitingTime;
			NumberOfCustomers++;
		}

		public double AverageWaitingTime() {
			if (NumberOfCustomers == 0) {
				return 0.0;
			}
			return _sumOfWaitingTime / NumberOfCustomers;
		}

		public void AddQueueLength(int actualLength) {
			double duration = Simulation.CurrentTime - _lastChange;
			_sumOfQueueLength += (actualLength * duration);
			DurationOfQueueChecks += duration;
			_lastChange = Simulation.CurrentTime;
		}

		public double AverageQueueLength() {
			if (DurationOfQueueChecks == 0) {
				return 0.0;
			}
			return _sumOfQueueLength / DurationOfQueueChecks;
		}

		public override string ToString() {
			return $"sumOfWaitingTime: {_sumOfWaitingTime}\n" +
			       $"sumOfQueueLength: {_sumOfQueueLength}\n";
		}
	}
}
