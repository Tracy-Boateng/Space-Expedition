namespace SpaceExpedition
{
	internal class Artifact
	{
		public string EncodedName { get; }
		public string DecodedName { get; }
		public string Planet { get; }
		public string DiscoveryDate { get; }
		public string StorageLocation { get; }
		public string Description { get; }

		public Artifact(string encodedName, string decodedName, string planet,
						string discoveryDate, string storageLocation, string description)
		{
			EncodedName = encodedName;
			DecodedName = decodedName;
			Planet = planet;
			DiscoveryDate = discoveryDate;
			StorageLocation = storageLocation;
			Description = description;
		}

		public string ToFileLine()
		{
			return $"{EncodedName} | {Planet} | {DiscoveryDate} | {StorageLocation} | {Description}";
		}

		public override string ToString()
		{
			return $"{DecodedName} | {Planet} | {DiscoveryDate} | {StorageLocation} | {Description}";
		}
	}
}
