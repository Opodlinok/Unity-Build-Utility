using System;
using System.IO;
using UnityEngine;

namespace Opodlinok.BuildUtility.Runtime
{
    [DefaultExecutionOrder(-100)]
    public class VersionOverlay : MonoBehaviour
    {
        private enum OverlayPosition { TopLeft, TopRight, BottomLeft, BottomRight }

        [Serializable]
        private class BuildConfig
        {
            public bool releaseBuild;
            public bool enableOverlay;
            public string overlayPosition;
            public bool showStudioLabel;
        }

        private static bool s_ShouldDraw = true;
        private static OverlayPosition s_OverlayPosition = OverlayPosition.TopLeft;
        private static bool s_ShowStudioLabel = true;
        private static bool s_Initialized = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var root    = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
            var cfgPath = Path.Combine(root, "BuildUtilityConfig.json");

            if (File.Exists(cfgPath))
            {
                try
                {
                    var json = File.ReadAllText(cfgPath);
                    var cfg = JsonUtility.FromJson<BuildConfig>(json);

                    if (cfg.releaseBuild || !cfg.enableOverlay)
                        s_ShouldDraw = false;

                    if (Enum.TryParse(cfg.overlayPosition, out OverlayPosition pos))
                        s_OverlayPosition = pos;

                    s_ShowStudioLabel = cfg.showStudioLabel;
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"VersionOverlay: failed to read config: {e.Message}");
                }
            }

            s_Initialized = true;
        }

        private void OnGUI()
        {
            if (!s_Initialized || !s_ShouldDraw)
                return;

            var version = Application.version;
            var text = s_ShowStudioLabel
                ? $"{Application.companyName}\n{version}"
                : version;

            var style = GUI.skin.box;
            style.alignment = TextAnchor.UpperLeft;

            Rect rect = s_OverlayPosition switch
            {
                OverlayPosition.TopLeft => new Rect(  10,  10, 200, 50),
                OverlayPosition.TopRight => new Rect(Screen.width - 210,  10, 200, 50),
                OverlayPosition.BottomLeft => new Rect(  10, Screen.height - 60, 200, 50),
                OverlayPosition.BottomRight => new Rect(Screen.width - 210, Screen.height - 60, 200, 50),
                _ => new Rect(  10,  10, 200, 50),
            };

            GUI.Box(rect, text, style);
        }
    }
}
