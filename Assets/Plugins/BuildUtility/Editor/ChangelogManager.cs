using UnityEngine;

namespace Opodlinok.BuildUtility.Editor
{
    /// <summary>
    /// Stub
    /// </summary>
    internal static class ChangelogManager
    {
        public static void AppendEntry(string pathFormat, string version, string entry)
        {
            Debug.Log($"[Stub] AppendEntry: {version} / {entry}");
        }

        public static void CopyChangelogToBuildFolder(string pathFormat, string buildDir)
        {
            Debug.Log($"[Stub] CopyChangelogToBuildFolder -> {buildDir}");
        }
    }
}
