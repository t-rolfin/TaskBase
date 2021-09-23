using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public class CreateTaskModel
    {
        public string Title { get; set; }
        public string Description { get; set; }


        public Guid RowId { get; set; }
    }
}
