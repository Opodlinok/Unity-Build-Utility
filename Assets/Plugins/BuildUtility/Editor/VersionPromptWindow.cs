using System;
using UnityEditor;

namespace Opodlinok.BuildUtility.Editor
{
    /// <summary>
    /// Stub
    /// </summary>
    internal static class VersionPromptWindow
    {
        public static void ShowWindow(
            BuildPlayerOptions options,
            Action<BuildPlayerOptions> defaultHandler)
        {
            defaultHandler(options);
        }
    }
}
