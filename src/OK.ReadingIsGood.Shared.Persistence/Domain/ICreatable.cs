using System;

namespace OK.ReadingIsGood.Shared.Persistence.Domain
{
    public interface ICreatable
    {
        string CreatedBy { get; set; }
        DateTime? CreatedAt { get; set; }
    }
}