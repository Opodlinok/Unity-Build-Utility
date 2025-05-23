using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Opodlinok.BuildUtility.Editor
{
    class VersionPromptWindow : EditorWindow
    {
        private BuildPlayerOptions _opts;
        private Action<BuildPlayerOptions> _defaultHandler;
        private string _version;
        private VersionTag _tag;
        private string _changelog;
        private bool _releaseBuild;
        private BuildUtilitySettings _settings;
        
        public static void ShowWindow(
            BuildPlayerOptions opts,
            Action<BuildPlayerOptions> defaultHandler)
        {
            var w = GetWindow<VersionPromptWindow>("Build Utility");
            w._opts = opts;
            w._defaultHandler = defaultHandler;
            w.Init();
            w.ShowUtility();
        }

        private void Init()
        {
            _settings = BuildUtilitySettingsProvider.Settings;
            _version = PlayerSettings.bundleVersion;
            if (string.IsNullOrEmpty(_version) || _version == "1.0.0")
                _version = _settings.InitialVersion;
            _tag = _settings.LastSelectedTag;
            _changelog = "";
            _releaseBuild = false;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Version", EditorStyles.boldLabel);
            _version = EditorGUILayout.TextField(_version);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tag", EditorStyles.boldLabel);
            _tag = (BuildUtility.VersionTag)EditorGUILayout.EnumPopup(_tag);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Changelog", EditorStyles.boldLabel);
            _changelog = EditorGUILayout.TextArea(_changelog, GUILayout.Height(80));

            EditorGUILayout.Space();
            _releaseBuild = EditorGUILayout.Toggle("Release Build", _releaseBuild);
            if (_releaseBuild)
                EditorGUILayout.HelpBox(
                    "⚠ Release build: skip changelog copy and disable overlay",
                    MessageType.Warning);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Build")) { Execute(); Close(); }
            if (GUILayout.Button("Cancel")) { Cancel(); Close(); }
            EditorGUILayout.EndHorizontal();
        }

        private void Execute()
        {
            _settings.LastSelectedTag = _tag;
            EditorUtility.SetDirty(_settings);
            AssetDatabase.SaveAssets();

            var suffix = _tag == BuildUtility.VersionTag.None
                ? "" : "-" + _tag.ToString().ToLower();
            var full = _version + suffix;
            PlayerSettings.bundleVersion = full;

            ChangelogManager.AppendEntry(
                _settings.ChangelogPathFormat, full, _changelog);

            var buildDir = Path.GetDirectoryName(_opts.locationPathName);

            if (!_releaseBuild && _settings.CopyChangelogToBuildFolder)
                ChangelogManager.CopyChangelogToBuildFolder(
                    _settings.ChangelogPathFormat, buildDir);

            var config = new
            {
                releaseBuild = _releaseBuild,
                enableOverlay = _settings.EnableOverlay,
                overlayPosition = _settings.OverlayPosition.ToString(),
                showStudioLabel = _settings.ShowStudioLabel
            };

            File.WriteAllText(
                Path.Combine(buildDir, "BuildUtilityConfig.json"),
                JsonUtility.ToJson(config));

            _defaultHandler?.Invoke(_opts);
        }

        private void Cancel()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(_defaultHandler);
        }
    }
}
