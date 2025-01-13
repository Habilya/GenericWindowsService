﻿using HeyRed.Mime;

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

	public static string AdjustDirectoryNameIfDuplicate(string directoryPath)
	{
		var count = 1;
		var newFullPath = directoryPath;

		while (Directory.Exists(newFullPath))
		{
			newFullPath = string.Format("{0} ({1})", directoryPath, ++count);
		}

		return newFullPath;
	}

	public static string GuessMimeTypeFromFileName(string fileName)
	{
		return MimeTypesMap.GetMimeType(fileName);
	}

	public static void MoveFilesAndSubFolders(string fromFolder, string toFolder)
	{
		// Get Files & Move
		string[] files = Directory.GetFiles(fromFolder);
		foreach (string file in files)
		{
			string name = Path.GetFileName(file);
			string dest = Path.Combine(toFolder, name);
			dest = AdjustFileNameIfDuplicate(dest);
			File.Move(file, dest);
		}

		// Get folders & Move
		string[] folders = Directory.GetDirectories(fromFolder);
		foreach (string folder in folders)
		{
			string name = Path.GetFileName(folder);
			string dest = Path.Combine(toFolder, name);
			Directory.Move(folder, dest);
		}
	}

	public static string CheckForShortcut(string fileFullPath)
	{
		return Path.GetExtension(fileFullPath).Equals(".lnk", StringComparison.InvariantCultureIgnoreCase)
			? GetShortcutTargetPath(fileFullPath)
			: fileFullPath;
	}

	public static string GetShortcutTargetPath(string shortcutFilePath)
	{
		if (!shortcutFilePath.ToLower().EndsWith(".lnk"))
		{
			return "";
		}

		var shell = new IWshRuntimeLibrary.WshShell();
		var shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutFilePath);

		return shortcut.TargetPath;
	}

	public static bool IsFileLocked(FileInfo file)
	{
		try
		{
			using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
			{
				stream.Close();
			}
		}
		catch (IOException)
		{
			//the file is unavailable because it is:
			//still being written to
			//or being processed by another thread
			//or does not exist (has already been processed)
			return true;
		}

		//file is not locked
		return false;
	}

	public static string AdjustFileNameIfLocked(string fileFullPath)
	{
		int count = 1;

		string fileNameOnly = Path.GetFileNameWithoutExtension(fileFullPath);
		string extension = Path.GetExtension(fileFullPath);
		string path = Path.GetDirectoryName(fileFullPath)!;
		string newFullPath = fileFullPath;

		while (File.Exists(newFullPath) && IsFileLocked(new FileInfo(newFullPath)))
		{
			string tempFileName = string.Format("{0}_WasLocked({1})", fileNameOnly, count++);
			newFullPath = Path.Combine(path, tempFileName + extension);
		}
		return newFullPath;
	}
}
