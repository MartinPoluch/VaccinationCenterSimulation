using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGenerators {
	public class TriangularGen : Generator {

		private readonly double _min;
		private readonly double _mode;
		private readonly double _max;

		public TriangularGen(int seed, double min, double max, double mode) : base(seed) {
			_min = min;
			_mode = mode;
			_max = max;
		}

		public override Generator Clone() {
			return new TriangularGen(GenerateRandomSeed(), _min, _max, _mode);
		}

		public override double GetValue() {
			//zdroj: https://stackoverflow.com/questions/33220176/triangular-distribution-in-java
			double bound = (_mode - _min) / (_max - _min);
			double randomNumber = _random.NextDouble();
			if (randomNumber < bound) {
				return _min + Math.Sqrt(randomNumber * (_max - _min) * (_mode - _min));
			}
			else {
				return _max - Math.Sqrt((1 - randomNumber) * (_max - _min) * (_max - _mode));
			}
		}

		public override string ToString() {
			return $"Triangular({_min}, {_max}, {_mode})";
		}
	}
}