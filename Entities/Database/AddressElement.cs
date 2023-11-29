using System.ComponentModel.DataAnnotations;
using Blog.API.Common.Enums;

namespace Blog.API.Entities.Database;

public class AddressElement
{
    [Key]
    public long ObjectId { get; set; }
    public Guid ObjectGuid { get; set; }
    public long ParentObjId { get; set; }
    public string Text { get; set; }
    public string NormalizedText { get; set; }
    public GarAddressLevel ObjectLevel { get; set; }
}   