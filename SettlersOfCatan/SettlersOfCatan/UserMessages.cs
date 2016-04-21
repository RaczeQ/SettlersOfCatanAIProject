using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan
{
    //*TEST*
    class UserMessages
    {

        public static string THERE_WAS_A_TIE = "There was a tie between: ";
        public static string ROAD_BUILDING_INSTRUCTIONS = "You may pick two locations to build roads, following the standard building rules.";

        public static string PlayerGotResourceFromPlayer(Player toPlayer, Player fromPlayer, Board.ResourceType res)
        {
            return toPlayer.getName() + " got " + Board.RESOURCE_NAMES[(int)res] + " from " + fromPlayer.getName() + ".";
        }

        public static string PlayerGoNoResourceFromPlayer(Player toPlayer, Player fromPlayer)
        {
            return toPlayer.getName() + " got no resource cards from " + fromPlayer.getName() + " because they have none to give.";
        }

        public static string PlayerEndedTurn(Player currentPlayer)
        {
            return currentPlayer.getName() + " has ended their turn.";
        }

        public static string PlayerRolledANumber(Player player, int number)
        {
            return player.getName() + " rolled " + (number == 8 || number == 11 ? "an " : "a ") + number + ".";
        }

        public static string PlayerDiceRollPrompt(Player player)
        {
            return player.getName() + " please roll the dice.";
        }

        public static string PlayerWinsDiceRoll(Player player)
        {
            return "Congrats player " + player.getName() + " you won the roll and are the first player!";
        }

        public static string PlayerPlacedASettlement(Player player)
        {
            return player.getName() + " placed a settlement.";
        }

        public static string PlayerBuiltACity(Player player)
        {
            return player.getName() + " built a city.";
        }

        public static string PlayerPlacedARoad(Player player)
        {
            return player.getName() + " placed a road.";
        }

        public static string PlayerPurchasedADevCard(Player player)
        {
            return player.getName() + " purchased a development card.";
        }

        public static string PlayerUsedRoadBuilding(Player player)
        {
            return player.getName() + " used a Road Building card.";
        }

        public static string PlayerDoesNotHaveAdjascentSettlement = "You do not have an adjascent settlement to trade with this harbor.";

        public static string RobberHasMoved(Player player)
        {
            return player.getName() + " has moved the robber.";
        }

        public static string PlayerMoveThief(Player player)
        {
            return player.getName() + " please select a terrain tile to move the thief into.";
        }

        public static string PLAYER_STEAL_RESOURCES = "Please select the settlement of the player you wish to steal from.";
    }
}
