using System;
using NumberGenerators;
using SimulationCore;
using SimulationCore.Stats;

namespace VaccinationSim.Models {
	public class Service : Resettable{

		private readonly Generator _generator;

		public Service(int id, Generator generator, SimCore simulation) {
			Id = id;
			_generator = generator;
			Stat = new ServiceStat(simulation);
			Release();
		}

		public int Id { get; private set; }

		public ServiceStat Stat { get; private set; }

		private bool Occupied { get; set; }

		public string State { get; set; }

		public void Occupy() {
			Occupied = true;
			State = "Occupied";
		}

		public void Release() {
			Occupied = false;
			State = "Free";
		}

		public bool IsOccupied() {
			return Occupied;
		}

		public double GenerateDuration() {
			return _generator.GetValue();
		}

		public void Reset() {
			Stat.Reset();
			Release();
		}
	}
}
