using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using GUI.Inputs;
using NumberGenerators;
using VaccinationSim;

namespace GUI {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		public MainWindow() {
			InitializeComponent();
			InitializeVacCenter();
			DataContext = this;
		}

		private void InitializeVacCenter() {
			VacCenterSim = new VacCenterSim(
				SimInputs.SourceIntensity, 
				SimInputs.NumOfWorkers,
				SimInputs.NumOfDoctors,
				SimInputs.NumOfNurses
				) 
			{
				MaximumSpeed = SimInputs.MaximumSpeed,
				WarmUpDuration = 0, //TODO without warmup
				ReportProgressReplicationFrequency = 1,
			};
			VacCenterSim.RegisterRefreshDuringSimulation(RefreshGuiInClassicMode);
			VacCenterSim.RegisterRefreshAfterSimulation(RefreshAfterSimulation);
		}

		public VacCenterSim VacCenterSim { get; set; }

		/// <summary>
		/// Metoda pre refreshovanie GUI v pripade pomaleho a rychleho rezimu. Pocas zahrievania sa GUI nerefreshuje.
		/// </summary>
		public void RefreshGuiInClassicMode(object sender, ProgressChangedEventArgs e) {
			VacCenterState currentState = (VacCenterState)e.UserState;
			CurrentReplicationOut.Text = currentState.CurrentReplication.ToString();
			ReplicationsOut.Refresh(currentState);
			if (!VacCenterSim.MaximumSpeed) {
				CurrentStateOutput.Refresh(currentState);
				SimulationTimeOut.Text = SimInputs.StartDateTime().AddSeconds(currentState.Time).ToString("HH:mm:ss");
			}
		}

		private void RefreshAfterSimulation(object sender, RunWorkerCompletedEventArgs e) {
			var error = e.Error;
			if (e.Error != null) {
				MessageBox.Show(error.StackTrace, error.Message, MessageBoxButton.OK, MessageBoxImage.Error);
			}
			StartAndStopBtn.IsChecked = false;
			ActivateReadyState();
		}

		public void CheckIntegerInput(object sender, TextCompositionEventArgs e) {
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		public void CheckDoubleInput(object sender, TextCompositionEventArgs e) {
			Regex regex = new Regex("[^0-9.-]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		private void StartClick(object sender, RoutedEventArgs e) {
			if (SimInputs.ValidInputs()) {
				ResetAllOutputs();
				ActivateRunningState();
				if (OtherInputs.SelectedMode() == Mode.Classic) {
					InitializeVacCenter();
					VacCenterSim.SimulateAsync(SimInputs.SimulationDuration, SimInputs.Replications);
				}
			}
			else {
				MessageBox.Show("Cannot start simulation", "Wrong inputs", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}


		private void ActivateReadyState() {
			ConsoleOut.Text = "Simulation is ready to start";
			StartAndStopBtn.Content = "Start";
			StartAndStopBtn.IsEnabled = true;
			PauseAndContinueBtn.Content = "Pause";

			PauseAndContinueBtn.IsEnabled = false;
			SimInputs.IsEnabled = true;
			OtherInputs.EnableAllInputs();
		}

		private void ActivateRunningState() {
			ConsoleOut.Text = "Simulation is running...";
			StartAndStopBtn.Content = "Stop";
			StartAndStopBtn.IsEnabled = true;
			PauseAndContinueBtn.Content = "Pause";

			PauseAndContinueBtn.IsEnabled = true;
			SimInputs.IsEnabled = false;
			OtherInputs.DisableAllInputs();
		}


		private void ActivatePausedState() {
			ConsoleOut.Text = "Simulation is paused";
			StartAndStopBtn.Content = "Stop";
			StartAndStopBtn.IsEnabled = false;
			PauseAndContinueBtn.Content = "Continue";

			PauseAndContinueBtn.IsEnabled = true;
			SimInputs.IsEnabled = false;
			OtherInputs.DisableAllInputs();
		}

		private void ResetAllOutputs() {
			//TODO reset outputs here
		}

		private void StopClick(object sender, RoutedEventArgs e) {
			VacCenterSim.Stop = true;
			ActivateReadyState();
		}

		private void PauseClick(object sender, RoutedEventArgs e) {
			VacCenterSim.Pause = true;
			ActivatePausedState();
		}

		private void ContinueClick(object sender, RoutedEventArgs e) {
			VacCenterSim.Pause = false;
			ActivateRunningState();
		}
	}
}

