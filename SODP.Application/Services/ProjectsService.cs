using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SODP.DataAccess;
using SODP.Domain.DTO;
using SODP.Domain.Helpers;
using SODP.Domain.Managers;
using SODP.Domain.Services;
using SODP.Model;
using System;
using System.Collections.Generic;
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

        public async Task<ServicePageResponse<ProjectDTO>> GetAllAsync()
        {
            return await GetAllAsync( 0, 0);
        }

        public async Task<ServicePageResponse<ProjectDTO>> GetAllAsync(int currentPage = 0, int pageSize = 0)
        {
            var serviceResponse = new ServicePageResponse<ProjectDTO>();
            try
            {
                serviceResponse.Data.TotalCount = await _context.Projects.CountAsync();
                if (pageSize == 0)
                {
                    pageSize = serviceResponse.Data.TotalCount;
                }
                var projects = await _context.Projects.Include(s => s.Stage)
                   .OrderBy(x => x.Number)
                   .ThenBy(y => y.Stage.Sign)
                   .Skip(currentPage * pageSize)
                   .Take(pageSize)
                   .ToListAsync();
                serviceResponse.Data.PageNumber = currentPage;
                serviceResponse.Data.PageSize = pageSize;
                serviceResponse.SetData(_mapper.Map<IList<ProjectDTO>>(projects));
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<ProjectDTO>> GetAsync(int projectId)
        {
            var serviceResponse = new ServiceResponse<ProjectDTO>();
            try
            {
                var project = await _context.Projects
                    .Include(s => s.Stage)
                    .FirstOrDefaultAsync(x => x.Id == projectId);
                if (project == null)
                {
                    serviceResponse.SetError(string.Format("Project Id:{0} nie odnaleziony.", projectId), 404);
                }
                serviceResponse.SetData(_mapper.Map<ProjectDTO>(project));
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<ProjectDTO>> CreateAsync(ProjectCreateDTO createProject)
        {
            var serviceResponse = new ServiceResponse<ProjectDTO>();
            try
            {
                var exist = await _context.Projects.Include(x => x.Stage).FirstOrDefaultAsync(x => x.Number == createProject.Number && x.Stage.Id == createProject.StageId);
                if(exist != null)
                {
                    serviceResponse.SetError(string.Format("Projekt {0} już istnieje.", exist.Symbol), 400);
                    return serviceResponse;
                }
                var project = _mapper.Map<Project>(createProject);
                var validationResult = await _validator.ValidateAsync(project);
                if (!validationResult.IsValid)
                {
                    serviceResponse.ValidationErrorProcess(validationResult);
                    return serviceResponse;
                }

                project.Normalize();
                project.Stage = await _context.Stages.FirstOrDefaultAsync(x => x.Id == project.StageId);
                var (Command, Success) = await _folderManager.CreateOrUpdateFolderAsync(project);
                if (!Success)
                {
                    serviceResponse.SetError(string.Format("Błąd tworzenia folderu roboczego: {0}, komenda: {1}", createProject.ToString(), Command), 500);
                    
                    return serviceResponse;
                }

                project.Location = createProject.ToString();
                var entity = await _context.AddAsync(project);
                await _context.SaveChangesAsync();
                serviceResponse.SetData(_mapper.Map<ProjectDTO>(entity.Entity));
            }
            catch(Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
        
            return serviceResponse;
        }

        public async Task<ServiceResponse> UpdateAsync(ProjectUpdateDTO updateProject)
        {
            var serviceResponse = new ServiceResponse<Project>();
            try
            {
                var oldProject = await _context.Projects.Include(x => x.Stage).FirstOrDefaultAsync(x => x.Id == updateProject.Id);
                if(oldProject == null)
                {
                    serviceResponse.SetError(string.Format("Project Id:{0} nie odnaleziony.", updateProject.Id), 401);
                    return serviceResponse;
                }
                var project = _mapper.Map<Project>(updateProject);
                project.Stage = await _context.Stages.FirstOrDefaultAsync(x => x.Id == project.StageId);
                var validationResult = await _validator.ValidateAsync(project);
                if(!validationResult.IsValid)
                {
                    var error = "";
                    foreach(var item in validationResult.Errors)
                    {
                        error += string.Format("{0}: {1}",item.PropertyName, item.ErrorMessage);
                    }
                    serviceResponse.SetError(error, 400);
                    return serviceResponse;
                }

                project.Normalize();
                var (Command, Success) = await _folderManager.CreateOrUpdateFolderAsync(project);
                if (!Success)
                {
                    serviceResponse.SetError(string.Format("Błąd tworzenia/modyfikacji folderu: {0}, komwnda: {1}", project.ToString(), Command));
                    return serviceResponse;
                }

                oldProject.Title = project.Title;
                oldProject.Description = project.Description;
                oldProject.Location = project.ToString();
                _context.Projects.Update(oldProject);
                await _context.SaveChangesAsync();
                serviceResponse.SetData( oldProject );
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
                var project = await _context.Projects.Include(x => x.Stage).FirstOrDefaultAsync(x => x.Id == id); 
                if(project == null)
                {
                    serviceResponse.SetError(string.Format("Project Id:{0} nie odnaleziony.", id.ToString()), 401);
                    return serviceResponse;
                }
                var (Command, Success) = await _folderManager.DeleteFolderAsync(project);
                if (!Success)
                {
                    serviceResponse.SetError(string.Format("Błąd usuwania folderu: {0}, komenda: {1}", project.Symbol, Command));
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

        public async Task<ServiceResponse> AddBrach(int projectId, int branchId)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == branchId);
                if (branch == null)
                {
                    serviceResponse.SetError(string.Format("Branża Id:{0} nie odnalziona.", branchId), 404);
                    return serviceResponse;
                }
                var projectBranch = await _context.ProjectBranches.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.BranchId == branchId);
                if (projectBranch == null)
                {
                    projectBranch = new ProjectBranch
                    {
                        ProjectId = projectId,
                        BranchId = branchId
                    };
                    var result = await _context.ProjectBranches.AddAsync(projectBranch);
                    await _context.SaveChangesAsync();
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
