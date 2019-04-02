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
        public static bool IsLocked { get; private set; } = false;
        public static void lockGame()
        {
            IsLocked = true;
        }

        public static void unlockGame()
        {
            IsLocked = false;
        }

        public static async Task waitForEventEnd()
        {
            while (IsLocked) await Task.Delay(100);
        }
    }
}
