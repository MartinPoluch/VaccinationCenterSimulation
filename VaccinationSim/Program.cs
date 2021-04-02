using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberGenerators;
using NumberGenerators.Tests;

namespace VaccinationSim {
	class Program {
		static void Main(string[] args) {
			Seeder seeder = Seeder.GetInstance();
			Generator generator = new UniformContinuousGen(seeder.GetSeed(), 140, 220);
			GeneratorTester tester = new GeneratorTester(generator);
			tester.GenerateToFile("C:\\Users\\uzivatel\\Desktop\\School\\Ing\\4. semester\\Generator output", 1000000);
		}
	}
}
