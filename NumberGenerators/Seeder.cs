using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGenerators {
	public class Seeder {

		private static Seeder _instance = null;
		private static int? _initialSeed = null;
		private readonly Random _random;

		private Seeder() {
			_random = (_initialSeed != null) ? new Random((int) _initialSeed) : new Random();
		}

		public static void SetInitialSeed(int seed) {
			if (_initialSeed == null) {
				_initialSeed = seed;
			}
			else {
				throw new Exception($"Seeder is already initialized with seed: {_initialSeed}");
			}
		}

		public static Seeder GetInstance() {
			return _instance ?? (_instance = new Seeder());
		}

		public int GetSeed() {
			return _random.Next();
		}
	}
}
