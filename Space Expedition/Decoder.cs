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

		xx
	}

	return result;
}
