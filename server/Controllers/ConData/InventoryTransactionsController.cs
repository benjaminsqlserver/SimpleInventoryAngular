using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;



namespace InventoryManager.Controllers.ConData
{
  using Models;
  using Data;
  using Models.ConData;

  [Route("odata/ConData/InventoryTransactions")]
  public partial class InventoryTransactionsController : ODataController
  {
    private Data.ConDataContext context;

    public InventoryTransactionsController(Data.ConDataContext context)
    {
      this.context = context;
    }
    // GET /odata/ConData/InventoryTransactions
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet]
    public IEnumerable<Models.ConData.InventoryTransaction> GetInventoryTransactions()
    {
       AuthController.AddTokenToHeader(this.HttpContext).Wait();

      var items = this.context.InventoryTransactions.AsQueryable<Models.ConData.InventoryTransaction>();
      this.OnInventoryTransactionsRead(ref items);

      return items;
    }

    partial void OnInventoryTransactionsRead(ref IQueryable<Models.ConData.InventoryTransaction> items);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet("/odata/ConData/InventoryTransactions(TransactionID={TransactionID})")]
    public SingleResult<InventoryTransaction> GetInventoryTransaction(int key)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        var items = this.context.InventoryTransactions.Where(i=>i.TransactionID == key);
        this.OnInventoryTransactionsGet(ref items);

        return SingleResult.Create(items);
    }

    partial void OnInventoryTransactionsGet(ref IQueryable<Models.ConData.InventoryTransaction> items);

    partial void OnInventoryTransactionDeleted(Models.ConData.InventoryTransaction item);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpDelete("/odata/ConData/InventoryTransactions(TransactionID={TransactionID})")]
    public IActionResult DeleteInventoryTransaction(int key)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var items = this.context.InventoryTransactions
                .Where(i => i.TransactionID == key)
                .AsQueryable();

            items = EntityPatch.ApplyTo<Models.ConData.InventoryTransaction>(Request, items);

            var itemToDelete = items.FirstOrDefault();

            if (itemToDelete == null)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed);
            }

            this.OnInventoryTransactionDeleted(itemToDelete);
            this.context.InventoryTransactions.Remove(itemToDelete);
            this.context.SaveChanges();

            return new NoContentResult();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnInventoryTransactionUpdated(Models.ConData.InventoryTransaction item);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpPut("/odata/ConData/InventoryTransactions(TransactionID={TransactionID})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PutInventoryTransaction(int key, [FromBody]Models.ConData.InventoryTransaction newItem)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var items = this.context.InventoryTransactions
                .Where(i => i.TransactionID == key)
                .AsQueryable();

            items = EntityPatch.ApplyTo<Models.ConData.InventoryTransaction>(Request, items);

            var itemToUpdate = items.FirstOrDefault();

            if (itemToUpdate == null)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed);
            }

            this.OnInventoryTransactionUpdated(newItem);
            this.context.InventoryTransactions.Update(newItem);
            this.context.SaveChanges();

            var itemToReturn = this.context.InventoryTransactions.Where(i => i.TransactionID == key);
            Request.QueryString = Request.QueryString.Add("$expand", "Product");
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpPatch("/odata/ConData/InventoryTransactions(TransactionID={TransactionID})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PatchInventoryTransaction(int key, [FromBody]Delta<Models.ConData.InventoryTransaction> patch)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var items = this.context.InventoryTransactions.Where(i => i.TransactionID == key);

            items = EntityPatch.ApplyTo<Models.ConData.InventoryTransaction>(Request, items);

            var itemToUpdate = items.FirstOrDefault();

            if (itemToUpdate == null)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed);
            }

            patch.Patch(itemToUpdate);

            this.OnInventoryTransactionUpdated(itemToUpdate);
            this.context.InventoryTransactions.Update(itemToUpdate);
            this.context.SaveChanges();

            var itemToReturn = this.context.InventoryTransactions.Where(i => i.TransactionID == key);
            Request.QueryString = Request.QueryString.Add("$expand", "Product");
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnInventoryTransactionCreated(Models.ConData.InventoryTransaction item);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpPost]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult Post([FromBody] Models.ConData.InventoryTransaction item)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item == null)
            {
                return BadRequest();
            }

            this.OnInventoryTransactionCreated(item);
            this.context.InventoryTransactions.Add(item);
            this.context.SaveChanges();

            var key = item.TransactionID;

            var itemToReturn = this.context.InventoryTransactions.Where(i => i.TransactionID == key);

            Request.QueryString = Request.QueryString.Add("$expand", "Product");

            return new ObjectResult(SingleResult.Create(itemToReturn))
            {
                StatusCode = 201
            };
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }
  }
}
