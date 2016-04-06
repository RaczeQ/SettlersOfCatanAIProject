using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan
{
    public interface Event
    {

        void beginExecution(Board b);
        void executeUpdate(Object sender, EventArgs e);
        void endExecution();

    }
}
