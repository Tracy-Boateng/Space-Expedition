using System;
using System.IO;
using System.Text;

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

		// -------------------------
		// LOAD VAULT
		// -------------------------
		public void LoadVault(string vaultFile)
		{
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

		// -------------------------
		// SAVE SUMMARY
		// -------------------------
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
				Console.WriteLine("ERROR: Could not save expedition summary.");
			}
		}

		// -------------------------
		// VIEW INVENTORY
		// -------------------------
		public void PrintInventory()
		{
			if (count == 0)
			{
				Console.WriteLine("Inventory is empty.");
				return;
			}

			Console.WriteLine("\n--- Galactic Vault (Sorted by Decoded Name) ---");
			for (int i = 0; i < count; i++)
			{
				Console.WriteLine($"{i + 1}) {inventory[i]}");
			}
			Console.WriteLine("---------------------------------------------\n");
		}

		// -------------------------
		// SORT INVENTORY (Insertion Sort)
		// -------------------------
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

		// -------------------------
		// ADD ARTIFACT BY NAME (reads artifact file)
		// user types: "mystic_orb"
		// file: "mystic_orb.txt"
		// -------------------------
		public void AddArtifactByName(string artifactName)
		{
			string fileName = artifactName + ".txt";

			if (!File.Exists(fileName))
			{
				Console.WriteLine($"ERROR: Could not find file {fileName}");
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

				//Artifact newArtifact = ParseArtifactLine(line.Trim());
				//if (newArtifact == null)
				//{
				//	Console.WriteLine("ERROR: Could not parse artifact file.");
				//	return;
				//} 888888888888

				// Inventory is already sorted, so we can binary search
				int foundIndex = BinarySearchByDecodedName(newArtifact.DecodedName);

				if (foundIndex != -1)
				{
					Console.WriteLine("That artifact already exists in the vault. No duplicate added.");
					return;
				}

				OrderedInsert(newArtifact);
				Console.WriteLine($"Added: {newArtifact.DecodedName} (Inserted in sorted position)");
			}
			catch
			{
				Console.WriteLine("ERROR: Could not read artifact file.");
			}
		}

		// -------------------------
		// PARSING ONE LINE INTO ARTIFACT
		// Expected: encodedName | planet | discoveryDate | storageLocation | description
		// -------------------------
		//private Artifact ParseArtifactLine(string line)
		//{
		//	// Some files might use commas, but assignment says pipe separators.
		//	// So we split by '|'
		//	string[] parts = line.Split('|');

		//	// Need at least 5 parts (description can contain separators sometimes)
		//	if (parts.Length < 5)
		//		return null;

		//	string encodedName = parts[0].Trim();
		//	string planet = parts[1].Trim();
		//	string discovery = parts[2].Trim();
		//	string storage = parts[3].Trim();

		//	// Description might contain extra '|' if file is messy,
		//	// so we rebuild it from part 4 onward.
		//	string description = "";
		//	for (int i = 4; i < parts.Length; i++)
		//	{
		//		if (i > 4) description += "|";
		//		description += parts[i].Trim();
		//	}

		//	string decodedName = Decoder.DecodeFull(encodedName);

		//	return new Artifact(encodedName, decodedName, planet, discovery, storage, description);
		//}

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

		//binary search and returns index or -1
		private int BinarySearchByDecodedName(string target)
		{
			int left = 0;
			int right = count - 1;

			while (left <= right)
			{
				int mid = (left + right) / 2;
				int cmp = CompareNames(inventory[mid].DecodedName, target);

				if (cmp == 0) return mid;
				if (cmp < 0) left = mid + 1;
				else right = mid - 1;
			}

			return -1;
		}

		//ordered insert into inventory
		private void OrderedInsert(Artifact newItem)
		{
			if (count == inventory.Length)
				inventory = Grow(inventory);

			int pos = 0;
			while (pos < count && CompareNames(inventory[pos].DecodedName, newItem.DecodedName) < 0)
				pos++;

			//shift right
			for (int i = count; i > pos; i--)
				inventory[i] = inventory[i - 1];

			inventory[pos] = newItem;
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
