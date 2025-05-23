using UnityEditor;
using UnityEngine;

namespace Opodlinok.BuildUtility.Editor
{
    static class BuildUtilitySettingsProvider
    {
        private const string k_Path = "ProjectSettings/BuildUtilitySettings.asset";
        private static BuildUtilitySettings s_Instance;

        public static BuildUtilitySettings Settings
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = AssetDatabase.LoadAssetAtPath<BuildUtilitySettings>(k_Path);
                    if (s_Instance == null)
                    {
                        s_Instance = ScriptableObject.CreateInstance<BuildUtilitySettings>();
                        AssetDatabase.CreateAsset(s_Instance, k_Path);
                        AssetDatabase.SaveAssets();
                    }
                }
                return s_Instance;
            }
        }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            var provider = new SettingsProvider("Project/Build Utility", SettingsScope.Project)
            {
                label = "Build Utility",
                guiHandler = _ =>
                {
                    var so = new SerializedObject(Settings);
                    so.Update();

                    EditorGUILayout.LabelField("Versioning", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(so.FindProperty(nameof(Settings.InitialVersion)));
                    EditorGUILayout.PropertyField(so.FindProperty(nameof(Settings.ChangelogPathFormat)));

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Changelog Copy", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(so.FindProperty(nameof(Settings.CopyChangelogToBuildFolder)));

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Version Prompt Defaults", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(so.FindProperty(nameof(Settings.LastSelectedTag)));
                    EditorGUILayout.PropertyField(so.FindProperty(nameof(Settings.ShowPromptOnBuildAndRun)));

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Runtime Overlay", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(so.FindProperty(nameof(Settings.EnableOverlay)));
                    EditorGUILayout.PropertyField(so.FindProperty(nameof(Settings.OverlayPosition)));
                    EditorGUILayout.PropertyField(so.FindProperty(nameof(Settings.ShowStudioLabel)));

                    so.ApplyModifiedProperties();
                }
            };
            
            return provider;
        }
    }
}
