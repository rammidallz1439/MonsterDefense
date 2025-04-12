using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public class PersistentDataPathOpener : EditorWindow
{
    [MenuItem("Tools/Open Persistent Data Path")]
    private static void OpenPersistentDataPath()
    {
        string path = Application.persistentDataPath;

        if (Directory.Exists(path))
        {
#if UNITY_EDITOR_WIN
            Process.Start("explorer.exe", path.Replace("/", "\\"));
#elif UNITY_EDITOR_OSX
            Process.Start("open", path);
#elif UNITY_EDITOR_LINUX
            Process.Start("xdg-open", path);
#else
            UnityEngine.Debug.Log($"PersistentDataPath: {path}");
#endif
        }
        else
        {
            UnityEngine.Debug.LogError($"PersistentDataPath not found: {path}");
        }
    }
}
