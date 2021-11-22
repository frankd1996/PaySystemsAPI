using PaySystemsApi.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySystemsAPI.Entities
{
    public abstract class Entity:IEntity
    {
        public int Id { get; set; }
    }
}
