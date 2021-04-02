using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGenerators {
	public class UniformContinuousGen : Generator {

		private readonly int _min;
		private readonly int _max;

		/// <summary>
		/// Generuje cisla na intervale <0, 1)
		/// </summary>
		/// <param name="seed"></param>
		public UniformContinuousGen(int seed) : base(seed) {
			_min = 0;
			_max = 1;
		}

		public override Generator Clone() {
			return new UniformContinuousGen(GenerateRandomSeed(), _min, _max);
		}

		/// <summary>
		/// Generuje cisla na intervale <min, max)
		/// </summary>
		/// <param name="seed"></param>
		public UniformContinuousGen(int seed, int min, int max) : base(seed) {
			_min = min;
			_max = max;
		}

		public override double GetValue() {
			return (_random.NextDouble() * (_max - _min)) + _min;
		}

		public override string ToString() {
			return $"UniformContinuous({_min}, {_max})";
		}
	}
}
