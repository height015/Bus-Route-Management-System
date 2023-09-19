namespace Backend.Domain.Common;

public partial interface ISoftDeletedEntity
{
    bool Deleted { get; set; }
}
