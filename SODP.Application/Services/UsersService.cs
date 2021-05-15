using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SODP.DataAccess;
using SODP.Domain.DTO;
using SODP.Domain.Helpers;
using SODP.Domain.Services;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SODP.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SODPDBContext _context;
        private readonly IServiceProvider _serviceProvider;

        public UsersService(IMapper mapper, UserManager<User> userManager, SODPDBContext context, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
            _serviceProvider = serviceProvider;
        }
        public async Task<ServicePageResponse<UserDTO>> GetAllAsync()
        {
            var serviceResponse = new ServicePageResponse<UserDTO>();
            try
            {
                var users = await _context.Users.OrderBy(x => x.UserName).ToListAsync();
                serviceResponse.Data.Collection = _mapper.Map<IList<UserDTO>>(users);
                serviceResponse.StatusCode = 200;
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<UserDTO>> GetAsync(int id)
        {
            var serviceResponse = new ServiceResponse<UserDTO>();
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    serviceResponse.SetError("",404);
                }
                var userDTO = _mapper.Map<UserDTO>(user);
                userDTO.Roles = await _userManager.GetRolesAsync(user);

                serviceResponse.SetData(userDTO);
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                var result = await _userManager.SetLockoutEnabledAsync(user, false);
            }
            catch(Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> UpdateAsync(UserDTO user)
        {
            // required correct validation of user
            var serviceResponse = new ServiceResponse();
            if (user == null)
            {
                serviceResponse.SetError("Nieprawidłowe dane użytkownika.", 400);
                return serviceResponse;
            }
            try
            {
                var currentUser = await _userManager.FindByIdAsync(user.Id.ToString());
                if (currentUser == null)
                {
                    serviceResponse.SetError("Użytkownik nie odnaleziony.", 404);
                    return serviceResponse;
                }
                currentUser.Lastname = user.Lastname;
                currentUser.Firstname = user.Firstname;
                var result = await _userManager.UpdateAsync(currentUser);
                if (!result.Succeeded)
                {
                    serviceResponse.IdentityResultErrorProcess(result);
                    return serviceResponse;
                }
                (IdentityResult removeRolesResult, IdentityResult addRolesResult) = await _userManager.UpdateRolesAsync(currentUser, user.Roles);
                if (!removeRolesResult.Succeeded)
                {
                    serviceResponse.IdentityResultErrorProcess(result);
                    return serviceResponse;
                }
                if (!addRolesResult.Succeeded)
                {
                    serviceResponse.IdentityResultErrorProcess(result);
                }
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServicePageResponse<RoleDTO>> GetRolesAsync(int userId)
        {
            var serviceResponse = new ServicePageResponse<RoleDTO>();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    serviceResponse.SetError(string.Format("Użytkownik Id:{0} nie odnaleziony.", userId), 404);
                    return serviceResponse;
                }
                var roles = await _userManager.GetRolesAsync(user);
                serviceResponse.SetData(roles.Select(x => new RoleDTO {Role = x}).ToList());
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }
    }
}
