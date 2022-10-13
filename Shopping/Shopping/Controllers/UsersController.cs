﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Common;
using Shopping.Data;
using Shopping.Entities;
using Shopping.Enums;
using Shopping.Helpers;
using Shopping.Models;
using System.Data;

namespace Shopping.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        //private readonly IBlobHelper _blobHelper;

        public UsersController(IUserHelper userHelper, DataContext context, ICombosHelper combosHelper
           /* IBlobHelper blobHelper*/)
        {
            _userHelper = userHelper;
            _context = context;
            _combosHelper = combosHelper;
            //_blobHelper = blobHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                .Include(u => u.City)
                .ThenInclude(c => c.State)
                .ThenInclude(s => s.Country)
                .ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Id = Guid.Empty.ToString(),
                Countries = await _combosHelper.GetComboCountriesAsync(),
                States = await _combosHelper.GetComboStatesAsync(0),
                Cities = await _combosHelper.GetComboCitiesAsync(0),
                UserType = UserType.Admin,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                string imagePath = String.Empty;
                string ejemplo = model.ImageFile.FileName;

                if (model.ImageFile != null)
                {
                    //imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    imagePath = await _userHelper.UploadImageAsync(ejemplo);

                    System.IO.File.Create(imagePath);
                }
                model.ImageId = imageId;

                //User user = await _userHelper.AddUserAsync(model);
                User user = await _userHelper.AddUserAsync(model, imagePath);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    model.Countries = await _combosHelper.GetComboCountriesAsync();
                    model.States = await _combosHelper.GetComboStatesAsync(model.CountryId);
                    model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
                    return View(model);
                }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);//Llamamos el metodo y le mandamos el usuario y el nos devuelve el token
                string tokenLink = Url.Action("ConfirmEmail", "Account", new// Con el token generamos el link y lo enviamos al metodo confirmEmail en el controlador
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);
                //TODO: Confirmar usuario por Mail
                //Response response = _mailHelper.SendMail(//Aquí llamamos el metodo para enviar el correo
                //    $"{model.FirstName} {model.LastName}",
                //    model.Username,
                //    "Shopping - Confirmación de Email",
                //    $"<h1>Shopping - Confirmación de Email</h1>" +
                //        $"Para habilitar el usuario por favor hacer click en el siguiente link:, " +
                //        $"<hr/><br/><p><a href = \"{tokenLink}\">Confirmar Email</a></p>");
                Response response = new Response();
                response.IsSuccess = true;

                if (response.IsSuccess)
                {
                    ViewBag.Message = "Las instrucciones para habilitar el usuario han sido enviadas al correo.";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }

            model.Countries = await _combosHelper.GetComboCountriesAsync();
            model.States = await _combosHelper.GetComboStatesAsync(model.CountryId);
            model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
            return View(model);
        }


        public JsonResult? GetStates(int countryId)
        {
            Country? country = _context.Countries
                .Include(c => c.States)
                .FirstOrDefault(c => c.Id == countryId);
            if (country == null)
            {
                return null;
            }

            return Json(country.States.OrderBy(d => d.Name));
        }

        public JsonResult? GetCities(int stateId)
        {
            State? state = _context.States
                .Include(s => s.Cities)
                .FirstOrDefault(s => s.Id == stateId);
            if (state == null)
            {
                return null;
            }

            return Json(state.Cities.OrderBy(c => c.Name));
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }
    }


}
