using System.IO;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions.Utilities
{
    [InitializeOnLoad]
    internal static class StopPlayingAtCompile
    {
        private static readonly FileSystemWatcher Watcher = new FileSystemWatcher
        {
            Path = Application.dataPath,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
            Filter = "*.dll",
            IncludeSubdirectories = true
        };

        private static volatile bool _needStop;
        
        static StopPlayingAtCompile()
        {
            Watcher.Changed += (_, __) => _needStop = true;
            Watcher.Created += (_, __) => _needStop = true;
            Watcher.Deleted += (_, __) => _needStop = true;
            Watcher.Renamed += (_, __) => _needStop = true;
            
            EditorApplication.playmodeStateChanged += () => Watcher.EnableRaisingEvents = EditorApplication.isPlaying;
            EditorApplication.update += () => {
                if (EditorApplication.isPlaying && (_needStop || EditorApplication.isCompiling))
                {
                    EditorApplication.isPlaying = _needStop = false;
                }
            };
        }
    }
}
