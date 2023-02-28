using AutoMapper;
using MagicVilla.VillaAPI.Data;
using MagicVilla.VillaAPI.Models;
using MagicVilla.VillaAPI.Models.Dto;
using MagicVilla.VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla.VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext dbContext, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _mapper = mapper;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(x => x.UserName == username);

            return (user == null ? true : false);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var usuario = _dbContext.ApplicationUsers.FirstOrDefault(x=> x.UserName == loginRequestDTO.Usuario);
            bool isValid = await _userManager.CheckPasswordAsync(usuario, loginRequestDTO.Password);
            var roles = await _userManager.GetRolesAsync(usuario);

            if(usuario != null && isValid)
            {
                //Generar Token JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);


                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault())

                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
                {
                    Token = tokenHandler.WriteToken(token),
                    User = _mapper.Map<UserDTO>(usuario),
                    Role = roles.FirstOrDefault()
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

        public async Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            ApplicationUser usuario = new ApplicationUser()
            {
                UserName = registerationRequestDTO.Usuario,
                Email = registerationRequestDTO.Usuario,
                NormalizedEmail = registerationRequestDTO.Usuario.ToLower(),
                Nombre = registerationRequestDTO.Nombre,
            };

            try
            {
                var result = await _userManager.CreateAsync(usuario, registerationRequestDTO.Password);
                if(result.Succeeded)
                {
                    if(!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await _userManager.AddToRoleAsync(usuario, "admin");
                    var userToReturn = _dbContext.ApplicationUsers
                        .FirstOrDefault(x => x.UserName == registerationRequestDTO.Usuario);

                    return _mapper.Map<UserDTO>(userToReturn);
                }
            }
            catch(Exception ex)
            {

            }
            return new UserDTO();
        }
    }
}
