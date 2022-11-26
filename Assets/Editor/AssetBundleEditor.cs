using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleEditor
{
    [MenuItem("Custom/Build Asset Bundles")]
    static void BuildAssetBundles()
    {
        // var sourceFolder = "";
        // var sourceDefaultName = "LuaScripts";
        // var destinationFolder = Path.Combine(Application.dataPath, "LuaScripts");
        // CopyLuaScripts(sourceFolder, sourceDefaultName, destinationFolder);
        // BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Custom/Copy Lua Script To Directory")]
    static void CopyLuaScripts2Directory()
    {
        var destinationFolder = Path.Combine(Application.dataPath, "LuaScripts");
        var sourcePath = EditorUtility.OpenFolderPanel("Select Lua Folder", "", "LuaScripts");
        DirectoryCopy(sourcePath, destinationFolder);
        AssetDatabase.Refresh();
    }

    private static void DirectoryCopy(string sourceDirectoryName, string destDirectoryName)
    {
        // If the destination directory does not exist, create it.
        if (!Directory.Exists(destDirectoryName))
        {
            Directory.CreateDirectory(destDirectoryName);
        }
        else
        {
            //Do nothing.
        }

        // If the source directory does not exist, throw an exception.
        if (!Directory.Exists(sourceDirectoryName))
        {
            throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDirectoryName}");
        }
        else
        {
            // Get the file contents of the directory to copy.
            var files = Directory.GetFiles(sourceDirectoryName, "*.lua");
            // Debug.Log($"{sourceDirectoryName} File Count:{files.Length}");
            foreach (var file in files)
            {
                var destFileName = Path.Combine(destDirectoryName, $"{Path.GetFileNameWithoutExtension(file)}.txt");
                // Debug.Log($"Copy to {destFileName}");
                File.Copy(file, destFileName);
            }

            var directories = Directory.GetDirectories(sourceDirectoryName);
            foreach (var directory in directories)
            {
                DirectoryCopy(directory, Path.Combine(destDirectoryName, directory));
            }
        }

    }
}