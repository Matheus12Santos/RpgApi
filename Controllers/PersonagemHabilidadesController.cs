using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RpgApi.Data;
using RpgApi.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonagemHabilidadesController : ControllerBase
    {
        private readonly DataContext _context;

        public PersonagemHabilidadesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPersonagemHabilidadeAsync(PersonagemHabilidade novoPersonagemHabilidade)
        {
            try
            {
                Personagem personagem = await _context.TB_PERSONAGENS
                    .Include(p => p.Arma)
                    .Include(p => p.PersonagemHabilidades).ThenInclude(ps => ps.Habilidade)
                    .FirstOrDefaultAsync(p => p.Id == novoPersonagemHabilidade.PersonagemId);

                if (personagem == null)
                    throw new System.Exception("Personagem não encontrado para o Id informado.");

                Habilidade habilidade = await _context.TB_HABILIDADES
                    .FirstOrDefaultAsync(h => h.Id == novoPersonagemHabilidade.HabilidadeId);

                if (habilidade == null)
                    throw new System.Exception("Habilidade não encontrada.");

                PersonagemHabilidade ph = new PersonagemHabilidade();
                ph.Personagem = personagem;
                ph.Habilidade = habilidade;
                await _context.TB_PERSONAGENS_HABILIDADES.AddAsync(ph);
                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //(5) Criar um método na classe PersonagemHabilidadesController.cs que retorne uma lista de 
        // PersonagemHabilidade de acordo com o id do personagem passado por parâmetro. 
        // Using de System.Collections.Generic e System.Linq 
        [HttpGet("BuscarId/{id}")]
        public async Task<IActionResult> PersonagemHabilidadeByPersonagemId(int id)
        {
            try {
                List<PersonagemHabilidade> habilidades = await _context.TB_PERSONAGENS_HABILIDADES
                .Include(p => p.Personagem)
                .Include(p => p.Habilidade)                
                .Where(ph => ph.PersonagemId == id).ToListAsync();

                if (habilidades == null || !habilidades.Any())
                {
                    return NotFound("Nenhuma habilidade encontrada para o personagem com o ID fornecido.");
                }

                return Ok(habilidades);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //(6) Criar um método na classe PersonagemHabilidadesController.cs que retorne uma lista de 
        // Habilidades com a rota chamada GetHabilidades. 
        [HttpGet("GetHabilidades")]
        public async Task<IActionResult> GetHabilidades()
        {
            try{
                var habilidades = await _context.TB_PERSONAGENS_HABILIDADES.ToListAsync();
                return Ok(habilidades);
            }
            catch(System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        //(7) Criar um método na controller PersonagemHabilidadesController.cs que remova os dados da tabela PersonagemHabilidades. 
        // Esse método terá que ser do tipo Post (com rota chamada DeletePersonagemHabilidade) pelo fato de ter que receber o objeto 
        // como parâmetro, contendo o id do personagem e da habilidade. Dica: Use o FirstOrDefaultAsync que exige o using System.Linq. 
        [HttpPost("DeletePersonagemHabilidade")]
        public async Task<IActionResult> DeletePersonagemHabilidade(PersonagemHabilidade personagemHabilidade)
        {
            try 
            {
                if (personagemHabilidade == null || personagemHabilidade.PersonagemId == 0 || personagemHabilidade.HabilidadeId == 0)
                {
                    return BadRequest("Parâmetros inválidos.");
                }

                var removerPersonagemHabilidade = await _context.TB_PERSONAGENS_HABILIDADES
                    .FirstOrDefaultAsync(ph => ph.PersonagemId == personagemHabilidade.PersonagemId && 
                    ph.HabilidadeId == personagemHabilidade.HabilidadeId);

                if (removerPersonagemHabilidade == null)
                {
                    return NotFound("Relacionamento não encontrado.");
                }

                _context.TB_PERSONAGENS_HABILIDADES.Remove(removerPersonagemHabilidade);

                await _context.SaveChangesAsync();

                return Ok("Relacionamento removido com sucesso.");
            }            
            catch(System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}