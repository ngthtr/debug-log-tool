
using System.Diagnostics;

namespace WindowsFormsApp1.Data
{
    public static class Logger
    {
        public static bool IsDebugEnabled = true;
        public static void logD(string tag, string message)
        {
            if (IsDebugEnabled)
            {
                Debug.WriteLine(tag + ": " + message);
            }
        }
    }

}
