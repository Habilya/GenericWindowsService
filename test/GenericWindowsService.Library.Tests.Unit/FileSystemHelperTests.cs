using FluentAssertions;
using GenericWindowsService.Library.Helpers;

namespace GenericWindowsService.Library.Tests.Unit;

public class FileSystemHelperTests
{
	[Theory]
	[InlineData("test.html", "text/html")]
	[InlineData("test.pdf", "application/pdf")]
	[InlineData("test.js", "application/javascript")]
	[InlineData("test.php", "application/octet-stream")]
	[InlineData("test.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
	[InlineData("test.xls", "application/vnd.ms-excel")]
	[InlineData("test.rtf", "application/rtf")]
	[InlineData("test.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
	[InlineData("test.doc", "application/msword")]
	[InlineData("test.csv", "text/csv")]
	[InlineData("test.avi", "video/x-msvideo")]
	[InlineData("test.txt", "text/plain")]
	[InlineData("test", "application/octet-stream")]
	[InlineData("test.", "application/octet-stream")]
	public void GuessMimeTypeFromFileName_ShouldReturnMimeType_WhenFileNameIsValid(string input, string expected)
	{
		// Arrange

		// Act
		var actual = FileSystemHelper.GuessMimeTypeFromFileName(input);

		// Assert
		actual.Should().Be(expected);
	}

	[Theory]
	[InlineData("file.txt", "file (2).txt")]
	[InlineData("another_file.txt", "another_file (3).txt")]
	// Should be the same as there is no duplicates in the tested folder
	[InlineData("different_file.txt", "different_file.txt")]
	public void AdjustFileNameIfDuplicate_ShouldAdjustFileName_WhenDuplicatePresent(string input, string expected)
	{
		// Arrange
		var filefullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileDuplicatesTests", input);

		// Act
		var actual = FileSystemHelper.AdjustFileNameIfDuplicate(filefullPath);
		actual = Path.GetFileName(actual);

		// Assert
		actual.Should().Be(expected);
	}

	[Theory]
	[InlineData("SomeFolder", "SomeFolder (2)")]
	[InlineData("AnotherFolder", "AnotherFolder (3)")]
	// Should be the same as there is no duplicates in the tested folder
	[InlineData("DifferentFolder", "DifferentFolder")]
	public void AdjustFolderNameIfDuplicate_ShouldAdjustFolderName_WhenDuplicatePresent(string input, string expected)
	{
		// Arrange
		var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FoldersDuplicatesTests", input);

		// Act
		var actual = FileSystemHelper.AdjustDirectoryNameIfDuplicate(folderPath);
		actual = new DirectoryInfo(actual).Name;

		// Assert
		actual.Should().Be(expected);
	}
}
