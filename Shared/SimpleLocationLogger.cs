using System;

namespace PerpetualEngine.Location
{
    public class SimpleLocationLogger
    {
        public static void Log(string message)
        {
            Console.WriteLine("[SimpleLocation: {0}]", message);
        }

    }
}

