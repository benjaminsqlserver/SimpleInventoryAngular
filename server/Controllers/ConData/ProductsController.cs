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

  [Route("odata/ConData/Products")]
  public partial class ProductsController : ODataController
  {
    private Data.ConDataContext context;

    public ProductsController(Data.ConDataContext context)
    {
      this.context = context;
    }
    // GET /odata/ConData/Products
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet]
    public IEnumerable<Models.ConData.Product> GetProducts()
    {
       AuthController.AddTokenToHeader(this.HttpContext).Wait();

      var items = this.context.Products.AsQueryable<Models.ConData.Product>();
      this.OnProductsRead(ref items);

      return items;
    }

    partial void OnProductsRead(ref IQueryable<Models.ConData.Product> items);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet("/odata/ConData/Products(ProductID={ProductID})")]
    public SingleResult<Product> GetProduct(int key)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        var items = this.context.Products.Where(i=>i.ProductID == key);
        this.OnProductsGet(ref items);

        return SingleResult.Create(items);
    }

    partial void OnProductsGet(ref IQueryable<Models.ConData.Product> items);

    partial void OnProductDeleted(Models.ConData.Product item);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpDelete("/odata/ConData/Products(ProductID={ProductID})")]
    public IActionResult DeleteProduct(int key)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var items = this.context.Products
                .Where(i => i.ProductID == key)
                .Include(i => i.InventoryTransactions)
                .AsQueryable();

            items = EntityPatch.ApplyTo<Models.ConData.Product>(Request, items);

            var itemToDelete = items.FirstOrDefault();

            if (itemToDelete == null)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed);
            }

            this.OnProductDeleted(itemToDelete);
            this.context.Products.Remove(itemToDelete);
            this.context.SaveChanges();

            return new NoContentResult();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnProductUpdated(Models.ConData.Product item);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpPut("/odata/ConData/Products(ProductID={ProductID})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PutProduct(int key, [FromBody]Models.ConData.Product newItem)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var items = this.context.Products
                .Where(i => i.ProductID == key)
                .Include(i => i.InventoryTransactions)
                .AsQueryable();

            items = EntityPatch.ApplyTo<Models.ConData.Product>(Request, items);

            var itemToUpdate = items.FirstOrDefault();

            if (itemToUpdate == null)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed);
            }

            this.OnProductUpdated(newItem);
            this.context.Products.Update(newItem);
            this.context.SaveChanges();

            var itemToReturn = this.context.Products.Where(i => i.ProductID == key);
            Request.QueryString = Request.QueryString.Add("$expand", "Supplier,Category");
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpPatch("/odata/ConData/Products(ProductID={ProductID})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PatchProduct(int key, [FromBody]Delta<Models.ConData.Product> patch)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var items = this.context.Products.Where(i => i.ProductID == key);

            items = EntityPatch.ApplyTo<Models.ConData.Product>(Request, items);

            var itemToUpdate = items.FirstOrDefault();

            if (itemToUpdate == null)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed);
            }

            patch.Patch(itemToUpdate);

            this.OnProductUpdated(itemToUpdate);
            this.context.Products.Update(itemToUpdate);
            this.context.SaveChanges();

            var itemToReturn = this.context.Products.Where(i => i.ProductID == key);
            Request.QueryString = Request.QueryString.Add("$expand", "Supplier,Category");
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnProductCreated(Models.ConData.Product item);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpPost]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult Post([FromBody] Models.ConData.Product item)
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

            this.OnProductCreated(item);
            this.context.Products.Add(item);
            this.context.SaveChanges();

            var key = item.ProductID;

            var itemToReturn = this.context.Products.Where(i => i.ProductID == key);

            Request.QueryString = Request.QueryString.Add("$expand", "Supplier,Category");

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
