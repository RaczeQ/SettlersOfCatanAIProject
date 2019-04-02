using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettlersOfCatan.AI;
using SettlersOfCatan.GameObjects;
using SettlersOfCatan.Utils;

namespace SettlersOfCatan.Moves
{
    public class BuildRoadMove : Move
    {
        private readonly Road road;

        public BuildRoadMove(Road r)
        {
            road = r;
        }

        public void MakeMove(ref Board target)
        {
            if (CanMakeMove(target))
            {
                var originalRoad = target.roadLocations.Find(r => Equals(r, road));
                originalRoad.buildRoad(target.currentPlayer, true);
                Board.TheBank.takePayment(target.currentPlayer, Bank.ROAD_COST);
                target.addEventText(UserMessages.PlayerPlacedARoad(target.currentPlayer));
            }
            else
            {
                throw new BuildError("Cannot build road!");
            }
        }

        public void MakeMove(ref BoardState target)
        {
            if (CanMakeMove(target))
            {
                var originalRoad = target.Roads.ToList().Find(r => Equals(r, road));
                originalRoad.buildRoad(target.player, true);
                target.bank.takePayment(target.player, Bank.ROAD_COST);
            }
            else
            {
                throw new BuildError("Cannot build road!");
            }
        }

        private bool _canMakeMove(Road r, Player p, Bank b)
        {
            return r.getOwningPlayer() == null && r.checkForConnection(p) && Bank.hasPayment(p, Bank.ROAD_COST);
        }

        public bool CanMakeMove(Board target)
        {
            var originalRoad = target.roadLocations.Find(r => Equals(r, road));
            return _canMakeMove(originalRoad, target.currentPlayer, Board.TheBank);
        }

        public bool CanMakeMove(BoardState target)
        {
            var originalRoad = target.Roads.ToList().Find(r => Equals(r, road));
            return _canMakeMove(originalRoad, target.player, target.bank);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType()) return false;

            BuildRoadMove move = (BuildRoadMove)obj;
            return this.road == move.road;
        }
    }
}