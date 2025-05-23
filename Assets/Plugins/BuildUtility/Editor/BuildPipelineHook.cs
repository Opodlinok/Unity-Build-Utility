using System;
using UnityEditor;

namespace Opodlinok.BuildUtility.Editor
{
    [InitializeOnLoad]
    static class BuildPipelineHook
    {
        private static readonly Action<BuildPlayerOptions> k_DefaultHandler =
            BuildPlayerWindow.DefaultBuildMethods.BuildPlayer;

        static BuildPipelineHook()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(OnBuildPlayer);
        }

        private static void OnBuildPlayer(BuildPlayerOptions options)
        {
            var settings = BuildUtilitySettingsProvider.Settings;
            if (settings.ShowPromptOnBuildAndRun)
            {
                VersionPromptWindow.ShowWindow(options, k_DefaultHandler);
            }
            else
            {
                k_DefaultHandler(options);
            }
        }
    }
}
