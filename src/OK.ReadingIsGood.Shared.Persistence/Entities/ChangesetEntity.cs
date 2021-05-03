using System;
using System.Collections.Generic;
using OK.ReadingIsGood.Shared.Persistence.Domain;

namespace OK.ReadingIsGood.Shared.Persistence.Entities
{
    public class ChangesetEntity : IIdentifiable<int>
    {
        public int Id { get; set; }
        public string Operation { get; set; }
        public string TableName { get; set; }
        public string EntityId { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        
        public virtual ICollection<ChangeEntity> Changes { get; set; }
    }
}