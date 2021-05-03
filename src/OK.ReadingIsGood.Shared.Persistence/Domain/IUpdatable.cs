using System;

namespace OK.ReadingIsGood.Shared.Persistence.Domain
{
    public interface IUpdatable
    {
        string UpdatedBy { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
