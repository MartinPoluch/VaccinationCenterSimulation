using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberGenerators;

namespace VaccinationSim {
	class Program {
		static void Main(string[] args) {
			Generator generator = new UniformContinuousGen(Seeder.GetInstance().GetSeed(), 0, 0);
			for (int i = 0; i < 1000; i++) {
				Console.WriteLine(generator.GetValue());
			}

			Console.Read();
		}
	}
}
