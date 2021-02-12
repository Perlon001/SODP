﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using SODP.Model;
using SODP.Domain.Services;
using SODP.DataAccess;
using SODP.Domain.DTO;

namespace WebSODP.Application.Services
{
    public class StagesService : IStagesService
    {
        private readonly IMapper _mapper;
        private readonly SODPDBContext _context;

        public StagesService(IMapper mapper, SODPDBContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        
        public async Task<ServicePageResponse<Stage>> GetAllAsync()
        {
            var serviceResponse = new ServicePageResponse<Stage>();
            try
            {
                serviceResponse.Data.Collection = await _context.Stages.OrderBy(x => x.Sign).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Stage>> GetAsync(int id)
        {
            var serviceResponse = new ServiceResponse<Stage>();
            try
            {
                serviceResponse.Data = await _context.Stages.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<Stage>> GetAsync(string sign)
        {
            var serviceResponse = new ServiceResponse<Stage>();
            try
            {
                serviceResponse.Data = await _context.Stages.FirstOrDefaultAsync(x => x.Sign == sign);
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Stage>> CreateAsync(Stage newStage)
        {
            var serviceResponse = new ServiceResponse<Stage>();
            try
            {
                var stage = _mapper.Map<Stage>(newStage);
                var exist = await _context.Stages.FirstOrDefaultAsync(x => x.Sign.ToUpper() == stage.Sign.ToUpper());
                if(exist != null)
                {
                    serviceResponse.SetError(string.Format("Stage {0} already exist.", stage.Sign.ToUpper()));
                    return serviceResponse;
                }
                stage.Normalize();
                var entity = await _context.AddAsync(stage);
                await _context.SaveChangesAsync();
                serviceResponse.Data = entity.Entity;
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Stage>> UpdateAsync(Stage updateStage)
        {
            var serviceResponse = new ServiceResponse<Stage>();
            try
            {
                var stage = await _context.Stages.FirstOrDefaultAsync(x => x.Sign == updateStage.Sign);
                if(stage == null)
                {
                    return await CreateAsync(updateStage);
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

        public async Task<ServiceResponse<Stage>> DeleteAsync(int id)
        {
            var serviceResponse = new ServiceResponse<Stage>();
            try
            {
                var project = await _context.Projects.FirstOrDefaultAsync(x => x.Stage.Id == id);
                if (project == null)
                {
                    var stage = await _context.Stages.FirstOrDefaultAsync(x => x.Id == id);
                    if(stage == null)
                    {
                        serviceResponse.SetError(string.Format("Nie znaleziono stadium id:{0}.", id), 404);
                    }
                    else
                    {
                        var result = _context.Stages.Remove(stage);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    serviceResponse.SetError(string.Format("Istnieją projekty w stadium id:{0}.",id), 409);
                }
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Stage>> DeleteAsync(string sign)
        {
            var serviceResponse = new ServiceResponse<Stage>();
            try
            {
                var project = await _context.Projects.FirstOrDefaultAsync(x => x.Stage.Sign == sign);
                if(project == null)
                {
                    var stage = _context.Stages.Remove(await _context.Stages.FindAsync(sign));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    serviceResponse.SetError("Istnieją projekty w tym stadium.", 409);
                    return serviceResponse;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
                return serviceResponse;
            }
            serviceResponse.Message = "Operacja zakończona powodzeniem.";
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
