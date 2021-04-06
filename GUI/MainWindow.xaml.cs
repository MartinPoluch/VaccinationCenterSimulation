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
using VaccinationSim.Models;

namespace GUI {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		public MainWindow() {
			//Seeder.SetInitialSeed(5);
			InitializeComponent();
			InitVacCenter();
			Title = "Vaccination center";
			DataContext = this;
		}

		private void InitVacCenter() {
			VacCenterSim = new VacCenterSim(
				SimInputs.SourceIntensity,
				SimInputs.GetMinMissingPatients(),
				SimInputs.GetMaxMissingPatients(),
				SimInputs.NumOfWorkers,
				SimInputs.NumOfDoctors,
				SimInputs.NumOfNurses
				) {
				MaximumSpeed = SimInputs.MaximumSpeed,
				WarmUpDuration = 0,
				ReportProgressReplicationFrequency = SimInputs.ReplicationRefreshFreq,
			};
			//VacCenterSim = new VacCenterSim(
			//	SimInputs.SourceIntensity,
			//	5,
			//	25,
			//	SimInputs.NumOfWorkers,
			//	SimInputs.NumOfDoctors,
			//	SimInputs.NumOfNurses
			//) {
			//	MaximumSpeed = SimInputs.MaximumSpeed,
			//	WarmUpDuration = 0,
			//	ReportProgressReplicationFrequency = SimInputs.ReplicationRefreshFreq,
			//};
			VacCenterSim.RegisterRefreshDuringSimulation(RefreshGui);
			VacCenterSim.RegisterRefreshAfterSimulation(RefreshAfterSimulation);
		}

		public VacCenterSim VacCenterSim { get; set; }

		public SimulationWrapper SimulationWrapper { get; set; }

		/// <summary>
		/// Metoda pre refreshovanie GUI v pripade pomaleho a rychleho rezimu. Pocas zahrievania sa GUI nerefreshuje.
		/// </summary>
		public void RefreshGui(object sender, ProgressChangedEventArgs e) {
			VacCenterState currentState = (VacCenterState)e.UserState;
			CurrentReplicationOut.Text = currentState.CurrentReplication.ToString();
			ReplicationsOut.Refresh(currentState);
			if (currentState.CurrentReplication == SimInputs.Replications) {
				RefreshConsole(currentState);
			}
			if (!VacCenterSim.MaximumSpeed) { // refresh statistik pri pomalom rezime
				CurrentStateOutput.Refresh(currentState);
				SimulationTimeOut.Text = SimInputs.StartDateTime().AddSeconds(currentState.Time).ToString("HH:mm:ss");
			}
		}

		private void RefreshConsole(VacCenterState currentState) {
			var rooms = currentState.ReplicationStats;
			var doctorRoom = rooms[RoomType.DoctorCheck];
			ConsoleOut.Text = "";
			ConsoleOut.Text += $"\nworkers occupancy: {rooms[RoomType.Registration].ServiceOccupancy.Average()}";
			ConsoleOut.Text += $"\ndoctors occupancy: {doctorRoom.ServiceOccupancy.Average()}";
			ConsoleOut.Text += $"\nnurses occupancy: {rooms[RoomType.Vaccination].ServiceOccupancy.Average()}";
			ConsoleOut.Text += $"\n______________________________________________________";
			ConsoleOut.Text += $"\ndoctor queue length: {doctorRoom.QueueLength.Average()}";
			ConsoleOut.Text += $"\ndoctor wait time: {doctorRoom.WaitingTime.Average()}";
		}

		private void RefreshAfterSimulation(object sender, RunWorkerCompletedEventArgs e) {
			var error = e.Error;
			if (error != null) {
				Console.WriteLine($"Error occured: {error}");
				MessageBox.Show(error.StackTrace, error.Message, MessageBoxButton.OK, MessageBoxImage.Error);
			}
			StartAndStopBtn.IsChecked = false;
			ActivateReadyState();
		}

		private void StartClick(object sender, RoutedEventArgs e) {
			if (SimInputs.ValidInputs()) {
				ResetAllOutputs();
				ActivateRunningState();
				if (OtherInputs.SelectedMode() == Mode.Classic) {
					InitVacCenter();
					VacCenterSim.SimulateAsync(SimInputs.SimulationDuration, SimInputs.Replications);
				}
				if (OtherInputs.SelectedMode() == Mode.DependencyChart) {
					CreateDependencyChart();
				}
			}
			else {
				MessageBox.Show("Cannot start simulation", "Wrong inputs", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void CreateDependencyChart() {
			DependencyChart.Clear(); // clear previous chart values
			ConsoleOut.Text = "Calculating points for dependency chart ...";
			SimulationWrapper = new SimulationWrapper();
			BackgroundWorker worker = new BackgroundWorker() {
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};
			worker.DoWork += delegate (object sender, DoWorkEventArgs args) {
				SimulationWrapper.Simulate(
					worker, 
					SimInputs.Replications, 
					SimInputs.SourceIntensity, 
					SimInputs.NumOfWorkers, 
					OtherInputs.MinDoctors, 
					OtherInputs.MaxDoctors, 
					SimInputs.NumOfNurses, 
					SimInputs.GetMinMissingPatients(), 
					SimInputs.GetMaxMissingPatients());
			};
			worker.ProgressChanged += RefreshDependencyChart;
			worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs args) {
				RefreshAfterSimulation(sender, args);
				ConsoleOut.Text += "Dependency chart was successfully created.";
			};
			worker.RunWorkerAsync();
			
		}

		private void RefreshDependencyChart(object sender, ProgressChangedEventArgs e) {
			VacCenterState currentState = (VacCenterState)e.UserState;
			DependencyChart.Refresh(currentState);
			var doctorRoom = currentState.ReplicationStats[RoomType.DoctorCheck];
			ConsoleOut.Text += $"\nAdded point => " +
			                   $"doctors:{currentState.NumOfDoctors} queue length: {doctorRoom.QueueLength.Average()}";
		}

		private void ActivateReadyState() {
			ConsoleOut.Text += "\nSimulation is ready to start";
			StartAndStopBtn.Content = "Start";
			StartAndStopBtn.IsEnabled = true;
			PauseAndContinueBtn.Content = "Pause";

			PauseAndContinueBtn.IsEnabled = false;
			SimInputs.IsEnabled = true;
			OtherInputs.EnableAllInputs();
		}

		private void ActivateRunningState() {
			ConsoleOut.Text = "Simulation is running...";
			ConsoleOut.Text += $"\nMissing patients: <{SimInputs.GetMinMissingPatients()},{SimInputs.GetMaxMissingPatients()})";
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
			ConsoleOut.Text = "";
		}

		private void StopClick(object sender, RoutedEventArgs e) {
			if (OtherInputs.SelectedMode() == Mode.Classic) {
				VacCenterSim.Stop = true;
			}
			else {
				SimulationWrapper.Stop();
			}
			ActivateReadyState();
		}

		private void PauseClick(object sender, RoutedEventArgs e) {
			if (OtherInputs.SelectedMode() == Mode.Classic) {
				VacCenterSim.Pause = true;
			}
			else {
				SimulationWrapper.Pause();
			}
			ActivatePausedState();
		}

		private void ContinueClick(object sender, RoutedEventArgs e) {
			if (OtherInputs.SelectedMode() == Mode.Classic) {
				VacCenterSim.Pause = false;
			}
			else {
				SimulationWrapper.Continue();
			}
			ActivateRunningState();
		}
	}
}

