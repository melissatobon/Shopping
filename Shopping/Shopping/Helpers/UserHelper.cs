using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Entities;
using Shopping.Models;
using System.IO;

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
                UserType = model.UserType,
                ImageName= model.ImageFile.FileName
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

        public async Task<User> AddUserAsync(AddUserViewModel model, string imagePath)
        {
            User user = new User
            {
                Address = model.Address,
                Document = model.Document,
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageId = model.ImageId,
                ImageSource= imagePath,
                PhoneNumber = model.PhoneNumber,
                City = await _context.Cities.FindAsync(model.CityId),
                UserName = model.Username,
                UserType = model.UserType,
                ImageName= model.ImageFile.FileName
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

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)//Para cambiar contraseña
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

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

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        //public Task<string> DeleteImageProductAsync(string ejemplo)
        //{
        //    string fiu = Path.Combine("C:\\Projects\\Shopping\\Shopping\\Shopping\\Resources\\ProductImages\\", Path.GetFileName(ejemplo));
        //    if (File.Exists(fiu))
        //    {
        //        try
        //        {
        //            File.Delete(fiu);
        //        }
        //        catch (IOException ev)
        //        {
        //            //Console.WriteLine(ev.Message);
        //            return;
        //        }
        //    }
        //}

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

    

    public async Task<User> GetUserAsync(string email) //Para buscar el usuario
        {
            return await _context.Users//busca en nuestro contexto de usuario
            .Include(u => u.City)//nos incluye la ciudad
            .ThenInclude(c=>c.State)
            .ThenInclude(s=>s.Country)
            .FirstOrDefaultAsync(u => u.Email == email);

        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _context.Users//busca en nuestro contexto de usuario
            .Include(u => u.City)//nos incluye la ciudad
            .ThenInclude(c => c.State)
            .ThenInclude(s => s.Country)
            .FirstOrDefaultAsync(u => u.Id == userId.ToString());
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName) //Busca el rol del usuario
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)//Actualizar usuario
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<string> UploadImageAsync(string ejemplo)
        {
            
            //string fiu = Path.Combine("C:\\Projects\\Shopping\\Shopping\\Shopping\\Resources\\UserImages\\", Path.GetFileName(ejemplo));
            string path = Path.Combine($"{Environment.CurrentDirectory}\\wwwroot\\UserImages\\{ejemplo}");

            return path;
        }

        public async Task<string> UploadImageProductAsync(string ejemplo)
        {
            string path = Path.Combine($"{Environment.CurrentDirectory}\\wwwroot\\ProductImages\\{ejemplo}");
            return path;
        }


    }
}
