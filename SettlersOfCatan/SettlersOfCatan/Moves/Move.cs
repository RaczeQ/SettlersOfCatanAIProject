using SettlersOfCatan.AI;

namespace SettlersOfCatan.Moves
{
    public interface Move
    {
        void MakeMove(ref Board target);
        void MakeMove(ref BoardState target);
        bool CanMakeMove(Board target);
        bool CanMakeMove(BoardState target);
    }
}
