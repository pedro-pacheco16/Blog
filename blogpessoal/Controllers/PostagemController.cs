﻿using blogpessoal.model;
using blogpessoal.Service;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace blogpessoal.Controllers
{

    [Route("~/postagens")]
    [ApiController]
    public class PostagemController : ControllerBase
    {

        private readonly IPostagemService _postagemService; 
        private readonly IValidator<Postagem> _postagemValidator;

        public PostagemController(IPostagemService postagemService, IValidator <Postagem>postagemValidator)
        {
            _postagemService = postagemService;
            _postagemValidator = postagemValidator;
        }

        [HttpGet]
        public async Task <ActionResult> GetAll()
        {
            return Ok(await _postagemService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task <ActionResult> GetById(long id)
        {
            var Resposta = await _postagemService.GetById(id);

            if (Resposta is null)
                return NotFound();

            return Ok(Resposta);
        }

        [HttpGet("titulo/{titulo}")]
        public async Task<ActionResult> GetByTitulo(string titulo)
        {
            return Ok(await _postagemService.GetByTitulo(titulo));
        }

        [HttpPost]
        public async Task<ActionResult> Create ([FromBody]Postagem postagem)
        {
            var validarPostagem = await _postagemValidator.ValidateAsync(postagem);

            if (!validarPostagem.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarPostagem);

            await _postagemService.Create(postagem);

            return CreatedAtAction(nameof(GetById), new { id = postagem.id }, postagem);
        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Postagem postagem)
        {
            if (postagem.id == 0)
                return BadRequest("Id da Postagem é invalido!");

            var validarPostagem = await _postagemValidator.ValidateAsync(postagem);

            if (!validarPostagem.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validarPostagem);
            }

            var resposta = await _postagemService.Update(postagem);

            if (resposta is null)
                return NotFound("Postagem não encontrada!");

            return Ok();
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var BuscaPostagem = await _postagemService.GetById(id);

            if (BuscaPostagem is null)
                return NotFound("A postagem não foi encontrada!");

            await _postagemService.Delete(BuscaPostagem);

            return NoContent();
        }
    }
}
