using MagicVilla.VillaAPI.Data;
using MagicVilla.VillaAPI.Models;
using MagicVilla.VillaAPI.Models.Dto;
using MagicVilla.VillaAPI.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla.VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private string secretKey;

        public UserRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _dbContext.LocalUser.FirstOrDefault(x => x.Usuario == username);

            return (user == null ? true : false);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var usuario = _dbContext.LocalUser.FirstOrDefault(x=> x.Usuario == loginRequestDTO.Usuario
            && x.Password == loginRequestDTO.Password);

            if(usuario != null)
            {
                //Generar Token JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);


                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Role, usuario.Rol)

                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
                {
                    Token = tokenHandler.WriteToken(token),
                    User = usuario
                };

                return loginResponseDTO;
            }
            else
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }
        }

        public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            LocalUser usuario = new LocalUser()
            {
                Usuario = registerationRequestDTO.Usuario,
                Password = registerationRequestDTO.Password,
                Nombre = registerationRequestDTO.Nombre,
                Rol = registerationRequestDTO.Rol

            };

            _dbContext.LocalUser.Add(usuario);
            await _dbContext.SaveChangesAsync();
            usuario.Password = "";
            return usuario;
        }
    }
}
