using API.RequestHelper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

public  class ProductsController(IUnitOfWork unit ,
    ILogger<ProductsController> _logger) : BaseApiController
{

   


    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
       [FromQuery] ProductSpecParams specParams)
    {
        ProductSpecification spec =new ProductSpecification(specParams) ;

       var result =await CreatePagedResult(unit.Repository<Product>(),
            spec, specParams.PageIndex, specParams.PageSize);

        return result ;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>>GetProduct(int id)
    {

        var product = await unit.Repository<Product>().GetByIdAsync(id);
    
        return product == null ? NotFound() : Ok(product);

    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        unit.Repository<Product>().Add(product);

        if (await unit.Complete())
        {
            return CreatedAtAction(nameof(CreateProduct),new {id=product.Id}, product);
        }

        return BadRequest("problem in creating new product"); 
    }
    [HttpPut("{id:int}")]

    public async Task<ActionResult>UpdateProduct(int id,Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        unit.Repository<Product>().Update(product);
        if (await unit.Complete())
        {
            return NoContent();
        }

            return BadRequest("Problem update the product");

    }
    [HttpDelete("{id:int}")]

    public async Task<ActionResult>DeleteProduct(int id)
    {

        var product = await unit.Repository<Product>().GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        unit.Repository<Product>().Remove(product);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleteing  the product");

    }


    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        return Ok(await unit.Repository<Product>().ListAsync(spec));
    }
    
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();

        return Ok(await unit.Repository<Product>().ListAsync(spec));
    }

    private bool ProductExists(int id)
    {
        return unit.Repository<Product>().Exists(id);
    }

}
