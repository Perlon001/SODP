using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SODP.DataAccess;
using SODP.Domain.Managers;
using SODP.Domain.Services;
using SODP.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebSODP.Application.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly IMapper _mapper;
        private readonly IFolderManager _folderManager;
        private readonly IValidator<Project> _validator;
        private readonly SODPDBContext _context;

        public ProjectsService(IMapper mapper, IFolderManager folderManager, IValidator<Project> validator, SODPDBContext context)
        {
            _mapper = mapper;
            _folderManager = folderManager;
            _validator = validator;
            _context = context;
        }

        public async Task<ServicePageResponse<Project>> GetAllAsync(int page_number = 0, int page_size = 0)
        {
            var serviceResponse = new ServicePageResponse<Project>();

            serviceResponse.Data.TotalCount = await _context.Projects.CountAsync();
            if (page_size == 0)
            {
                page_size = serviceResponse.Data.TotalCount;
            }
            serviceResponse.Data.Collection = await _context.Projects.Include(s => s.Stage)
                .OrderBy(x => x.Number)
                .ThenBy(y => y.Stage.Sign)
                .Skip(page_number * page_size)
                .Take(page_size)
                .ToListAsync();
            serviceResponse.Data.PageNumber = page_number;
            serviceResponse.Data.PageSize = page_size;

            return serviceResponse;
        }

        public async Task<ServiceResponse<Project>> GetAsync(int id)
        {
            var serviceResponse = new ServiceResponse<Project>();
            try
            {
                serviceResponse.Data = await _context.Projects
                    .Include(s => s.Stage)
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Project>> CreateAsync(Project createProject)
        {
            var serviceResponse = new ServiceResponse<Project>();
            try
            {
                var exist = await _context.Projects.Include(x => x.Stage).FirstOrDefaultAsync(x => x.Number == createProject.Number && x.Stage.Id == createProject.StageId);
                if(exist != null)
                {
                    serviceResponse.SetError(string.Format("Project {0} already exist.", exist.Symbol));
                    return serviceResponse;
                }

                var validationResult = await _validator.ValidateAsync(createProject);
                if (!validationResult.IsValid)
                {
                    var error = "";
                    foreach(var item in validationResult.Errors)
                    {
                        error += string.Format("{0}: {1}\n ",item.PropertyName, item.ErrorMessage);
                    }
                    serviceResponse.SetError(error);
                    return serviceResponse;
                }

                createProject.Normalize();
                createProject.Stage = await _context.Stages.FirstOrDefaultAsync(x => x.Id == createProject.StageId);
                var (Command, Success) = await _folderManager.CreateOrUpdateFolderAsync(createProject);
                if (!Success)
                {
                    serviceResponse.SetError(string.Format("Create folder error: {0}.\n {1}\n ", createProject.ToString(), Command));
                    return serviceResponse;
                }

                createProject.Location = createProject.ToString();
                var entity = await _context.AddAsync(createProject);
                await _context.SaveChangesAsync();
                serviceResponse.Data = entity.Entity;
                return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.SetError(ex.Message);
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<Project>> UpdateAsync(Project updateProject)
        {
            var serviceResponse = new ServiceResponse<Project>();
            try
            {
                var oldProject = await _context.Projects.Include(x => x.Stage).FirstOrDefaultAsync(x => x.Id == updateProject.Id);
                if(oldProject == null)
                {
                    serviceResponse.SetError(string.Format("Project Id:{0} not exist.", updateProject.Id));
                    return serviceResponse;
                }

                updateProject.Stage = await _context.Stages.FirstOrDefaultAsync(x => x.Id == updateProject.StageId);
                var validationResult = await _validator.ValidateAsync(updateProject);
                if(!validationResult.IsValid)
                {
                    var error = "";
                    foreach(var item in validationResult.Errors)
                    {
                        error += string.Format("{0}: {1}\n ",item.PropertyName, item.ErrorMessage);
                    }
                    serviceResponse.SetError(error);
                    return serviceResponse;
                }

                updateProject.Normalize();
                var (Command, Success) = await _folderManager.CreateOrUpdateFolderAsync(updateProject);
                if (!Success)
                {
                    serviceResponse.SetError(string.Format("Create/Modify folder error: {0}. {1}", updateProject.ToString(), Command));
                    return serviceResponse;
                }

                oldProject.Title = updateProject.Title;
                oldProject.Description = updateProject.Description;
                oldProject.Location = updateProject.ToString();
                _context.Projects.Update(oldProject);
                await _context.SaveChangesAsync();
                serviceResponse.Data = oldProject;
            }
            catch(Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Project>> DeleteAsync(int id)
        {
            var serviceResponse = new ServiceResponse<Project>();
            try
            {
                var project = await _context.Projects.Include(x => x.Stage).FirstOrDefaultAsync(x => x.Id == id); 
                if(project == null)
                {
                    serviceResponse.SetError(string.Format("Project Id:{0} not exist.",id.ToString()));
                    return serviceResponse;
                }
                var (Command, Success) = await _folderManager.DeleteFolderAsync(project);
                if (!Success)
                {
                    serviceResponse.SetError(string.Format("Delete folder error: {0}.\n {1}\n ", project.Symbol, Command));
                    return serviceResponse;
                }
                _context.Entry(project).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }


    }
}
