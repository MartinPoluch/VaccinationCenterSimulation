using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGenerators {
	public class ExponentialGen : Generator {

		private readonly double _mean;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="seed"></param>
		/// <param name="mean">Stredna hodnota</param>
		public ExponentialGen(int seed, double mean) : base(seed) {
			this._mean = mean;
		}

		public override Generator Clone() {
			return new ExponentialGen(GenerateRandomSeed(), _mean);
		}

		public override double GetValue() {
			return - Math.Log(1 - _random.NextDouble()) * _mean;
		}

		public override string ToString() {
			return $"Exp({_mean})";
		}
	}
}
