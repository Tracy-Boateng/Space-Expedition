using System;
using System.IO;
namespace SpaceExpedition
{
	internal class VaultManager
	{
		private Artifact[] inventory;
		private int count;

		public VaultManager()
		{
			inventory = new Artifact[10];
			count = 0;
		}

		public int Count => count;

		//load vault
		public void LoadVault(string vaultFile)
		{
			vaultFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, vaultFile);

			if (!File.Exists(vaultFile))
			{
				Console.WriteLine($"ERROR: Could not find {vaultFile}");
				return;
			}

			try
			{
				using (StreamReader reader = new StreamReader(vaultFile))
				{
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						line = line.Trim();
						if (line.Length == 0) continue;

						Artifact a = ParseArtifactLine(line);
						if (a != null)
							AddToEnd(a);
					}
				}
			}
			catch
			{
				Console.WriteLine("ERROR: Could not read the vault file.");
			}
		}


		//save summary
		public void SaveSummary(string outputFile)
		{
			try
			{
				using (StreamWriter writer = new StreamWriter(outputFile))
				{
					for (int i = 0; i < count; i++)
						writer.WriteLine(inventory[i].ToFileLine());
				}

				Console.WriteLine($"Saved to {outputFile}");
			}
			catch
			{
				Console.WriteLine("ERROR: The expedition summary was not saved.");
			}
		}

		//view inventory
		public void PrintInventory()
		{
			if (count == 0)
			{
				Console.WriteLine("This inventory is empty.");
				return;
			}

			Console.WriteLine("\n *Galactic Vault -> Sorted by the Decoded Name* ");
			for (int i = 0; i < count; i++)
			{
				Console.WriteLine($"{i + 1}) {inventory[i]}");
			}
			Console.WriteLine();
		}


		//insertion sort
		public void SortInventory()
		{
			for (int i = 1; i < count; i++)
			{
				Artifact key = inventory[i];
				int j = i - 1;

				while (j >= 0 && CompareNames(inventory[j].DecodedName, key.DecodedName) > 0)
				{
					inventory[j + 1] = inventory[j];
					j--;
				}

				inventory[j + 1] = key;
			}
		}

		//add artifact by name 
		public void AddArtifactByName(string artifactName)
		{
			string fileName = artifactName + ".txt";

			if (!File.Exists(fileName))
			{
				Console.WriteLine($"ERROR: {fileName} was not found");
				return;
			}

			try
			{
				string line;
				using (StreamReader reader = new StreamReader(fileName))
				{
					line = reader.ReadLine();
				}

				if (string.IsNullOrWhiteSpace(line))
				{
					Console.WriteLine("ERROR: Artifact file is empty or invalid.");
					return;
				}

				Artifact newArtifact = ParseArtifactLine(line.Trim());
				if (newArtifact == null)
				{
					Console.WriteLine("ERROR: Could not parse artifact file.");
					return;
				}

				//inventory is sorted -> binary search
				int foundIndex = BinarySearchByDecodedName(newArtifact.DecodedName);

				if (foundIndex != -1)
				{
					Console.WriteLine("Artifact already exists in the vault. No duplicate added.");
					return;
				}

				OrderedInsert(newArtifact);
				Console.WriteLine($"Added: {newArtifact.DecodedName} and Inserted in sorted position");
			}
			catch
			{
				Console.WriteLine("ERROR: Could not read artifact file.");
			}
		}

		//parsing one line into artifact
		private Artifact ParseArtifactLine(string line)
		{
			string[] parts = SplitByFirstCommas(line, 4);

			if (parts == null) return null;

			string encodedName = parts[0].Trim();
			string planet = parts[1].Trim();
			string discovery = parts[2].Trim();
			string storage = parts[3].Trim();
			string description = parts[4].Trim();

			string decodedName = Decoder.DecodeFull(encodedName);

			return new Artifact(encodedName, decodedName, planet, discovery, storage, description);
		}

		private string[] SplitByFirstCommas(string line, int commasToSplit)
		{
			string[] result = new string[commasToSplit + 1];

			int start = 0;
			int partIndex = 0;

			for (int i = 0; i < line.Length && partIndex < commasToSplit; i++)
			{
				if (line[i] == ',')
				{
					result[partIndex] = line.Substring(start, i - start);
					partIndex++;
					start = i + 1;
				}
			}

			
			result[partIndex] = line.Substring(start);

			
			if (partIndex != commasToSplit) return null;

			return result;
		}

		//array helpers
		private void AddToEnd(Artifact a)
		{
			if (count == inventory.Length)
				inventory = Grow(inventory);

			inventory[count] = a;
			count++;
		}

		private static Artifact[] Grow(Artifact[] oldArr)
		{
			int newSize = oldArr.Length * 2;
			Artifact[] newArr = new Artifact[newSize];

			for (int i = 0; i < oldArr.Length; i++)
				newArr[i] = oldArr[i];

			return newArr;
		}

		//binary search
		private int BinarySearchByDecodedName(string target)
		{
			int left = 0;
			int right = count - 1;

			while (left <= right)
			{
				int middle = (left + right) / 2;

				int comparison =
					CompareNames(inventory[middle].DecodedName, target);

				if (comparison == 0)
				{
					return middle;
				}

				if (comparison < 0)
				{
					left = middle + 1;
				}
				else
				{
					right = middle - 1;
				}
			}

			return -1;
		}


		//ordered insert into inventory
		private void OrderedInsert(Artifact newItem)
		{
			if (count == inventory.Length)
			{
				inventory = Grow(inventory);
			}

			int insertIndex = 0;

			while (insertIndex < count &&
				   CompareNames(inventory[insertIndex].DecodedName, newItem.DecodedName) < 0)
			{
				insertIndex++;
			}

			for (int i = count; i > insertIndex; i--)
			{
				inventory[i] = inventory[i - 1];
			}

			inventory[insertIndex] = newItem;
			count++;
		}



		private int CompareNames(string a, string b)
		{
			a = a.ToLower();
			b = b.ToLower();
			return string.Compare(a, b);
		}
	}
}
