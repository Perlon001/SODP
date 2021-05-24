using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SODP.DataAccess;
using SODP.Domain.DTO;
using SODP.Domain.Helpers;
using SODP.Domain.Services;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SODP.Application.Services
{
    public class DesignersService : IDesignersService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<Designer> _validator;
        private readonly SODPDBContext _context;

        public DesignersService(IMapper mapper, IValidator<Designer> validator, SODPDBContext context)
        {
            _mapper = mapper;
            _validator = validator;
            _context = context;
        }

        public async Task<ServicePageResponse<DesignerDTO>> GetAllAsync(int currentPage = 1, int pageSize = 0)
        {
            return await GetAllAsync(currentPage, pageSize, false);
        }

        public async Task<ServicePageResponse<DesignerDTO>> GetAllAsync(int currentPage = 1, int pageSize = 0, bool active = true)
        {
            var serviceResponse = new ServicePageResponse<DesignerDTO>();
            try
            {
                if(pageSize == 0)
                {
                    pageSize = serviceResponse.Data.TotalCount;
                }
                IList<Designer> designers = new List<Designer>();
                serviceResponse.Data.TotalCount = await _context.Designers.Where(x => active ? x.Active : true).CountAsync();
                if(pageSize == 0)
                {
                    pageSize = serviceResponse.Data.TotalCount;
                }
                designers = await _context.Designers.OrderBy(x => x.Lastname)
                    .ThenBy(y => y.Firstname)
                    .Where(z => active ? z.Active : true)
                    .Skip(currentPage * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                serviceResponse.Data.PageNumber = currentPage;
                serviceResponse.Data.PageSize = pageSize;
                serviceResponse.SetData(_mapper.Map<IList<DesignerDTO>>(designers));
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<DesignerDTO>> GetAsync(int designerId)
        {
            var serviceResponse = new ServiceResponse<DesignerDTO>();
            try
            {
                var designer = await _context.Designers.FirstOrDefaultAsync(x => x.Id == designerId);
                if (designer == null)
                {
                    serviceResponse.SetError($"Błąd: Projektant Id:{designerId} nie odnaleziony.", 404);
                }
                serviceResponse.SetData(_mapper.Map<DesignerDTO>(designer));
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<DesignerDTO>> CreateAsync(DesignerDTO createDesigner)
        {
            var serviceResponse = new ServiceResponse<DesignerDTO>();
            try
            {
                var exist = await _context.Designers.FirstOrDefaultAsync(x => x.Firstname.Trim().Equals(createDesigner.Firstname.Trim()) && x.Lastname.Trim().Equals(createDesigner.Lastname.Trim()));
                if (exist != null)
                {

                    serviceResponse.SetError($"Projektant {createDesigner} już istnieje.", 400);
                    serviceResponse.ValidationErrors.Add("Designer", "Projektant już istnieje.");
                    return serviceResponse;
                }
                var designer = _mapper.Map<Designer>(createDesigner);
                var validationResult = await _validator.ValidateAsync(designer);
                if (!validationResult.IsValid)
                {
                    serviceResponse.ValidationErrorProcess(validationResult);
                    return serviceResponse;
                }

                designer.Normalize();
                var entity = await _context.AddAsync(designer);
                await _context.SaveChangesAsync();

                serviceResponse.SetData(_mapper.Map<DesignerDTO>(entity.Entity));
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;

        }

        public Task<ServiceResponse<DesignerDTO>> UpdateAsync(DesignerDTO designer)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
