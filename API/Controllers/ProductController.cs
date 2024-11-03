using API.RequestHelper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

public class ProductsController: BaseApiController
{

    private readonly ILogger<ProductsController> _logger;
    private readonly IGenericRepository<Product> repo;

    public ProductsController(ILogger<ProductsController> logger,
        StoreContext context, IGenericRepository<Product> service)
    {
            _logger = logger;
            this.repo = service;
    }


    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
       [FromQuery] ProductSpecParams specParams)
    {
        ProductSpecification spec =new ProductSpecification(specParams) ;

        
        return Ok (await CreatePagedResult(repo,
            spec, specParams.PageIndex, specParams.PageSize));

    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>>GetProduct(int id)
    {

        var product = await repo.GetByIdAsync(id);
    
        return product == null ? NotFound() : Ok(product);

    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);

        if (await repo.SaveAllAsync())
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

        repo.Update(product);
        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

            return BadRequest("Problem update the product");

    }
    [HttpDelete("{id:int}")]

    public async Task<ActionResult>DeleteProduct(int id)
    {

        var product = await repo.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        repo.Remove(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleteing  the product");

    }


    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        return Ok(await repo.ListAsync(spec));
    }
    
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();

        return Ok(await repo.ListAsync(spec));
    }

    private bool ProductExists(int id)
    {
        return repo.Exists(id);
    }


}
