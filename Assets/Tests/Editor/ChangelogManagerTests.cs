using System.IO;
using NUnit.Framework;
using Opodlinok.BuildUtility.Editor;
using UnityEngine;

namespace BuildUtility.Tests
{
    public class ChangelogManagerTests
    {
        private string _tempDir;
        private string _format;
        private string _changelogPath;

        [SetUp]
        public void SetUp()
        {
            var root = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
            _tempDir = Path.Combine(root, "Temp/ChangelogTests");
            Directory.CreateDirectory(_tempDir);
            _format = "Temp/ChangelogTests/{0}-CHANGELOG.md";
            _changelogPath = Path.Combine(_tempDir, Application.productName + "-CHANGELOG.md");
            if (File.Exists(_changelogPath))
                File.Delete(_changelogPath);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, true);
        }

        [Test]
        public void AppendEntry_CreatesFileWithContent()
        {
            ChangelogManager.AppendEntry(_format, "1.0.0", "First entry");
            Assert.That(File.Exists(_changelogPath));
            var lines = File.ReadAllLines(_changelogPath);
            Assert.That(lines[0], Does.StartWith("## [1.0.0] - "));
            Assert.That(lines[1], Is.EqualTo("- First entry"));
        }

        [Test]
        public void AppendEntry_PrependsToExistingFile()
        {
            ChangelogManager.AppendEntry(_format, "1.0.0", "First entry");
            var firstContent = File.ReadAllText(_changelogPath);

            ChangelogManager.AppendEntry(_format, "1.1.0", "Second entry");
            var finalContent = File.ReadAllText(_changelogPath);

            Assert.That(finalContent, Does.StartWith("## [1.1.0] - "));
            Assert.That(finalContent.IndexOf("Second entry"), Is.LessThan(finalContent.IndexOf("First entry")));
            Assert.That(finalContent.Contains(firstContent.Substring(0, firstContent.Length)));
        }

        [Test]
        public void CopyChangelogToBuildFolder_CopiesFile()
        {
            ChangelogManager.AppendEntry(_format, "1.0.0", "First entry");
            var buildDir = Path.Combine(_tempDir, "Build");
            Directory.CreateDirectory(buildDir);

            ChangelogManager.CopyChangelogToBuildFolder(_format, buildDir);

            var dst = Path.Combine(buildDir, Application.productName + "-CHANGELOG.md");
            Assert.That(File.Exists(dst));
        }
    }
}
