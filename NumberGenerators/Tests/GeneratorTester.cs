using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGenerators.Tests {
	public class GeneratorTester {

		private Generator _generator;

		public GeneratorTester(Generator generator) {
			_generator = generator;
		}

		public void GenerateToFile(string pathToDirectory, int numberOfValues) {
			string fileName = $"{_generator} seed={ _generator.Seed}.txt";
			string pathToFile = Path.Combine(pathToDirectory, fileName);
			using (StreamWriter writer = new StreamWriter(pathToFile)) {
				for (int i = 0; i < numberOfValues; i++) {
					writer.WriteLine(_generator.GetValue().ToString(CultureInfo.InvariantCulture));
				}
			}
		}

		public void GenerateToConsole(int numberOfValues) {
			Console.WriteLine($"{_generator} seed: { _generator.Seed}");
			for (int i = 0; i < numberOfValues; i++) {
				Console.WriteLine(_generator.GetValue());
			}
		}
	}
}
