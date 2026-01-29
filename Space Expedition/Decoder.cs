namespace SpaceExpedition
{
	internal static class Decoder
	{
		public static string DecodeFull(string encodedName)
		{
			string decodedName = "";
			int index = 0;

			while (index < encodedName.Length)
			{
				if (encodedName[index] == '|' || encodedName[index] == ' ')
				{
					index++;
					continue;
				}

				char letter = encodedName[index];

				if (letter < 'A' || letter > 'Z')
				{
					index++;
					continue;
				}

				index++; 

				int level = 0;
				while (index < encodedName.Length &&
					   encodedName[index] >= '0' &&
					   encodedName[index] <= '9')
				{
					level = level * 10 + (encodedName[index] - '0');
					index++;
				}

				if (level == 0)
					level = 1;

				char decodedChar = DecodeChar(letter, level);

				decodedName += decodedChar;
			}

			return decodedName;
		}


		private static char DecodeChar(char letter, int level)
		{
			if (level <= 1)
				return Mirror(letter);

			char mapped = MapLetter(letter);
			return DecodeChar(mapped, level - 1);
		}

		private static char MapLetter(char letter)
		{
			char[] original =
			{
				'A','B','C','D','E','F','G','H','I','J','K','L','M',
				'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
			};

			char[] mapped =
			{
				'H','Z','A','U','Y','E','K','G','O','T','I','R','J',
				'V','W','N','M','F','Q','S','D','B','X','L','C','P'
			};

			for (int i = 0; i < original.Length; i++)
			{
				if (original[i] == letter)
					return mapped[i];
			}

			return letter;
		}

		private static char Mirror(char letter)
		{
			int index = letter - 'A';
			int mirrored = 25 - index;
			return (char)('A' + mirrored);
		}
	}
}
