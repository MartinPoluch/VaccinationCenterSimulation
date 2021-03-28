using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationCore.Stats {
	public class MeanStat : Resettable{

		private double _sumOfValues;

		public MeanStat() {
		}

		public double NumberOfValues { get; private set; }

		public virtual void Reset() {
			_sumOfValues = 0;
			NumberOfValues = 0;
		}

		public virtual void AddValue(double value) {
			_sumOfValues += value;
			NumberOfValues++;
		}

		public virtual double Average() {
			if (NumberOfValues == 0) {
				return 0;
			}
			else {
				return _sumOfValues / NumberOfValues;
			}
		}
	}
}
