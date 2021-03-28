using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccinationSim.Models {
	public class Patient {
		public Patient() {
			StartOfWaiting =
				new Dictionary<RoomType, double> {
					[RoomType.Registration] = 0.0, 
					[RoomType.DoctorCheck] = 0.0, 
					[RoomType.Vaccination] = 0.0
				};

			ServiceDuration =
				new Dictionary<RoomType, double> {
					[RoomType.Registration] = 0.0,
					[RoomType.DoctorCheck] = 0.0,
					[RoomType.Vaccination] = 0.0
				};
		}

		public RoomType RoomType { get; set; }

		public Service Service { get; set; }

		public Dictionary<RoomType, double> StartOfWaiting { get; set; }

		public Dictionary<RoomType, double> ServiceDuration { get; set; }
	}
}
