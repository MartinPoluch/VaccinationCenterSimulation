using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Priority_Queue;

namespace SimulationCore {
	public abstract class SimCore {

		public static readonly double StartTime = 0; // zaciatok simulacneho behu

		private SimplePriorityQueue<SimEvent, double> _calendar;
		private BackgroundWorker _worker;
		
		protected SimCore() {
			Stop = false;
			Pause = false;
			ReportProgressReplicationFrequency = 1;
			MaximumSpeed = true;
			WarmUpDuration = 0;
			_calendar = new SimplePriorityQueue<SimEvent, double>();
			CurrentTime = StartTime;
			_worker = new BackgroundWorker() {
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};
			AnimationFrequency = 1;
			AnimationDuration = 1000;
		}

		public bool MaximumSpeed { get; set; }

		public int ReportProgressReplicationFrequency { get; set; }

		public double WarmUpDuration { get; set; }

		public int AnimationFrequency { get; set; }

		public int AnimationDuration { get; set; }

		public double CurrentTime { get; private set; }

		public int CurrentReplication { get; set; }

		public bool Pause { get; set; }

		public bool Stop { get; set; }

		public void PlanEvent(SimEvent e) {
			_calendar.Enqueue(e, e.Time);
		}

		public void RegisterRefreshDuringSimulation(ProgressChangedEventHandler handler) {
			_worker.ProgressChanged += handler;
		}

		public void RegisterRefreshAfterSimulation(RunWorkerCompletedEventHandler handler) {
			_worker.RunWorkerCompleted += handler;
		}

		protected abstract void BeforeSimulation();

		protected abstract void BeforeReplication();

		protected abstract void AfterSimulation();

		protected abstract void AfterReplication();

		public abstract SimState GetCurrentState();

		private void PlanInitializationEvents() {
			if (!MaximumSpeed) {
				PlanEvent(new AnimationEvent(this, StartTime)); // pocas zahrievania nechcem nikdy simulaciu spomalovat
			}
			
		}

		private void Simulate(double endTime, int replications, bool reportProgress) {
			_calendar.Clear();
			CurrentTime = StartTime;
			Stop = false;
			BeforeSimulation();
			int replication;
			for (replication = 1; (replication <= replications) && (! Stop); replication++) {
				CurrentReplication = replication;
				CurrentTime = StartTime;
				PlanInitializationEvents();
				BeforeReplication();
				while ((CurrentTime <= endTime) && (!Stop) && (_calendar.Count > 0)) {
					while (Pause) {
						Thread.Sleep(300);
					}

					SimEvent currentEvent = _calendar.Dequeue();
					Debug.Assert(currentEvent.Time >= CurrentTime, "Time traveling!");
					Debug.Assert(currentEvent.Time >= 0, "Time is negative!, maybe it was not initialized.");
					CurrentTime = currentEvent.Time;
					currentEvent.Execute();

					if (! MaximumSpeed && (CurrentTime >= WarmUpDuration) && reportProgress) {
						ReportProgress(); // aktualizacia GUI pri pomalom rezime
					}
				}
				AfterReplication();

				if ((replication % ReportProgressReplicationFrequency == 0) && reportProgress) { // aby sa aktualizovalo gui aj v pripade rychleho rezimu
					ReportProgress();
				}
				_calendar.Clear();
			}

			if (reportProgress) {
				ReportProgress(replication - 1); // na konci simulacie sa GUI vzdy refresne
			}

			AfterSimulation();
		}

		private void ReportProgress(int replication) {
			SimState currentState = GetCurrentState();
			currentState.Time = CurrentTime;
			currentState.CurrentReplication = replication;
			_worker.ReportProgress(1, currentState);
		}

		internal void ReportProgress() {
			ReportProgress(CurrentReplication);
		}

		public void Simulate(double endTime, int replications = 1) {
			Simulate(endTime, replications, false);
		}

		public void SimulateAsync(double endTime, int replications = 1) {
			_worker.DoWork += delegate(object sender, DoWorkEventArgs args) {
				try {
					Simulate(endTime, replications, true);
				}
				catch (Exception e) {
					Console.WriteLine(e.Message);
					Console.WriteLine(e.StackTrace);
				}
			};
			_worker.RunWorkerAsync();
		}
	}

	
}