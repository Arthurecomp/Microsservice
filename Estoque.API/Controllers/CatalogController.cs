using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estoque.API.DTOs;
using Estoque.API.Entities;
using Estoque.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        public CatalogController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _repository.GetProductById(id);
            if (product is null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto productDto)
        {
            if (productDto is null)
            {
                return BadRequest("Produto inválido");
            }
            var newProduct = new Product
            {
                Nome = productDto.Nome,
                Descricao = productDto.Descricao,
                Preco = productDto.Preco,
                QuantidadeEmEstoque = productDto.QuantidadeEmEstoque
            };
            await _repository.CreateProduct(newProduct);
            return StatusCode(StatusCodes.Status201Created, newProduct);

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] UpdateProductDto productDto)
        {
            var produtoExistente = await _repository.GetProductById(id);
            if (produtoExistente == null)
            {
                return NotFound($"Produto com ID {id} não encontrado.");
            }
            produtoExistente.QuantidadeEmEstoque = productDto.QuantidadeEmEstoque;
            await _repository.UpdateQtd(produtoExistente);
            return NoContent();
        }

    }
}