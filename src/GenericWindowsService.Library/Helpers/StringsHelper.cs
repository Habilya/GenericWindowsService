using System.Globalization;
using System.Text;

namespace GenericWindowsService.Library.Helpers;

public static class StringsHelper
{
	public static string RemoveDiacritics(string input)
	{
		var normalizedString = input.Normalize(NormalizationForm.FormD);
		var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

		foreach (var c in normalizedString)
		{
			var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
			if (unicodeCategory != UnicodeCategory.NonSpacingMark)
			{
				stringBuilder.Append(c);
			}
		}

		return stringBuilder
			.ToString()
			.Normalize(NormalizationForm.FormC);
	}
}
