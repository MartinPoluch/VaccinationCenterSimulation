using System.Windows.Controls;
using GUI.Outputs;
using VaccinationSim;

namespace GUI {
	/// <summary>
	/// Interaction logic for GeneralOutput.xaml
	/// </summary>
	public partial class GeneralOutput : UserControl, OutputStat {
		public GeneralOutput() {
			InitializeComponent();
		}

		public void Refresh(VacCenterState state) {
			throw new System.NotImplementedException();
		}
	}
}
