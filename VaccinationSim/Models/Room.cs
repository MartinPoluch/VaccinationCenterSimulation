using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberGenerators;
using SimulationCore.Generators;

namespace VaccinationSim.Models {
	public class Room {

		private Dictionary<int, UniformDiscreteGen> _serviceDecision;

		public Room(int numberOfServices, Generator generator, RoomType type) {
			Seeder seeder = Seeder.GetInstance();
			_serviceDecision = new Dictionary<int, UniformDiscreteGen>();
			for (int i = 2; i <= numberOfServices; i++) {
				_serviceDecision[i] = new UniformDiscreteGen(seeder.GetSeed(), 0, numberOfServices - 1);
			}

			Services = new Service[numberOfServices];
			for (int i = 0; i < numberOfServices; i++) {
				Services[i] = new Service(generator.Clone());
			}

			Queue = new Queue<Patient>();
			Type = type;
		}

		public Service[] Services { get; set; }

		public Queue<Patient> Queue { get; set; }

		public RoomType Type { get; }
	}
}
