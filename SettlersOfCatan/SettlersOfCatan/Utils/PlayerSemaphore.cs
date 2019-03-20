using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SettlersOfCatan.Utils
{
    public static class PlayerSemaphore
    {
        public static bool isLocked { get; private set; } = false;
        public static void lockGame()
        {
            isLocked = true;
        }

        public static void unlockGame()
        {
            isLocked = false;
        }

        public static async Task waitForEventEnd()
        {
            while (isLocked) await Task.Delay(100);
        }
    }
}
