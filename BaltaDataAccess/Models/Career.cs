using System;
using System.Collections.Generic;

namespace BaltaDataAccess.Models
{
    public class Career
    {

        public Career()
        {
            Items = new List<CareerItem>();// Inicializando a lista para não termos problemas de objeto NULO ao tentar adicionar algo na lista.
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public IList<CareerItem> Items { get; set; } //Lista de CareerItem
    }
}
