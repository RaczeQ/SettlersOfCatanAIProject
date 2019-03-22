using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.GameObjects
{
    [Serializable]
    public class ResourceCard
    {
        public Guid guid { get; set; } = Guid.NewGuid();
        private Board.ResourceType resourceType;

        public ResourceCard(Board.ResourceType resource)
        {
            this.resourceType = resource;
        }

        public Board.ResourceType getResourceType()
        {
            return this.resourceType;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType()) return false;

            ResourceCard rc = (ResourceCard)obj;
            return this.guid == rc.guid;
        }
    }
}
