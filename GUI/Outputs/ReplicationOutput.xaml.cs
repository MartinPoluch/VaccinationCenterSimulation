using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GUI.Outputs;
using VaccinationSim;
using VaccinationSim.Models;

namespace GUI {
	/// <summary>
	/// Interaction logic for ReplicationOutput.xaml
	/// </summary>
	public partial class ReplicationOutput : UserControl, OutputStat {
		public ReplicationOutput() {
			InitializeComponent();
			Registration.RoomType = RoomType.Registration;
			DoctorCheck.RoomType = RoomType.DoctorCheck;
			Vaccination.RoomType = RoomType.Vaccination;
		}

		public void Refresh(VacCenterState state) {
			Registration.Refresh(state);
			DoctorCheck.Refresh(state);
			Vaccination.Refresh(state);

			var waitRoom = state.WaitRoomReplicationStat;
			AvgPeopleInWaitRoom.Text = waitRoom.Average().ToString(CultureInfo.InvariantCulture);
			CiPeopleInWaitRoom.Text = waitRoom.ConfidenceInterval().ToString();

			AvgMissingPatients.Text = state.MissingPatientsReplicationStat.Average().ToString(CultureInfo.InvariantCulture);
			AvgLeftPatients.Text = state.LeftPatientReplicationStat.Average().ToString(CultureInfo.InvariantCulture);
			AvgCoolingDuration.Text = (state.DurationOfCoolingReplicationStat.Average() / 3600).ToString(CultureInfo.InvariantCulture);


		}
	}
}
