using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationCore {

	/// <summary>
	/// Trieda reprezentuje aktualny simulacny stav.
	/// Potomok tejto triedy by mal obsahovat vsetky informacie ktore chceme zo simulacneho jadra poslat na GUI.
	/// Po kazdom vykonanom evente by sa mali aktualizovat property tejto triedy (to ma zabezpecit potomok abstraktneho simulacneho jadra).
	/// Tato trieda je poslana na GUI aby notifikovala GUI o novych zmenach (toto zabezpecuje abstraktne simulacne jadro).
	/// Je predpoklad ze potomok tejto triedy bude obsahovat ako property typy jednotlivych statistik.
	/// </summary>
	public abstract class SimState {

		public double Time { get; set; }

		public int CurrentReplication { get; set; }

		public abstract void ResetCurrentStats();

		public abstract void ResetBeforeReplication();

		public abstract void ResetAll();

	}
}
