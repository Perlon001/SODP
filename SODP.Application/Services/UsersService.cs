using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SODP.DataAccess;
using SODP.Domain.Services;
using SODP.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SODP.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<User> _validator;
        private readonly UserManager<User> _userManager;
        private readonly SODPDBContext _context;

        public UsersService(IMapper mapper, IValidator<User> validator, UserManager<User> userManager, SODPDBContext context)
        {
            _mapper = mapper;
            _validator = validator;
            _userManager = userManager;
            _context = context;

        }
        public async Task<ServicePageResponse<User>> GetAllAsync()
        {
            var serviceResponse = new ServicePageResponse<User>();
            serviceResponse.Data.Collection = await _context.Users
                .OrderBy(x => x.UserName)
                .ToListAsync();

            return serviceResponse;
        }

        public async Task<ServiceResponse<User>> GetAsync(int id)
        {
            var serviceResponse = new ServiceResponse<User>();
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user != null)
                {
                    serviceResponse.Data = user;
                }
            }
            catch(Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<User>> DeleteAsync(int id)
        {
            var serviceResponse = new ServiceResponse<User>();
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                var result = await _userManager.SetLockoutEnabledAsync(user, false);
                if (result.Succeeded)
                {
                    serviceResponse.StatusCode = 204;
                }
            }
            catch(Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<User>> UpdateAsync(User user)
        {
            var serviceResponse = new ServiceResponse<User>();
            try
            {
                if(user == null || user.Id == 0)
                {
                    serviceResponse.SetError("Użytkownik nie odnaleziony.");
                    serviceResponse.StatusCode = 404;
                    return serviceResponse;
                }
                var currentUser = await _userManager.FindByIdAsync(user.Id.ToString());
                currentUser.Surname = user.Surname;
                currentUser.Forename = user.Forename;
                var result = await _userManager.UpdateAsync(currentUser);
                if (result.Succeeded)
                {
                    serviceResponse.StatusCode = 204;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }
    }
}
