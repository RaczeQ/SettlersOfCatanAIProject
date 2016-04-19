using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan
{
    class Exeptions
    {
    }


    public class ThiefException : Exception
    {
        public static String THIEF_CANNOT_GO_DESERT = "The thief cannot go back to the desert.";
        public static String THIEF_CANNOT_STAY = "The thief may not remain in the same tile. You must select a new location.";
        public static String CANT_STEAL_FROM_PLAYER = "The settlement you have chosen is not adjascent to the thief.";
        public static String NO_PLAYER = "This location is not occupied by a player. Please choose one with a player.";
        public static String YOU_OWN_THIS_SETTLEMENT = "You can't steal from yourself. Please choose another player's settlement.";
        public ThiefException(String message) : base(message) { }
    }

    public class SamePlayerException : Exception
    {
        public static String MESSAGE = "You own this settlement, please select a settlement owned by another player.";
        public SamePlayerException() : base(MESSAGE)
        { }
    }

    public class TradeError : Exception
    {

        public static string NO_HARBOR_RESOURCE_SELECTED = "You must choose a resource from the harbor first.";
        public TradeError(String message) : base(message) { }
    }

    public class BankError : Exception
    {
        public static string BankDoesNotHaveEnoughResourceCard()
        {
            return "The bank does not have enough of that type of resource to give.";
        }

        public BankError(String message) : base(message) { }
    }

    public class BuildError : Exception
    {
        public static String LocationOwnedBy(Player owner)
        {
            return "This location is already owned by " + owner.getPlayerName();
        }

        public static string NOT_ENOUGH_RESOURCES = "You do not have enough resources.";
        public static string SETTLEMENT_TOO_CLOSE = "This location is too close to another player's settlement.";
        public static string IS_CITY = "You cannot upgrade this location because it is already a city.";
        public static string NO_CONNECTED_ROAD = "You cannot build here without a connecting city or other road.";
        public static string NOT_ENOUGH_DEV_CARDS = "There are no more development cards in the deck.";
        public BuildError(String message) : base(message)
        {}
    }

}
