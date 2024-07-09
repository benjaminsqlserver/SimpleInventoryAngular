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

  [Route("odata/ConData/Categories")]
  public partial class CategoriesController : ODataController
  {
    private Data.ConDataContext context;

    public CategoriesController(Data.ConDataContext context)
    {
      this.context = context;
    }
    // GET /odata/ConData/Categories
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet]
    public IEnumerable<Models.ConData.Category> GetCategories()
    {
       AuthController.AddTokenToHeader(this.HttpContext).Wait();

      var items = this.context.Categories.AsQueryable<Models.ConData.Category>();
      this.OnCategoriesRead(ref items);

      return items;
    }

    partial void OnCategoriesRead(ref IQueryable<Models.ConData.Category> items);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet("/odata/ConData/Categories(CategoryID={CategoryID})")]
    public SingleResult<Category> GetCategory(int key)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        var items = this.context.Categories.Where(i=>i.CategoryID == key);
        this.OnCategoriesGet(ref items);

        return SingleResult.Create(items);
    }

    partial void OnCategoriesGet(ref IQueryable<Models.ConData.Category> items);

    partial void OnCategoryDeleted(Models.ConData.Category item);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpDelete("/odata/ConData/Categories(CategoryID={CategoryID})")]
    public IActionResult DeleteCategory(int key)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var items = this.context.Categories
                .Where(i => i.CategoryID == key)
                .Include(i => i.Products)
                .AsQueryable();

            items = EntityPatch.ApplyTo<Models.ConData.Category>(Request, items);

            var itemToDelete = items.FirstOrDefault();

            if (itemToDelete == null)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed);
            }

            this.OnCategoryDeleted(itemToDelete);
            this.context.Categories.Remove(itemToDelete);
            this.context.SaveChanges();

            return new NoContentResult();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnCategoryUpdated(Models.ConData.Category item);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpPut("/odata/ConData/Categories(CategoryID={CategoryID})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PutCategory(int key, [FromBody]Models.ConData.Category newItem)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var items = this.context.Categories
                .Where(i => i.CategoryID == key)
                .Include(i => i.Products)
                .AsQueryable();

            items = EntityPatch.ApplyTo<Models.ConData.Category>(Request, items);

            var itemToUpdate = items.FirstOrDefault();

            if (itemToUpdate == null)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed);
            }

            this.OnCategoryUpdated(newItem);
            this.context.Categories.Update(newItem);
            this.context.SaveChanges();

            var itemToReturn = this.context.Categories.Where(i => i.CategoryID == key);
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpPatch("/odata/ConData/Categories(CategoryID={CategoryID})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PatchCategory(int key, [FromBody]Delta<Models.ConData.Category> patch)
    {
        AuthController.AddTokenToHeader(this.HttpContext).Wait();

        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var items = this.context.Categories.Where(i => i.CategoryID == key);

            items = EntityPatch.ApplyTo<Models.ConData.Category>(Request, items);

            var itemToUpdate = items.FirstOrDefault();

            if (itemToUpdate == null)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed);
            }

            patch.Patch(itemToUpdate);

            this.OnCategoryUpdated(itemToUpdate);
            this.context.Categories.Update(itemToUpdate);
            this.context.SaveChanges();

            var itemToReturn = this.context.Categories.Where(i => i.CategoryID == key);
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnCategoryCreated(Models.ConData.Category item);

    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes="Bearer")]
    [HttpPost]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult Post([FromBody] Models.ConData.Category item)
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

            this.OnCategoryCreated(item);
            this.context.Categories.Add(item);
            this.context.SaveChanges();

            return Created($"odata/ConData/Categories/{item.CategoryID}", item);
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }
  }
}
