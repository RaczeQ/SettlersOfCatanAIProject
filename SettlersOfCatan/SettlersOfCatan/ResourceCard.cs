using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan
{
    [Serializable]
    public class ResourceCard
    {

        private Board.ResourceType resourceType;

        public ResourceCard(Board.ResourceType resource)
        {
            this.resourceType = resource;
        }

        public Board.ResourceType getResourceType()
        {
            return this.resourceType;
        }

    }
}
