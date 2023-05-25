using System.ComponentModel.DataAnnotations;

namespace ShopMVC.Models;

public class UpdatableEntity : Entity
{
    [Key] public long Id { get; set; }
    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    public bool IsTransient => Id == default;
}