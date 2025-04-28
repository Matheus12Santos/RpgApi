using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.Models;
using RpgApi.Utils;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly DataContext _context;
        public UsuariosController(DataContext context)
        {
            _context = context;
        }
        private async Task<bool> UsuarioExistente(string username)
        {
            if (await _context.TB_USUARIOS.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarUsuario(Usuario user)
        {
            try
            {
                if (await UsuarioExistente(user.Username)) throw new System.Exception("Nome de usuário já existe");

                Criptografia.CriaPasswordHash(user.PasswordString, out byte[] hash, out byte[] salt);
                user.PasswordString = string.Empty;
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
                await _context.TB_USUARIOS.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok(user.Id);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        //(3) Na classe UsuariosController.cs, altere o método autenticar para que na linha anterior ao “return Ok, a 
        // propriedade data de acesso do objeto “usuario” seja alimentada com a data/hora atual e 
        // salve as alterações no Banco via EF.  
        [HttpPost("Autenticar")]
        public async Task<IActionResult> AutenticarUsuario(Usuario credenciais)
        {
            try
            {
                Usuario? usuario = await _context.TB_USUARIOS.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(credenciais.Username.ToLower()));
                if (usuario == null)
                {
                    throw new System.Exception("Usuário não encontrado.");
                }
                else if (!Criptografia.VerificarPasswordHash(credenciais.PasswordString, usuario.PasswordHash, usuario.PasswordSalt))
                {
                    throw new System.Exception("Senha incorreta.");
                }
                else
                {
                    usuario.DataAcesso = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return Ok(usuario);
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //(1) Criar um método Put com rota “AlterarSenha” na classe UsuariosController.cs que 
        // criptografe e altere a senha do usuário no banco e faça com que ele consiga autenticar. 
        [HttpPut("AlterarSenha")]
        public async Task<IActionResult> AlterarSenha(Usuario usuario)
        {
            try
            {
                Usuario? usuarioExistente = await _context.TB_USUARIOS.FirstOrDefaultAsync(usu => usu.Id == usuario.Id);

                if(usuarioExistente == null)
                {
                    return NotFound("Usuario não encontrado.");
                }

                // Gerar o novo hash e salt para a nova senha
                byte[] novoHash;
                byte[] novoSalt;
                Criptografia.CriaPasswordHash(usuario.PasswordString, out novoHash, out novoSalt);

                usuarioExistente.PasswordHash = novoHash;
                usuarioExistente.PasswordSalt = novoSalt;

                await _context.SaveChangesAsync();

                return Ok("Senha alterada com sucesso.");
            }
            catch(System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //(2) Criar um método Get para listar todos os Usuarios na classe UsuariosController.cs 
        [HttpGet("GetAll")]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _context.TB_USUARIOS.ToListAsync();
            return Ok(usuarios);
        }
    }
}