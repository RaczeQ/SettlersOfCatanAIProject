﻿using SettlersOfCatan.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan
{
    public interface Event2
    {
        void asyncBeginExecution(Board b, EvtOwnr evtO);
        void beginExecution(Board b, EvtOwnr evtO);
        void executeUpdate(Object sender, EventArgs e);
        void endExecution();

        void disableEventObjects();
        void enableEventObjects();
    }
}
