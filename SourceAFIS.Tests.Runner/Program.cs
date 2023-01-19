// Uses SourceAFIS.Tests Library. 

namespace SourceAFIS.Tests.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            NUnit.ConsoleRunner.Runner.Main(new string[]
            {
                typeof(Executable.Installer).Assembly.Location
            });
        }
    }
}
