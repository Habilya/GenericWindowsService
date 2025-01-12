namespace GenericWindowsService.Library.Providers.FileSystem;

public class FileAccesibleRecordModel
{
	public FileAccesibleRecordModel(string fileFullPath)
	{
		FileFullPath = fileFullPath;
	}

	public string FileFullPath { get; set; }
	public int AttemptsCount { get; set; } = 0;
	public bool IsAccessible { get; set; } = false;
}
