using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberGenerators;
using SimulationCore;
using SimulationCore.Generators;
using SimulationCore.Stats;

namespace VaccinationSim.Models {
	public class Room : Resettable {

		private Dictionary<int, UniformDiscreteGen> _serviceDecision;

		public Room(int numberOfServices, Generator generator, RoomType type, SimCore simulation) {
			Seeder seeder = Seeder.GetInstance();
			_serviceDecision = new Dictionary<int, UniformDiscreteGen>();
			for (int i = 2; i <= numberOfServices; i++) {
				_serviceDecision[i] = new UniformDiscreteGen(seeder.GetSeed(), 0, i);
			}

			Services = new Service[numberOfServices];
			for (int i = 0; i < numberOfServices; i++) {
				Services[i] = new Service(i, generator.Clone(), simulation);
			}

			Queue = new Queue<Patient>();
			QueueStat = new QueueStat(simulation);
			Type = type;
		}

		public Service[] Services { get; set; }

		public Queue<Patient> Queue { get; set; }

		public QueueStat QueueStat { get; set; }

		public RoomType Type { get; }

		public void Reset() {
			QueueStat.Reset();
			Queue.Clear();
			foreach (Service service in Services) {
				service.Reset();
			}
		}

		public bool IsAnyServiceFree() {
			foreach (Service service in Services) {
				if (! service.IsOccupied()) {
					return true;
				}
			}

			return false;
		}

		public void AddPatientToQueue(Patient patient) {
			QueueStat.AddQueueLength(Queue.Count);
			Queue.Enqueue(patient);
		}

		public Patient RemovePatientFromQueue() {
			QueueStat.AddQueueLength(Queue.Count);
			return Queue.Dequeue();
		}

		private List<Service> GetFreeServices() {
			List<Service> freeServices = new List<Service>();
			foreach (Service service in Services) {
				if (!service.IsOccupied()) {
					freeServices.Add(service);
				}
			}

			return freeServices;
		}

		public Service GetFreeService() {
			var freeServices = GetFreeServices();
			Debug.Assert(freeServices.Count > 0, "Patient try to select service, but all services are occupied!");
			if (freeServices.Count == 1) {
				return freeServices[0]; // nie je o com rozhodovat
			}

			UniformDiscreteGen decisionGen = _serviceDecision[freeServices.Count];
			int decision = decisionGen.NextInt();
			return freeServices[decision];
		}

		public double AverageServiceOccupancy() {
			double sumOfOccupancy = 0;
			foreach (Service service in Services) {
				sumOfOccupancy += service.Stat.GetServiceOccupancy();
			}

			return sumOfOccupancy / Services.Length;
		}
	}
}
