using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace SourceAFIS.Tests.Executable
{
    [TestFixture]
    [Category("Installer")]
    public class Installer
    {
        string ProgramFiles = @"C:\Program Files\SourceAFIS";

        [Test]
        public void InstallDir()
        {
            Assert.That(Directory.Exists(ProgramFiles));
        }

        [Test]
        public void BinFolder()
        {
            string bin = Path.Combine(ProgramFiles, "Bin");
            Assert.That(Directory.Exists(bin));
            Assert.That(File.Exists(Path.Combine(bin, "SourceAFIS.dll")));
            Assert.That(File.Exists(Path.Combine(bin, "SourceAFIS.Visualization.dll")));
            Assert.That(File.Exists(Path.Combine(bin, "SourceAFIS.FingerprintAnalysis.exe")));
        }

        [Test]
        public void SampleFiles()
        {
            string sample = Path.Combine(ProgramFiles, "Sample");
            Assert.That(Directory.Exists(sample));
            Assert.That(File.Exists(Path.Combine(sample, "Sample.sln")));
            Assert.AreEqual(4, Directory.GetFiles(Path.Combine(sample, "images")).Length);
            Assert.That(File.Exists(Path.Combine(sample, "dll", "SourceAFIS.dll")));
            Assert.That(File.Exists(Path.Combine(sample, "bin", "Debug", "Sample.exe")));
        }

        [Test]
        public void DocFolder()
        {
            string doc = Path.Combine(ProgramFiles, "Documentation");
            Assert.That(Directory.Exists(doc));
            Assert.That(File.Exists(Path.Combine(doc, "SourceAFIS_Home.html")));
            Assert.That(File.Exists(Path.Combine(doc, "SourceAFIS.chm")));
            Assert.That(File.Exists(Path.Combine(doc, "SourceAFIS", "html", "N_SourceAFIS_Simple.htm")));
            Assert.That(File.Exists(Path.Combine(doc, "SourceAFIS", "icons", "pubclass.gif")));
        }

        [Test]
        public void StartMenu()
        {
            string start = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "SourceAFIS");
            Assert.That(Directory.Exists(start));
            Assert.That(File.Exists(Path.Combine(start, "Program Files.lnk")));
            Assert.That(File.Exists(Path.Combine(start, "Project Homepage.lnk")));
            Assert.That(File.Exists(Path.Combine(start, "Fingerprint Analysis.lnk")));
        }

        [Test]
        public void SampleRun()
        {
            string path = Path.Combine(ProgramFiles, "Sample", "bin", "Debug");
            Process process = new Process();
            process.StartInfo.FileName = Path.Combine(path, "Sample.exe");
            process.StartInfo.WorkingDirectory = path;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            Assert.That(output.Contains("300 x 300"));
            Assert.That(output.Contains("Similarity score"));
            Assert.That(output.Contains("matches registered person Wilma"));

            File.Delete(Path.Combine(path, "database.dat"));
        }
    }
}
