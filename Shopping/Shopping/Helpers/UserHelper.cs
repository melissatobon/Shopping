using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Entities;
using Shopping.Models;

namespace Shopping.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserHelper(DataContext context, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password); //Para crear el usuario, la clase usermanager tiene un metodo de crear y le mandamos el user y contraseña
        }

        public async Task<User> AddUserAsync(AddUserViewModel model)
        {
            User user = new User
            {
                Address = model.Address,
                Document = model.Document,
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageId = model.ImageId,
                PhoneNumber = model.PhoneNumber,
                City = await _context.Cities.FindAsync(model.CityId),
                UserName = model.Username,
                UserType = model.UserType
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            User newUser = await GetUserAsync(model.Username);
            await AddUserToRoleAsync(newUser, user.UserType.ToString());
            return newUser;

        }

        public async Task AddUserToRoleAsync(User user, string roleName)//Para asignar un rol
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName) //Verificar si el rol existe
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists) //Si el rol no existe lo crea
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }

        }

        public async Task<User> GetUserAsync(string email) //Para buscar el usuario
        {
            return await _context.Users//busca en nuestro contexto de usuario
            .Include(u => u.City)//nos incluye la ciudad
            .FirstOrDefaultAsync(u => u.Email == email);

        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName) //Busca el rol del usuario
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
