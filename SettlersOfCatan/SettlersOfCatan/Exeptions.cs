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

    /*
        This exception is thrown when the player is choosing a settlement to steal a resource
        card but does not choose a settlement adjascent to the thief's current location.
     */
    public class SettlementNotNearThiefException : Exception
    {
        public static String MESSAGE = "The settlement you have chosen is not adjascent to the thief.";
        public SettlementNotNearThiefException() : base(MESSAGE)
        {}
    }
    public class NoPlayerAtSettlementException : Exception
    {
        public static String MESSAGE = "This location is not occupied by a player. Please choose one with a player.";
        public NoPlayerAtSettlementException() : base(MESSAGE)
        {}
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
