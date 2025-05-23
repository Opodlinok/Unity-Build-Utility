using UnityEngine;

namespace Opodlinok.BuildUtility
{
    public enum VersionTag { None, Alpha, Beta, Rc }
    public enum OverlayPosition { TopLeft, TopRight, BottomLeft, BottomRight }

    public class BuildUtilitySettings : ScriptableObject
    {
        [Header("Versioning")]
        public string InitialVersion = "0.1.0";
        public string ChangelogPathFormat = "/{0}-CHANGELOG.md";

        [Header("Changelog Copy")]
        public bool CopyChangelogToBuildFolder = false;

        [Header("Version Prompt Defaults")]
        public VersionTag LastSelectedTag = VersionTag.None;
        public bool ShowPromptOnBuildAndRun = true;

        [Header("Runtime Overlay")]
        public bool EnableOverlay = true;
        public OverlayPosition OverlayPosition = OverlayPosition.TopRight;
        public bool ShowStudioLabel = true;
    }
}
