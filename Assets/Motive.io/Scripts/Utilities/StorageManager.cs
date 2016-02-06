using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// Helper that establishes the structure of the file system.
/// </summary>
public class StorageManager {
    public static string EnsureFolder(params string[] path)
    {
        var curr = Application.persistentDataPath;

        foreach (var p in path)
        {
            curr = Path.Combine(curr, p);
        }

        Directory.CreateDirectory(curr);

        return curr;
    }

    public static string EnsureGameFolder(string folder)
    {
        return EnsureFolder("gameData", folder);
    }

    public static string EnsureCatalogsFolder(string folder)
    {
        return EnsureFolder("catalogs");
    }

    public static string GetCatalogFileName(string fileName)
    {
        return GetFilePath("catalogs", fileName);
    }

    public static string GetGameFileName(string file)
    {
        return GetFilePath("gameData", file);
    }

    public static string GetGameFileName(string folder, string file)
    {
        return GetFilePath(new string[] { "gameData", folder, file });
    }

	public static void DeleteFolder(string folderName)
	{
		var path = Path.Combine(Application.persistentDataPath, folderName);

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
	}

    public static string GetFilePath(params string[] filePath)
    {
        var curr = Application.persistentDataPath;

        for (int i = 0; i < filePath.Length; i++ )
        {
            var p = filePath[i];

            curr = Path.Combine(curr, p);

            if (i < filePath.Length - 1)
            {
                Directory.CreateDirectory(curr);
            }
        }

        return curr;
    }

    internal static void DeleteGameFolder()
    {
        DeleteFolder("gameData");
    }
}
