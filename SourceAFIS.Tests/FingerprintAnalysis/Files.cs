using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using NUnit.Framework;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace SourceAFIS.Tests.FingerprintAnalysis
{
    [TestFixture, RequiresSTA]
    [Category("UI")]
    public class Files : Common
    {
        [Test]
        public void ClickThrough()
        {
            ResetOptions();
            Orientation.Checked = true;

            CloseFiles();
            SelectFiles();

            SaveFile(Left);
            SaveFile(Right);

            CloseFiles();
        }

        [Test]
        public void OpenCancel()
        {
            CloseFiles();

            Left.Open.Click();
            OpenDialog.Dialog.PasteText(Settings.SomeFingerprintPath);
            OpenDialog.Cancel.Click();

            Assert.AreEqual("", Left.FileName.Text);
        }

        [Test]
        public void SaveCancel()
        {
            ResetOptions();
            Orientation.Checked = true;

            SelectFiles();

            string path = Path.GetFullPath("saved.png");
            File.Delete(path);

            Left.Save.Click();
            SaveDialog.Dialog.PasteText(path);
            SaveDialog.Cancel.Click();

            Assert.IsFalse(File.Exists(path));
        }

        [Test]
        public void OpenInvalid()
        {
            CloseFiles();

            Left.Open.Click();
            FileOpenDialog dialog = OpenDialog;

            dialog.Dialog.PasteText(Path.GetFullPath("nonexistent.tif"));
            Assert.AreEqual(0, dialog.Dialog.ModalWindows().Count);

            dialog.Open.Click();
            Wait(() => Win.ModalWindows().Count == 2);
            Assert.AreEqual(1, dialog.Dialog.ModalWindows().Count);

            dialog.Dialog.ModalWindows()[0].Get<Button>(SearchCriteria.ByText("OK")).Click();
            Assert.AreEqual(0, dialog.Dialog.ModalWindows().Count);

            dialog.Cancel.Click();
            Assert.AreEqual("", Left.FileName.Text);
        }

        [Test]
        public void SaveExisting()
        {
            SelectFiles();
            GenerateSavedFileName();
            File.WriteAllBytes(Settings.LastSavedImage, new byte[0]);

            Left.Save.Click();
            FileSaveDialog dialog = SaveDialog;

            dialog.Dialog.PasteText(Settings.LastSavedImage);
            Assert.AreEqual(0, dialog.Dialog.ModalWindows().Count);

            dialog.Save.Click();
            Wait(() => Win.ModalWindows().Count == 2);
            Assert.AreEqual(1, dialog.Dialog.ModalWindows().Count);

            dialog.Dialog.ModalWindows()[0].Get<Button>(SearchCriteria.ByAutomationId("7")).Click();
            Assert.AreEqual(0, dialog.Dialog.ModalWindows().Count);

            dialog.Cancel.Click();
            Assert.AreEqual(0, File.ReadAllBytes(Settings.LastSavedImage).Length);
        }

        [Test]
        public void FileNameLabel()
        {
            SelectFiles();
            Assert.AreEqual(Path.GetFileNameWithoutExtension(Settings.SomeFingerprintPath), Left.FileName.Text);
            Assert.AreEqual(Path.GetFileNameWithoutExtension(Settings.MatchingFingerprintPath), Right.FileName.Text);
        }
    }
}
