using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManager.Models.ConData
{
  [Table("Products", Schema = "dbo")]
  public partial class Product
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductID
    {
      get;
      set;
    }


    public ICollection<InventoryTransaction> InventoryTransactions { get; set; }
    [ConcurrencyCheck]
    public string ProductName
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public int SupplierID
    {
      get;
      set;
    }

    public Supplier Supplier { get; set; }
    [ConcurrencyCheck]
    public int CategoryID
    {
      get;
      set;
    }

    public Category Category { get; set; }
    [ConcurrencyCheck]
    public string QuantityPerUnit
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public decimal? UnitPrice
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public int? UnitsInStock
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public int? UnitsOnOrder
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public int? ReorderLevel
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public bool? Discontinued
    {
      get;
      set;
    }
  }
}
