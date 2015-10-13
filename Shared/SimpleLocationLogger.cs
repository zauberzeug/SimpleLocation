using System;

namespace PerpetualEngine.Location
{
    public static class SimpleLocationLogger
    {
        public static bool Enabled = true;

        public static void Log(string message)
        {
            if (Enabled)
                Console.WriteLine("[SimpleLocation: {0}]", message);
        }

    }
}

