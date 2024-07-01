using FluentAssertions;
using GenericWindowsService.Library.Helpers;

namespace GenericWindowsService.Library.Tests.Unit;

public class StringsHelperTests
{
	[Theory]
	[InlineData("été", "ete")]
	[InlineData("Aujourd'hui", "Aujourd'hui")]
	[InlineData("The Meaning of Ça", "The Meaning of Ca")]
	[InlineData("ÉTÉ", "ETE")]
	[InlineData("Allô", "Allo")]
	[InlineData("Noël", "Noel")]
	[InlineData("NOËL", "NOEL")]
	[InlineData("ça", "ca")]
	[InlineData("Ça", "Ca")]
	[InlineData("è", "e")]
	[InlineData("être", "etre")]
	[InlineData("Maïs", "Mais")]
	public void RemoveDiacritics_ShouldRemoveDiacritics_WhenDiacriticsPresentInString(string input, string expected)
	{
		// Arrange

		// Act
		var actual = StringsHelper.RemoveDiacritics(input);

		// Assert
		actual.Should().Be(expected);
	}
}
