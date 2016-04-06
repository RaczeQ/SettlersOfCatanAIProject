using System;
using System.Windows.Forms;


public interface Event
{
    public Event(Board b);

    public void beginExecute(); //Preparation
    public void onExecute(); //
    public void endExecute();  //Cleanup
}
