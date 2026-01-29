namespace SpaceExpedition
{
	internal static class Decoder
	{
		public static string DecodeFull(string encodedName)
		{
			string result = "";
			int i = 0;

			while (i < encodedName.Length)
			{
				// Skip separators
				while (i < encodedName.Length && (encodedName[i] == '|' || encodedName[i] == ' '))
					i++;

				if (i >= encodedName.Length) break;

				char letter = encodedName[i];

				if (letter < 'A' || letter > 'Z')
				{
					i++;
					continue;
				}

				i++;

				//reading digits after the letter
				int level = 0;
				while (i < encodedName.Length && encodedName[i] >= '0' && encodedName[i] <= '9')
				{
					level = (level * 10) + (encodedName[i] - '0');
					i++;
				}

				if (level == 0) level = 1;

				char decodedChar = DecodeChar(letter, level);
				result += decodedChar;
			}

			return result;
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
