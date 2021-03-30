using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccinationSim.Models {
	public class Patient {

		private static int _lastId = 0; // counter pre generovanie id pacienta
		public readonly double NotInitialized = -1;

		public Patient(double arrivalTime) {
			ArrivalTime = arrivalTime;
			Id = _lastId;
			_lastId++;
			StartOfWaiting =
				new Dictionary<RoomType, double> {
					[RoomType.Registration] = NotInitialized, 
					[RoomType.DoctorCheck] = NotInitialized, 
					[RoomType.Vaccination] = NotInitialized
				};

			ServiceDuration =
				new Dictionary<RoomType, double> {
					[RoomType.Registration] = NotInitialized,
					[RoomType.DoctorCheck] = NotInitialized,
					[RoomType.Vaccination] = NotInitialized
				};
		}

		public static void ResetId() {
			_lastId = 0;
		}

		public int Id { get; }

		public double ArrivalTime { get;}

		public RoomType RoomType { get; set; }

		public Service Service { get; set; }

		public Dictionary<RoomType, double> StartOfWaiting { get; set; }

		public Dictionary<RoomType, double> ServiceDuration { get; set; }
	}
}
