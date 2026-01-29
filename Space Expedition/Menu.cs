using System;

namespace SpaceExpedition
{
	internal class Menu
	{
		private VaultManager vault;

		public Menu(VaultManager vault)
		{
			this.vault = vault;
		}

		public void Run()
		{
			while (true)
			{
				Console.WriteLine("SPACE EXPEDITION -> GALACTIC VAULT");
				Console.WriteLine("1) Add Artifact from journey log file");
				Console.WriteLine("2) View Inventory");
				Console.WriteLine("0) Save and Exit");
				Console.Write("Choose: ");

				string choice = Console.ReadLine();

				if (choice == "1")
				{
					Console.Write("Type the artifact file name without .extension: ");
					string name = Console.ReadLine().Trim();

					if (name.Length == 0)
					{
						Console.WriteLine("Enter a valid name.");
					}
					else
					{
						vault.AddArtifactByName(name);
					}

					Console.WriteLine();
				}
				else if (choice == "2")
				{
					vault.PrintInventory();
				}
				else if (choice == "0")
				{
					vault.SaveSummary("expedition_summary.txt");
					Console.WriteLine("Exit explorer.");
					break;
				}
				else
				{
					Console.WriteLine("Option is invalid.\n");
				}
			}
		}
	}
}
