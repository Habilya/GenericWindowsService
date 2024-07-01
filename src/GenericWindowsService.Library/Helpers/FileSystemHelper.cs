using HeyRed.Mime;

namespace GenericWindowsService.Library.Helpers;

public static class FileSystemHelper
{
	public static string AdjustFileNameIfDuplicate(string filefullPath)
	{
		var count = 1;
		var newFullPath = filefullPath;

		while (File.Exists(newFullPath))
		{
			var fileNameOnly = Path.GetFileNameWithoutExtension(filefullPath);
			var extension = Path.GetExtension(filefullPath);
			var pathOnly = Path.GetDirectoryName(filefullPath);

			var tempFileName = string.Format("{0} ({1})", fileNameOnly, ++count);
			newFullPath = Path.Combine(pathOnly!, tempFileName + extension);
		}

		return newFullPath;
	}

	public static string GuessMimeTypeFromFileName(string fileName)
	{
		return MimeTypesMap.GetMimeType(fileName);
	}
}
