using System.Globalization;
using System.Windows.Controls;
using GUI.Outputs;
using SimulationCore.Stats;
using VaccinationSim;
using VaccinationSim.Stats;

namespace GUI {
	/// <summary>
	/// Interaction logic for GeneralOutput.xaml
	/// </summary>
	public partial class GeneralOutput : UserControl, OutputStat {
		public GeneralOutput() {
			InitializeComponent();
		}

		public void Refresh(VacCenterState state) {
			VacSystemStat systemStat = state.SystemStat;
			PatientsArrived.Text = systemStat.ArrivedCustomers.ToString();
			PatientsLeft.Text = systemStat.NumberOfValues.ToString(CultureInfo.InvariantCulture);
			PatientsInSystem.Text = systemStat.CustomersInSystem.ToString();
			PatientsMissing.Text = systemStat.MissingPatients.ToString();
		}
	}
}
