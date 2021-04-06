using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberGenerators;
using NumberGenerators.Tests;

namespace VaccinationSim {
	class Program {
		//23, 116
		static void Main(string[] args) {
			VacCenterAnalyzer analyzer = new VacCenterAnalyzer() {
				NumOfPatients = 2500,
				MinMissingPatients = 23,
				MaxMissingPatients = 116,
				MinNumOfWorkers = 23,
				MaxNumOfWorkers = 23,
				MinNumOfDoctors = 10,
				MaxNumOfDoctors = 20,
				MinNumOfNurses = 14,
				MaxNumOfNurses = 14,
				Replications = 200,
			};
			analyzer.Analyze();
			Console.Read();
		}
	}
}
