using System;

namespace OK.ReadingIsGood.Shared.Persistence.Domain
{
    public interface IDeletable
    {
        bool IsDeleted { get; set; }
        string DeletedBy { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
