using System;
using System.Windows.Forms;

namespace PharmacyInventorySystem
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			ApplicationConfiguration.Initialize();
			Application.Run(new UI.MainForm());
		}
	}
}


