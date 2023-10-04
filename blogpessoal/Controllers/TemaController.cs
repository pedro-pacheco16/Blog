using blogpessoal.model;
using blogpessoal.Service;
using blogpessoal.Service.Implements;
using blogpessoal.validator;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogpessoal.Controllers
{
    [Authorize]
    [Route("~/temas")]
    [ApiController]
    public class TemaController : ControllerBase
    {
        private readonly ITemaService _TemaService;
        private readonly IValidator<Tema> _TemaValidator;

        public TemaController(ITemaService TemaService, IValidator<Tema> TemaValidator)
        {
            _TemaService = TemaService;
            _TemaValidator = TemaValidator;
        }


        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _TemaService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var Resposta = await _TemaService.GetById(id);

            if (Resposta is null)
                return NotFound();

            return Ok(Resposta);
        }

        [HttpGet("Descricao/{Descricao}")]
        public async Task<ActionResult> GetByDescricao(string descricao)
        {
            return Ok(await _TemaService.GetByDescricao(descricao));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Tema tema)
        {
            var validarTema = await _TemaValidator.ValidateAsync(tema);

            if (!validarTema.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarTema);

            await _TemaService.Create(tema);

            return CreatedAtAction(nameof(GetById), new { id = tema.Id }, tema);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Tema tema)
        {
            if (tema.Id == 0)
                return BadRequest("Id da Tema é invalido!");

            var validarTema = await _TemaValidator.ValidateAsync(tema);

            if (!validarTema.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validarTema);
            }

            var resposta = await _TemaService.Update(tema);

            if (resposta is null)
                return NotFound("Tema não encontrada!");

            return Ok(resposta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var BuscaTema = await _TemaService.GetById(id);

            if (BuscaTema is null)
                return NotFound("A Tema não foi encontrada!");

            await _TemaService.Delete(BuscaTema);

            return NoContent();
        }

    }
}
