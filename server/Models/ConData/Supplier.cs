using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManager.Models.ConData
{
  [Table("Suppliers", Schema = "dbo")]
  public partial class Supplier
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SupplierID
    {
      get;
      set;
    }


    public ICollection<Product> Products { get; set; }
    [ConcurrencyCheck]
    public string SupplierName
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string ContactName
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string Address
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string City
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string PostalCode
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string Country
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string Phone
    {
      get;
      set;
    }
  }
}
