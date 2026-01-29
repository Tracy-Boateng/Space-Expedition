public static string DecodeFull(string encodedName)
{
	string result = "";
	int i = 0;

	while (i < encodedName.Length)
	{

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
