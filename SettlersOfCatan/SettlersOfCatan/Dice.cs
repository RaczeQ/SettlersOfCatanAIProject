using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan
{
    public class Dice
    {
        public int diceValue1 = 0;
        public int diceValue2 = 0;
        private Random diceRandomizer;

        public Dice()
        {
            diceRandomizer = new Random();
        }

        /*
            Gives a realistic dice value by generating two random numbers and adding them.
            The values of the individual dice can be accessed later with the above global variables.
         */
        public int roll()
        {
            diceValue1 = diceRandomizer.Next(1, 7);
            diceValue2 = diceRandomizer.Next(1, 7);
            return diceValue1 + diceValue2;
        }
    }
}
