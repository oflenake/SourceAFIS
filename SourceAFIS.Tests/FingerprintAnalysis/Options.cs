using NUnit.Framework;

namespace SourceAFIS.Tests.FingerprintAnalysis
{
    [TestFixture, RequiresSTA]
    [Category("UI")]
    public class Options : Common
    {
        [Test]
        public void ClickThrough()
        {
            SelectFiles();
            ResetOptions();
            FullOptions();
        }

        [Test]
        public void NotFullyLoaded()
        {
            CloseFiles();
            FullOptions();
            SelectFile(Left, Settings.SomeFingerprintPath);
            SelectFile(Left, null);
            SelectFile(Right, Settings.MatchingFingerprintPath);
            SelectFile(Left, Settings.SomeFingerprintPath);
            SelectFile(Right, null);
            SelectFile(Left, null);
        }
    }
}
