using OK.ReadingIsGood.Shared.Persistence.Domain;

namespace OK.ReadingIsGood.Shared.Persistence.Entities
{
    public class ChangeEntity : IIdentifiable<int>
    {
        public int Id { get; set; }
        public int ChangesetId { get; set; }
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public virtual ChangesetEntity Changeset { get; set; }
    }
}