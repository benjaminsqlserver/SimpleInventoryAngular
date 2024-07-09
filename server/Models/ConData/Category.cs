using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManager.Models.ConData
{
  [Table("Categories", Schema = "dbo")]
  public partial class Category
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryID
    {
      get;
      set;
    }


    public ICollection<Product> Products { get; set; }
    [ConcurrencyCheck]
    public string CategoryName
    {
      get;
      set;
    }
  }
}
