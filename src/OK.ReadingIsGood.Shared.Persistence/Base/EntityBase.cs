using System;
using OK.ReadingIsGood.Shared.Persistence.Domain;

namespace OK.ReadingIsGood.Shared.Persistence.Base
{
    public class EntityBase : IIdentifiable<int>, ICreatable, IUpdatable, IDeletable, IAuditable
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}