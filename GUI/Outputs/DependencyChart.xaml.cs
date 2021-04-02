using System;
using System.Collections.Generic;
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
using VaccinationSim;
using VaccinationSim.Models;

namespace GUI.Outputs {
	/// <summary>
	/// Interaction logic for DependencyChart.xaml
	/// </summary>
	public partial class DependencyChart : UserControl, OutputStat {
		public DependencyChart() {
			InitializeComponent();
		}

		public void Refresh(VacCenterState state) {
			var doctorRoom = state.ReplicationStats[RoomType.DoctorCheck];
			DoctorQueueChart.AddChartValue(state.NumOfDoctors, doctorRoom.QueueLength.Average());
		}

		public void Clear() {
			DoctorQueueChart.Clear();
		}
	}
}
