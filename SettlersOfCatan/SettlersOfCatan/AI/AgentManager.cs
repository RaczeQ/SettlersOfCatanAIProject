using SettlersOfCatan.AI.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.AI
{
    public static class AgentManager
    {
        private static IDictionary<string, IAgent> _agents = new Dictionary<string, IAgent>
        {
            { "Player", null },
            { "Random", new RandomAgent() },
            { "Aggressive", new AggressiveAgent() },
            { "BoostedAggressive", new BoostedAggressiveAgent() },
            { "Controlling", new ControllingAgent() },
            { "MCTS", new MctsAgent() }
        };
        
        public static IEnumerable<string> availableAgents {
            get
            {
                return _agents.Keys;
            }
        }

        public static IAgent getAgent(string s)
        {
            if (!_agents.ContainsKey(s)) throw new ArgumentException($"Agent {s} does not exist!");
            return _agents[s];
        }
    }
}
