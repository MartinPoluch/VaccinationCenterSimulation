using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberGenerators;
using SimulationCore;
using SimulationCore.Stats;
using VaccinationSim.Events;
using VaccinationSim.Models;

namespace VaccinationSim {
	public class VacCenterSim : SimCore {

		private readonly double DurationOfWorkDay = 60 * 60 * 9;
		private readonly int NumOfPatients;

		private Generator _arrivalsGen;
		private bool _newsstandTest = true;

		public VacCenterSim(int numOfPatients ,int numOfWorkers, int numOfDoctors, int numOfNurses) {
			NumOfPatients = numOfPatients;
			Seeder seeder = Seeder.GetInstance();

			if (_newsstandTest) {
				_arrivalsGen = new ExponentialGen(seeder.GetSeed(), 5.0);
				Rooms = new Dictionary<RoomType, Room> {
					[RoomType.Registration] =
						new Room(numOfWorkers, new ExponentialGen(seeder.GetSeed(), 4.0), RoomType.Registration, new QueueStat(this)),
				};
			}
			else {
				Rooms = new Dictionary<RoomType, Room> {
					[RoomType.Registration] =
						new Room(
							numOfWorkers, 
							new UniformContinuousGen(seeder.GetSeed(), 140, 220), 
							RoomType.Registration, 
							new QueueStat(this)),
					[RoomType.DoctorCheck] =
						new Room(
							numOfDoctors, 
							new ExponentialGen(seeder.GetSeed(), 260), 
							RoomType.DoctorCheck, 
							new QueueStat(this)),
					[RoomType.Vaccination] =
						new Room(
							numOfNurses, 
							new TriangularGen(seeder.GetSeed(), 20, 100, 75), 
							RoomType.Vaccination, 
							new QueueStat(this))
				};
			}
			
			State = new VacCenterState() {
				Rooms = Rooms,
			};
		}

		public Dictionary<RoomType, Room> Rooms { get; set; }

		private VacCenterState State { get; set; }

		public double GetNextArrivalTime() {
			if (_newsstandTest) {
				return CurrentTime + _arrivalsGen.GetValue();
			}
			else {
				//TODO, niektory pacienti nepridu prestan planovat ked pridu uz vsetci pacienti alebo vyprsi cas
				double timeBetweenArrivals = DurationOfWorkDay / (double)NumOfPatients;
				return CurrentTime + timeBetweenArrivals;
			}
		}

		protected override void BeforeSimulation() {
			State.ResetAll();
		}

		protected override void BeforeReplication() {
			State.ResetBeforeReplication();
			PlanEvent(new ArrivalToRegistrationEvent(this, StartTime, new Patient()));
		}

		protected override void AfterReplication() {
			
		}

		protected override void AfterSimulation() {

		}

		public override SimState GetCurrentState() {
			return State;
		}
	}
}
