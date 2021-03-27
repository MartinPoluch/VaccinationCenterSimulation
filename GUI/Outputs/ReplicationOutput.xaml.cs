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
using VaccinationSim;

namespace GUI {
	/// <summary>
	/// Interaction logic for ReplicationOutput.xaml
	/// </summary>
	public partial class ReplicationOutput : UserControl {
		public ReplicationOutput() {
			InitializeComponent();
		}

		public void RefreshStats(VacCenterState currentState) {
		}

	}
}
