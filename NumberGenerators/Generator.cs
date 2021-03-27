using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGenerators {
	public abstract class Generator {

		protected Random _random;

		protected Generator(int seed) {
			this._random = new Random(seed);
			this.Seed = seed;
		}

		public abstract Generator Clone();

		protected int GenerateRandomSeed() {
			Seeder seeder = Seeder.GetInstance();
			return seeder.GetSeed();
		}

		public int Seed { get; set; }

		public abstract double GetValue();
	}
}
