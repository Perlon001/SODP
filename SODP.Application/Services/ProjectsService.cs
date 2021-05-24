using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SODP.DataAccess;
using SODP.Domain.DTO;
using SODP.Domain.Helpers;
using SODP.Domain.Managers;
using SODP.Domain.Services;
using SODP.Model;
using SODP.Model.Enums;
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
        private ProjectServiceMode _mode = ProjectServiceMode.Active;

        public ProjectsService(IMapper mapper, IFolderManager folderManager, IValidator<Project> validator, SODPDBContext context)
        {
            _mapper = mapper;
            _folderManager = folderManager;
            _validator = validator;
            _context = context;
        }

        public IProjectsService SetArchiveMode()
        {
            _mode = ProjectServiceMode.Archive;
            return this;
        }

        public async Task<ServicePageResponse<ProjectDTO>> GetAllAsync(int currentPage = 0, int pageSize = 0)
        {
            var serviceResponse = new ServicePageResponse<ProjectDTO>();
            try
            {
                if (pageSize == 0)
                {
                    pageSize = serviceResponse.Data.TotalCount;
                }
                IList<Project> projects = new List<Project>();
                var query = _context.Projects.Include(s => s.Stage);
                switch(_mode)
                {
                    case ProjectServiceMode.Active:                
                        serviceResponse.Data.TotalCount = await _context.Projects.Where(x => x.Status == ProjectStatus.Active).CountAsync();
                        if (pageSize == 0)
                        {
                            pageSize = serviceResponse.Data.TotalCount;
                        }
                        projects = await _context.Projects.Include(s => s.Stage)
                            .OrderBy(x => x.Number)
                            .ThenBy(y => y.Stage.Sign)
                            .Where(x => x.Status == ProjectStatus.Active)
                            .Skip(currentPage * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                        break;
                    case ProjectServiceMode.Archive:
                        serviceResponse.Data.TotalCount = await _context.Projects.Where(x => x.Status == ProjectStatus.Archived).CountAsync();
                        if (pageSize == 0)
                        {
                            pageSize = serviceResponse.Data.TotalCount;
                        }
                        projects = await _context.Projects.Include(s => s.Stage)
                            .OrderBy(x => x.Number)
                            .ThenBy(y => y.Stage.Sign)
                            .Where(x => x.Status == ProjectStatus.Archived)
                            .Skip(currentPage * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                        break;
                }
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
                    serviceResponse.SetError($"Błąd: Projekt Id:{projectId} nie odnaleziony.", 404);
                }
                serviceResponse.SetData(_mapper.Map<ProjectDTO>(project));
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<ProjectDTO>> CreateAsync(ProjectDTO createProject)
        {
            var serviceResponse = new ServiceResponse<ProjectDTO>();
            try
            {
                var exist = await _context.Projects.Include(x => x.Stage).FirstOrDefaultAsync(x => x.Number == createProject.Number && x.Stage.Id == createProject.StageId);
                if(exist != null)
                {
                    serviceResponse.SetError($"Błąd: Projekt {exist.Symbol} już istnieje.", 400);
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
                var (Success, Message) = await _folderManager.CreateFolderAsync(project);
                if (!Success)
                {
                    serviceResponse.SetError($"Błąd: {Message}", 500);
                    
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

        public async Task<ServiceResponse> UpdateAsync(ProjectDTO updateProject)
        {
            var serviceResponse = new ServiceResponse<Project>();
            try
            {
                var oldProject = await _context.Projects.Include(x => x.Stage).FirstOrDefaultAsync(x => x.Id == updateProject.Id);
                if(oldProject == null)
                {
                    serviceResponse.SetError($"Błąd: Project Id:{updateProject.Id} nie odnaleziony.", 401);
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
                        error += $"{item.PropertyName}: {item.ErrorMessage}";
                    }
                    serviceResponse.SetError(error, 400);
                    return serviceResponse;
                }

                project.Normalize();
                var (Success, Message) = await _folderManager.RenameFolderAsync(project);
                if (!Success)
                {
                    serviceResponse.SetError($"Błąd: {Message}");
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

        public async Task<ServiceResponse> ArchiveAsync(int id)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var project = await _context.Projects.Include(x => x.Stage).FirstOrDefaultAsync(x => x.Id == id);
                if (project == null)
                {
                    serviceResponse.SetError($"Błąd: Project Id:{id} nie odnaleziony.", 401);
                    return serviceResponse;
                }

                // tu pierwsze pytanie czy może być SaveChangesAsync()? Tak myślę, że chyba nie.
                project.Status = ProjectStatus.DuringArchive;
                _context.SaveChanges();

                // tu drugie pytanie jak prawidłowo wywołać taska który korzysta z zewnętrznych serwisów lub systemu operacyjnego
                // task może się wykonaywać bardzo długo np. 1godz. 
                // czy tu nie należałoby jednak również zastosować metody synchronicznej
                var (Success, Message) = await _folderManager.ArchiveFolderAsync(project);
                if (!Success)
                {
                    serviceResponse.SetError($"Błąd: {Message}");
                    project.Status = ProjectStatus.Active;
                    _context.SaveChanges();
                    return serviceResponse;
                }
                
                // bo z drugiego wynika trzecie pytanie kiedy poniższy zapis się wykona. Chciałbym po zakończeniu archiwizacji
                project.Status = ProjectStatus.Archived;
                await _context.SaveChangesAsync();
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
                    serviceResponse.SetError($"Błąd: Project Id:{id} nie odnaleziony.", 401);
                    return serviceResponse;
                }
                var (Success, Message) = await _folderManager.DeleteFolderAsync(project);
                if (!Success)
                {
                    serviceResponse.SetError($"Błąd: {Message}");
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

        public async Task<ServiceResponse> RestoreAsync(int id)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var project = await _context.Projects.Include(x => x.Stage).FirstOrDefaultAsync(x => x.Id == id); 
                if(project == null)
                {
                    serviceResponse.SetError($"Błąd: Project Id:{id} nie odnaleziony.", 401);
                    return serviceResponse;
                }
                project.Status = ProjectStatus.DuringRestore;
                _context.SaveChanges();
                
                var (Success, Message) = await _folderManager.RestoreFolderAsync(project);
                if (!Success)
                {
                    serviceResponse.SetError($"Błąd: {Message}");
                    return serviceResponse;
                }
                project.Status = ProjectStatus.Active;
                _context.Entry(project).State = EntityState.Modified;
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
                    serviceResponse.SetError($"Błąd: Branża Id:{branchId} nie odnaleziona.", 404);
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

        public Task<ServicePageResponse<BranchDTO>> GetBranchesAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> AddBranchAsync(int projectId, int branchId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> DeleteBranchAsync(int projectId, int branchId)
        {
            throw new NotImplementedException();
        }
    }
}
