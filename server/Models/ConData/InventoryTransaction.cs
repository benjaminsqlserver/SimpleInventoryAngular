using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManager.Models.ConData
{
  [Table("InventoryTransactions", Schema = "dbo")]
  public partial class InventoryTransaction
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionID
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public int ProductID
    {
      get;
      set;
    }

    public Product Product { get; set; }
    [ConcurrencyCheck]
    public int Quantity
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public DateTime? TransactionDate
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string TransactionType
    {
      get;
      set;
    }
  }
}
