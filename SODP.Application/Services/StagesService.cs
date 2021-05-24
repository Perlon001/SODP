using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using SODP.Model;
using SODP.Domain.Services;
using SODP.DataAccess;
using SODP.Domain.DTO;
using System.Collections.Generic;
using FluentValidation;
using SODP.Domain.Helpers;

namespace WebSODP.Application.Services
{
    public class StagesService : IStagesService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<Stage> _validator;
        private readonly SODPDBContext _context;

        public StagesService(IMapper mapper, IValidator<Stage> validator, SODPDBContext context)
        {
            _mapper = mapper;
            _validator = validator;
            _context = context;
        }

        public async Task<ServicePageResponse<StageDTO>> GetAllAsync(int currentPage = 1, int pageSize = 0)
        {
            var serviceResponse = new ServicePageResponse<StageDTO>();
            try
            {
                IQueryable<Stage> stages = _context.Stages.OrderBy(x => x.Sign);
                serviceResponse.Data.TotalCount = stages.Count();
                if (pageSize == 0)
                {
                    currentPage = 1;
                    pageSize = serviceResponse.Data.TotalCount;
                }
                else
                {
                    currentPage = Math.Min(currentPage, (int)Math.Ceiling(decimal.Divide(serviceResponse.Data.TotalCount, pageSize)));
                }

                var st = await stages
                    .Skip((currentPage-1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                serviceResponse.Data.TotalCount = stages.Count();
                serviceResponse.Data.PageNumber = currentPage;
                serviceResponse.Data.PageSize = pageSize;
                serviceResponse.SetData(_mapper.Map<IList<StageDTO>>(st));
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<StageDTO>> GetAsync(int stageId)
        {
            var serviceResponse = new ServiceResponse<StageDTO>();
            try
            {
                var stage = await _context.Stages.FirstOrDefaultAsync(x => x.Id == stageId);

                serviceResponse.SetData(_mapper.Map<StageDTO>(stage));
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<StageDTO>> GetAsync(string sign)
        {
            var serviceResponse = new ServiceResponse<StageDTO>();
            try
            {
                var stage = await _context.Stages.FirstOrDefaultAsync(x => x.Sign == sign);

                serviceResponse.SetData(_mapper.Map<StageDTO>(stage));
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<StageDTO>> CreateAsync(StageDTO createStage)
        {
            var serviceResponse = new ServiceResponse<StageDTO>();
            try
            {
                var exist = await _context.Stages.FirstOrDefaultAsync(x => x.Sign.ToUpper() == createStage.Sign.ToUpper());
                if(exist != null)
                {

                    serviceResponse.SetError($"Stadium {createStage.Sign.ToUpper()} już istnieje.", 400);
                    serviceResponse.ValidationErrors.Add("Sign", "Stadium już istnieje.");
                    return serviceResponse;
                }
                var stage = _mapper.Map<Stage>(createStage);
                var validationResult = await _validator.ValidateAsync(stage);
                if (!validationResult.IsValid)
                {
                    serviceResponse.ValidationErrorProcess(validationResult);
                    return serviceResponse;
                }

                stage.Normalize();
                var entity = await _context.AddAsync(stage);
                await _context.SaveChangesAsync();

                serviceResponse.SetData(_mapper.Map<StageDTO>(entity.Entity));
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> UpdateAsync(StageDTO updateStage)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var stage = await _context.Stages.FirstOrDefaultAsync(x => x.Id == updateStage.Id);
                if(stage == null)
                {
                    serviceResponse.SetError($"Stadium {updateStage.Id} nie odnalezione.", 404);
                    serviceResponse.ValidationErrors.Add("Sign", "Stadium nie odnalezione.");
                    return serviceResponse;
                }
                stage.Title = updateStage.Title;
                stage.Normalize();
                _context.Stages.Update(stage);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteAsync(int stageId)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var stage = await _context.Stages.FirstOrDefaultAsync(x => x.Id == stageId);
                if (stage == null)
                {
                    serviceResponse.SetError($"Stadium [{stageId}] nie odnalezione.", 404);
                    serviceResponse.ValidationErrors.Add("Id", $"Stadium [{stageId}] nie odnalezione.");

                    return serviceResponse;
                }
                var project = await _context.Projects.FirstOrDefaultAsync(x => x.Stage.Id == stageId);
                if(project != null)
                {
                    serviceResponse.SetError($"Stadium {project.Stage.Sign} posiada powiązane projekty.", 400);
                    serviceResponse.ValidationErrors.Add("Id", $"Stadium [{stageId}] posiada powiązane projekty.");
                    return serviceResponse;
                }
                _context.Entry(stage).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteAsync(string sign)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var stage = await _context.Stages.FirstOrDefaultAsync(x => x.Sign == sign);
                if (stage == null)
                {
                    serviceResponse.SetError($"Stadium Sign:{sign} nie odnalezione.", 404);
                    serviceResponse.ValidationErrors.Add("Sign", $"Stadium {sign} nie odnalezione.");
                    return serviceResponse;
                }
                var project = await _context.Projects.FirstOrDefaultAsync(x => x.Stage.Sign == sign);
                if(project != null)
                {
                    serviceResponse.SetError($"Stadium {sign} posiada powiązane projekty.", 409);
                    serviceResponse.ValidationErrors.Add("Sign", $"Stadium {sign} posiada powiązane projekty.");
                    return serviceResponse;
                }
                _context.Entry(stage).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
                return serviceResponse;
            }

            return serviceResponse;
        }

        public async Task<bool> ExistAsync(string sign)
        {
            var result = await _context.Stages.FirstOrDefaultAsync(x => x.Sign == sign);

            return result != null;
        }

        public async Task<bool> ExistAsync(int id)
        {
            var result = await _context.Stages.FirstOrDefaultAsync(x => x.Id == id);

            return result != null;
        }
    }
}
