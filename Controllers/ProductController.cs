using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Models;
using PharmaStock.Repositories;
using PharmaStock.Services;

namespace PharmaStock.Controllers
{
    [ApiController]
    [Route("api/producto")]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _repository;
        private readonly AlertService _alertService;

        public ProductController(ProductRepository repository, AlertService alertService)
        {
            _repository = repository;
            _alertService = alertService;
        }
       

        [HttpGet("all")]
        public ActionResult<List<Producto>> GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("productos/{id}")]
        public async Task<Producto> GetById(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        [HttpPost("registro")]
        [Authorize(Roles ="ADMIN")]
        public ActionResult<Producto> Agregar([FromBody]Producto producto)
        {
            var nuevoProducto=_repository.Agregar(producto);
            return CreatedAtAction(nameof(GetById), new {id=nuevoProducto.Id}, nuevoProducto);
            //return _repository.Agregar(producto);
        }

        [HttpPut("update/{id}")]
        public Producto Update([FromBody]Producto producto)
        {
            return _repository.Update(producto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="ADMIN")]
        public async Task Delete(long id)
        {
            var productoEncontrado = await _repository.GetByIdAsync(id);
            _repository.Delete(productoEncontrado);
        }

        
    }
}