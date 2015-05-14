using System;
using Cqs.Infrastructure;

namespace Cqs.Domain.Models
{
    public class Todo : IEntity
    {
        // TODO: Is is possible to remove the forcing of having Id in IEntity?
        public int Id { get; set; }

        public string Caption { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? DueUtc { get; set; }
        public string Category { get; set; }

        
    }
}
