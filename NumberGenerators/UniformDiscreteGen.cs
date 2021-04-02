using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationCore.Generators {
	public class UniformDiscreteGen {

		private readonly int _minValue;
		private readonly int _maxValue;
		private readonly Random random;

		public UniformDiscreteGen(int seed, int minValue, int maxValue) {
			_minValue = minValue;
			_maxValue = maxValue;
			Seed = seed;
			random = new Random(seed);
		}

		public int Seed { get;}



		
		public int NextInt() {
			return random.Next(_minValue, _maxValue);
		}
	}
}
