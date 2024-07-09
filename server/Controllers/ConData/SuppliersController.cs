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

  [Route("odata/ConData/Suppliers")]
  public partial class SuppliersController : ODataController
  {
    private Data.ConDataContext context;

    public SuppliersController(Data.ConDataContext context)
    {
      this.context = context;
    }
    // GET /odata/ConData/Suppliers
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet]
    public IEnumerable<Models.ConData.Supplier> GetSuppliers()
    {
       AuthController.AddTokenToHeader(this.HttpContext).Wait();

      var items = this.context.Suppliers.AsQueryable<Models.ConData.Supplier>();
      this.OnSuppliersRead(ref items);

      return items;
    }

    partial void OnSuppliersRead(ref IQueryable<Models.ConData.Supplier> items);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet("/odata/ConData/Suppliers(SupplierID={SupplierID})")]
    public SingleResult<Supplier> GetSupplier(int key)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        var items = this.context.Suppliers.Where(i=>i.SupplierID == key);
        this.OnSuppliersGet(ref items);

        return SingleResult.Create(items);
    }

    partial void OnSuppliersGet(ref IQueryable<Models.ConData.Supplier> items);

    partial void OnSupplierDeleted(Models.ConData.Supplier item);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpDelete("/odata/ConData/Suppliers(SupplierID={SupplierID})")]
    public IActionResult DeleteSupplier(int key)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var items = this.context.Suppliers
                .Where(i => i.SupplierID == key)
                .Include(i => i.Products)
                .AsQueryable();

            items = EntityPatch.ApplyTo<Models.ConData.Supplier>(Request, items);

            var itemToDelete = items.FirstOrDefault();

            if (itemToDelete == null)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed);
            }

            this.OnSupplierDeleted(itemToDelete);
            this.context.Suppliers.Remove(itemToDelete);
            this.context.SaveChanges();

            return new NoContentResult();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnSupplierUpdated(Models.ConData.Supplier item);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpPut("/odata/ConData/Suppliers(SupplierID={SupplierID})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PutSupplier(int key, [FromBody]Models.ConData.Supplier newItem)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var items = this.context.Suppliers
                .Where(i => i.SupplierID == key)
                .Include(i => i.Products)
                .AsQueryable();

            items = EntityPatch.ApplyTo<Models.ConData.Supplier>(Request, items);

            var itemToUpdate = items.FirstOrDefault();

            if (itemToUpdate == null)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed);
            }

            this.OnSupplierUpdated(newItem);
            this.context.Suppliers.Update(newItem);
            this.context.SaveChanges();

            var itemToReturn = this.context.Suppliers.Where(i => i.SupplierID == key);
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpPatch("/odata/ConData/Suppliers(SupplierID={SupplierID})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PatchSupplier(int key, [FromBody]Delta<Models.ConData.Supplier> patch)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var items = this.context.Suppliers.Where(i => i.SupplierID == key);

            items = EntityPatch.ApplyTo<Models.ConData.Supplier>(Request, items);

            var itemToUpdate = items.FirstOrDefault();

            if (itemToUpdate == null)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed);
            }

            patch.Patch(itemToUpdate);

            this.OnSupplierUpdated(itemToUpdate);
            this.context.Suppliers.Update(itemToUpdate);
            this.context.SaveChanges();

            var itemToReturn = this.context.Suppliers.Where(i => i.SupplierID == key);
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnSupplierCreated(Models.ConData.Supplier item);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpPost]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult Post([FromBody] Models.ConData.Supplier item)
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

            this.OnSupplierCreated(item);
            this.context.Suppliers.Add(item);
            this.context.SaveChanges();

            return Created($"odata/ConData/Suppliers/{item.SupplierID}", item);
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }
  }
}
