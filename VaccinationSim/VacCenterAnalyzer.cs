using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccinationSim.Models;

namespace VaccinationSim {
	public class VacCenterAnalyzer {
		public VacCenterAnalyzer() {
			CsvOutput = new StringBuilder();
			CsvOutput.Append("Workers;Doctors;Nurses;Workers oc.;Doctors oc.;Nurses oc.;\n");
		}

		public StringBuilder CsvOutput { get; set; }

		public int MinNumOfWorkers { get; set; }

		public int MaxNumOfWorkers { get; set; }

		public int MinNumOfDoctors { get; set; }

		public int MaxNumOfDoctors { get; set; }

		public int MinNumOfNurses { get; set; }

		public int MaxNumOfNurses { get; set; }

		public int NumOfPatients { get; set; }

		public int MinMissingPatients { get; set; }

		public int MaxMissingPatients { get; set; }

		public int Replications { get; set; }

		public void Analyze() {
			for (int nurse = MinNumOfNurses; nurse <= MaxNumOfNurses; nurse++) {
				for (int doctor = MinNumOfDoctors; doctor <= MaxNumOfDoctors; doctor++) {
					for (int worker = MinNumOfWorkers; worker <= MaxNumOfWorkers; worker++) {
						VacCenterSim simulation = new VacCenterSim(
							NumOfPatients, MinMissingPatients, MaxMissingPatients,
							worker, doctor, nurse);
						simulation.Simulate(int.MaxValue, Replications);
						VacCenterState result = (VacCenterState)simulation.GetCurrentState();
						double workersOccupancy = result.ReplicationStats[RoomType.Registration].ServiceOccupancy.Average();
						double doctorsOccupancy = result.ReplicationStats[RoomType.DoctorCheck].ServiceOccupancy.Average();
						double nursesOccupancy = result.ReplicationStats[RoomType.Vaccination].ServiceOccupancy.Average();
						double cooling = result.DurationOfCoolingReplicationStat.Average();
						double avgQueueLenForDoctor = result.ReplicationStats[RoomType.DoctorCheck].QueueLength.Average();
						double avgWaitingTimeForDoctor = result.ReplicationStats[RoomType.DoctorCheck].WaitingTime.Average();
						Console.WriteLine($"w: {worker}; d: {doctor}; n: {nurse} => " +
						                  $"occupancy(" +
						                  $"w= {workersOccupancy}; " +
						                  $"d= {doctorsOccupancy}; " +
						                  $"n= {nursesOccupancy}) " +
						                  $"cooling= {cooling}; " +
						                  $"doctor(" +
						                  $"queueLen= {avgQueueLenForDoctor}; " +
						                  $"waitTime= {avgWaitingTimeForDoctor})");
						CsvOutput.Append($"{worker};{doctor};{nurse};{workersOccupancy};{doctorsOccupancy};{nursesOccupancy};\n");
					}
				}
			}

			WriteCsvToFile();
		}

		private void WriteCsvToFile() {
			string pathToDirectory = @"C:\Users\uzivatel\Desktop";
			string fileName = $"analysis({MinNumOfWorkers}, {MinNumOfDoctors}, {MinNumOfNurses}).txt";
			string pathToFile = Path.Combine(pathToDirectory, fileName);
			using (StreamWriter writer = new StreamWriter(pathToFile)) {
				writer.WriteLine(CsvOutput.ToString());

			}
		}
	}
}
