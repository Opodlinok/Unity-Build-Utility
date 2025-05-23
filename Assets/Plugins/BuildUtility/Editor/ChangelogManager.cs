using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Opodlinok.BuildUtility.Editor
{
    static class ChangelogManager
    {
        public static void AppendEntry(string format, string version, string entry)
        {
            var root = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
            var rel = string.Format(format, Application.productName);
            var abs = Path.Combine(root, rel);
            Directory.CreateDirectory(Path.GetDirectoryName(abs));

            var sb = new StringBuilder();
            sb.AppendLine($"## [{version}] - {DateTime.Now:yyyy-MM-dd}");
            foreach (var line in entry.Split(
                new[]{'\r','\n'}, StringSplitOptions.RemoveEmptyEntries))
                sb.AppendLine($"- {line}");
            sb.AppendLine();

            var old = File.Exists(abs) ? File.ReadAllText(abs) : "";
            File.WriteAllText(abs, sb + old);
        }

        public static void CopyChangelogToBuildFolder(string format, string buildDir)
        {
            var root = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
            var rel = string.Format(format, Application.productName);
            var src = Path.Combine(root, rel);
            if (!File.Exists(src)) return;
            var dst = Path.Combine(buildDir, Path.GetFileName(src));
            File.Copy(src, dst, true);
        }
    }
}
