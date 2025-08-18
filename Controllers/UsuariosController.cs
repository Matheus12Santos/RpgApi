using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RpgApi.Data;
using RpgApi.Models;
using RpgApi.Utils;

namespace RpgApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public UsuariosController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        private async Task<bool> UsuarioExistente(string username)
        {
            if (await _context.TB_USUARIOS.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        private string CriarToken(Usuario usuario)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Role, usuario.Perfil)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_configuration.GetSection("ConfiguracaoToken:Chave").Value));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
        [AllowAnonymous]
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
                    _context.TB_USUARIOS.Update(usuario);
                    await _context.SaveChangesAsync();

                    usuario.PasswordHash = null;
                    usuario.PasswordSalt = null;
                    usuario.Token = CriarToken(usuario);
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
        public async Task<IActionResult> AlterarSenha(Usuario credenciais)
        {
            try
            {
                Usuario? usuarioExistente = await _context.TB_USUARIOS.FirstOrDefaultAsync(x => x.Username.ToLower()
                .Equals(credenciais.Username.ToLower()));

                if(usuarioExistente == null)                
                    throw new System.Exception("Usuario não encontrado.");                

                // Gerar o novo hash e salt para a nova senha
                Criptografia.CriaPasswordHash(credenciais.PasswordString, out byte[] hash, out byte[] salt);

                usuarioExistente.PasswordHash = hash;
                usuarioExistente.PasswordSalt = salt;

                _context.TB_USUARIOS.Update(usuarioExistente);
                int linhasAfetadas = await _context.SaveChangesAsync();                

                return Ok(linhasAfetadas);
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

        [HttpGet("{usuarioId}")] 
        public async Task<IActionResult> GetUsuario(int usuarioId) 
        { 
            try 
            { 
                //List exigirá o using System.Collections.Generic 
                Usuario usuario = await _context.TB_USUARIOS //Busca o usuário no banco através do Id 
                   .FirstOrDefaultAsync(x => x.Id == usuarioId); 
 
                return Ok(usuario); 
            } 
            catch (System.Exception ex) 
            { 
                return BadRequest(ex.Message); 
            } 
        } 
 
        [HttpGet("GetByLogin/{login}")] 
        public async Task<IActionResult> GetUsuario(string login) 
        { 
            try 
            { 
                //List exigirá o using System.Collections.Generic 
                Usuario usuario = await _context.TB_USUARIOS //Busca o usuário no banco através do login 
                   .FirstOrDefaultAsync(x => x.Username.ToLower() == login.ToLower()); 
 
                return Ok(usuario); 
            } 
            catch (System.Exception ex) 
            { 
                return BadRequest(ex.Message); 
            } 
        } 

        //Método para alteração da geolocalização 
        [HttpPut("AtualizarLocalizacao")] 
        public async Task<IActionResult> AtualizarLocalizacao(Usuario u) 
        { 
            try 
            { 
                Usuario usuario = await _context.TB_USUARIOS //Busca o usuário no banco através do Id 
                   .FirstOrDefaultAsync(x => x.Id == u.Id); 
 
                usuario.Latitude = u.Latitude; 
                usuario.Longitude = u.Longitude; 
 
                var attach = _context.Attach(usuario); 
                attach.Property(x => x.Id).IsModified = false; 
                attach.Property(x => x.Latitude).IsModified = true; 
                attach.Property(x => x.Longitude).IsModified = true; 
 
                int linhasAfetadas = await _context.SaveChangesAsync(); //Confirma a alteração no banco 
                return Ok(linhasAfetadas); //Retorna as linhas afetadas (Geralmente sempre 1 linha msm) 
            } 
            catch (System.Exception ex) 
            { 
                return BadRequest(ex.Message); 
            } 
        } 

        [HttpPut("AtualizarEmail")] 
        public async Task<IActionResult> AtualizarEmail(Usuario u) 
        { 
            try 
            { 
                Usuario usuario = await _context.TB_USUARIOS //Busca o usuário no banco através do Id 
                   .FirstOrDefaultAsync(x => x.Id == u.Id); 
 
                usuario.Email = u.Email;                 
 
                var attach = _context.Attach(usuario); 
                attach.Property(x => x.Id).IsModified = false; 
                attach.Property(x => x.Email).IsModified = true;                 
 
                int linhasAfetadas = await _context.SaveChangesAsync(); //Confirma a alteração no banco 
                return Ok(linhasAfetadas); //Retorna as linhas afetadas (Geralmente sempre 1 linha msm) 
            } 
            catch (System.Exception ex) 
            { 
                return BadRequest(ex.Message); 
            } 
        } 

        //Método para alteração da foto 
        [HttpPut("AtualizarFoto")] 
        public async Task<IActionResult> AtualizarFoto(Usuario u) 
        { 
            try 
            { 
                Usuario usuario = await _context.TB_USUARIOS  
                   .FirstOrDefaultAsync(x => x.Id == u.Id); 
 
                usuario.Foto = u.Foto;                 
 
                var attach = _context.Attach(usuario); 
                attach.Property(x => x.Id).IsModified = false; 
                attach.Property(x => x.Foto).IsModified = true;                 
 
                int linhasAfetadas = await _context.SaveChangesAsync();  
                return Ok(linhasAfetadas);  
            } 
            catch (System.Exception ex) 
            { 
                return BadRequest(ex.Message); 
            } 
        } 
    }
}