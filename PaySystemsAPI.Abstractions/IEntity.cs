using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySystemsApi.Abstractions
{
    //Con esto podemos obligar a que a la capa de aplicación sólo entren objetos de dominio
    //(modelos) con un Id. Esta interfaz se hereda al objeto Entity en capa Entities, a su vez
    //Entity se hereda al objeto de dominio o modelo, y este modelo el que se usa como clase genérica
    //en la capa de Aplicacion, repositorio y DataAcces (todo clase que abstracta que implemente CRUDs genéricos).
    //Luego en estas clases (Aplicacion, repositorio y DataAcces) se agrega la restricción where T : IEntity
    //Para asegurarnos que entran solo modelos de tipo IEntity
    public interface IEntity
    {
        int Id { get; set; }
    }
}
