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
        var destDirectory = Path.Combine(Application.dataPath, "LuaScripts");
        var sourceDirectory = EditorUtility.OpenFolderPanel("Select Lua Folder", "", "LuaScripts");
        if (sourceDirectory == string.Empty)
        {
            //Do nothing.
        }
        else
        {
            // Clear file in destination path without root path
            var directoryInfo = new DirectoryInfo(destDirectory);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                dir.Delete(true);
            }

            foreach (var directory in Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(directory.Replace(sourceDirectory, destDirectory));
            }

            foreach (var file in Directory.GetFiles(sourceDirectory, "*.lua", SearchOption.AllDirectories))
            {
                var destFileName = Path.ChangeExtension(file.Replace(sourceDirectory, destDirectory), ".txt");
                File.Copy(file, destFileName, true);
            }

            AssetDatabase.Refresh();
        }

    }

}