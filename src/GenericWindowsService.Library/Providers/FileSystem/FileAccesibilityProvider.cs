using GenericWindowsService.Library.Helpers;

namespace GenericWindowsService.Library.Providers.FileSystem;

public enum FileCheckResult
{
	FileAccessible = 0,
	FileNotAccessibleAttemptsWithinLimit = 1,
	FileNotAccessibleAttemptsExceeded = 2
}


public class FileAccesibilityProvider
{
	public List<FileAccesibleRecordModel> FileAccesibleRecords { get; private set; }

	public FileAccesibilityProvider()
	{
		FileAccesibleRecords = new List<FileAccesibleRecordModel>();
	}

	public FileCheckResult CheckFile(string fileFullPath, int attemptsLimit, out int nbAttempt)
	{
		var fileAccesibleRecord = GetFileAccessibleRecord(fileFullPath);

		// Check for file accesibility
		fileAccesibleRecord.IsAccessible = IsFileAccessible(fileFullPath);

		if (fileAccesibleRecord.IsAccessible)
		{
			RemoveFileAccessibleRecordIfExists(fileAccesibleRecord);
			nbAttempt = -1;
			return FileCheckResult.FileAccessible;
		}
		else
		{
			fileAccesibleRecord.AttemptsCount++;
			nbAttempt = fileAccesibleRecord.AttemptsCount;
		}

		if (fileAccesibleRecord.AttemptsCount >= attemptsLimit)
		{
			RemoveFileAccessibleRecordIfExists(fileAccesibleRecord);
			return FileCheckResult.FileNotAccessibleAttemptsExceeded;
		}
		else
		{
			StoreFileAccessibleRecord(fileAccesibleRecord);
			return FileCheckResult.FileNotAccessibleAttemptsWithinLimit;
		}
	}

	private bool IsFileAccessible(string fileFullPath)
	{
		return !FileSystemHelper.IsFileLocked(new FileInfo(fileFullPath));
	}

	private void StoreFileAccessibleRecord(FileAccesibleRecordModel fileAccesibleRecord)
	{
		var lookupInStoredFileRecords = FileAccesibleRecords
			.Where(q => q.FileFullPath.Equals(fileAccesibleRecord.FileFullPath))
			.ToList();

		if (lookupInStoredFileRecords.Any())
		{
			RemoveFileAccessibleRecordIfExists(fileAccesibleRecord);
		}

		FileAccesibleRecords.Add(fileAccesibleRecord);
	}

	private void RemoveFileAccessibleRecordIfExists(FileAccesibleRecordModel fileAccesibleRecord)
	{
		FileAccesibleRecords.RemoveAll(q => q.FileFullPath.Equals(fileAccesibleRecord.FileFullPath));
	}

	private FileAccesibleRecordModel GetFileAccessibleRecord(string fileFullPath)
	{
		FileAccesibleRecordModel fileAccesibleRecord;

		var lookupInStoredFileRecords = FileAccesibleRecords
			.Where(q => q.FileFullPath.Equals(fileFullPath))
			.ToList();

		if (lookupInStoredFileRecords.Any())
		{
			fileAccesibleRecord = lookupInStoredFileRecords.First();
		}
		else
		{
			fileAccesibleRecord = new FileAccesibleRecordModel(fileFullPath);
		}

		return fileAccesibleRecord;
	}
}
