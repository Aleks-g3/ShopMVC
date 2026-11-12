using System.ComponentModel.DataAnnotations;

namespace ShopMVC.Models;

public class UpdatableEntity : Entity
{
    [Key] public long Id { get; set; }
    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    [Timestamp] public byte[] RowVersion { get; private set; } = null!;

    public bool IsTransient => Id == default;
}