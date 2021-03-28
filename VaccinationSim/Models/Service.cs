﻿using NumberGenerators;
using SimulationCore;
using SimulationCore.Stats;

namespace VaccinationSim.Models {
	public class Service : Resettable{

		private readonly Generator _generator;

		public Service(int id, Generator generator) {
			Id = id;
			_generator = generator;
			Stat = new ServiceStat();
			Occupied = false;
		}

		public int Id { get; private set; }

		public ServiceStat Stat { get; set; }

		public bool Occupied { get; set; }

		public double GenerateDuration() {
			return _generator.GetValue();
		}

		public void Reset() {
			Stat.Reset();
			Occupied = false;
		}
	}
}