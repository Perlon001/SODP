using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SODP.Application.Validators;
using SODP.DataAccess;
using SODP.Domain.Managers;
using SODP.Domain.Services;
using SODP.Model;
using SODP.Model.Extensions;
using System;
using System.IO;
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
                .ThenBy(y => y.StageSign)
                .Skip(page_number * page_size)
                .Take(page_size)
                .ToListAsync();
            serviceResponse.Data.PageNumber = page_number;
            serviceResponse.Data.PageSize = page_size;

            return serviceResponse;
        }

        public async Task<ServiceResponse<Project>> GetAsync(int? id)
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
                var project = _mapper.Map<Project>(createProject);
                var exist = await _context.Projects.FirstOrDefaultAsync(x => x.Number == project.Number && x.StageSign == project.StageSign);
                if(exist != null)
                {
                    serviceResponse.SetError(string.Format("Project {0} already exist.", project.Symbol));
                    return serviceResponse;
                }

                var validationResult = await _validator.ValidateAsync(project);
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

                project.Normalize();
                var result = await _folderManager.CreateOrUpdateFolder(project);
                if (!result.Item2)
                {
                    serviceResponse.SetError(string.Format("Create folder error: {0}.\n {1}\n ",project.ToString(), result.Item1));
                    return serviceResponse;
                }

                project.Location = project.ToString();
                var entity = await _context.AddAsync(project);
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
                var oldProject = await _context.Projects.FirstOrDefaultAsync(x => x.Id == updateProject.Id);
                if(oldProject == null)
                {
                    serviceResponse.SetError(string.Format("Project Id:{0} not exist.", updateProject.Id));
                    return serviceResponse;
                }

                var newProject = _mapper.Map<Project>(updateProject);
                var validationResult = await _validator.ValidateAsync(newProject);
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

                newProject.Normalize();
                var result = await _folderManager.CreateOrUpdateFolder(newProject);
                if (!result.Item2)
                {
                    serviceResponse.SetError(string.Format("Create/Modify folder error: {0}. {1}", newProject.ToString(), result.Item1));
                    return serviceResponse;
                }

                oldProject.Title = newProject.Title;
                oldProject.Description = newProject.Description;
                oldProject.Location = newProject.ToString();
                _context.Projects.Update(oldProject);
                await _context.SaveChangesAsync();
                serviceResponse.Data = oldProject;
                return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.SetError(ex.Message);
                return serviceResponse;
            }

        }

        public async Task<ServiceResponse<Project>> DeleteAsync(int id)
        {
            var serviceResponse = new ServiceResponse<Project>();
            try
            {
                var project = await _context.Projects.FindAsync(id); 
                if(project == null)
                {
                    serviceResponse.SetError(string.Format("Project Id:{0} not exist.",id.ToString()));
                    return serviceResponse;
                }
                var result = await _folderManager.DeleteFolder(project);
                if (!result.Item2)
                {
                    serviceResponse.SetError(string.Format("Delete folder error: {0}.\n {1}\n ", project.Symbol, result.Item1));
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

        private (string number, string sign) SymbolResolve(string catalog)
        {
            var symbol = Path.GetFileName(catalog).GetUntilOrEmpty("_");
            return (symbol.Substring(0,4), symbol[4..]);
        }
    }
}
