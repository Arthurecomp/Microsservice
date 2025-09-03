using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estoque.API.DTOs;
using Estoque.API.Entities;

namespace Estoque.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProductById(int id);
        Task CreateProduct(Product product); // cadastrar produto
        Task<bool> UpdateQtd(Product product); //atualizar quantidade
        Task<int?> GetEstoqueById(int id); // pegar quantidade em estoque
    }
}

//Microserviço de Gestão de Estoque deve permitir cadastrar produtos, consultar estoque e atualizar quantidades.  1 requisito