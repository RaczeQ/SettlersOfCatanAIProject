using SettlersOfCatan.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SettlersOfCatan
{
    public abstract class Event
    {
        public void asyncBeginExecution(Board b, EvtOwnr evtO)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.Sleep(100);
                beginExecution(b, evtO);
            }).Start();
        }
        public abstract void beginExecution(Board b, EvtOwnr evtO);

        public abstract void disableEventObjects();

        public abstract void enableEventObjects();

        public abstract void endExecution();

        public abstract void executeUpdate(Object sender, EventArgs e);
    }
}
