namespace SpaceExpedition
{
	internal class Program
	{
		static void Main()
		{
			VaultManager vault = new VaultManager();
			vault.LoadVault("galactic_vault.txt");
			vault.SortInventory(); 

			Menu menu = new Menu(vault);
			menu.Run();
		}
	}
}
