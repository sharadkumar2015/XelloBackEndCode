using System;
using GraduationTracker.Interfaces;

namespace GraduationTracker.Helper
{
    class FileLogger : ILogger
    {
        public void LogError(string error)
        {
            //Not implementing
            //System.IO.File.WriteAllText(@"c:\Error.txt", error);
        }
    }
}
