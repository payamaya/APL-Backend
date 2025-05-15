using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Base;
using Infrastructure.Repositories.Interfaces;


namespace Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repository;
        private readonly IMapper _mapper;

        public ActivityService(IActivityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityDto>> GetAllActivitiesAsync(Guid moduleId)
        {
            var activities = await _repository.GetAllAsync(moduleId);
            return _mapper.Map<IEnumerable<ActivityDto>>(activities);
        }

        public async Task<ActivityDto?> GetActivityByIdAsync(Guid moduleId, Guid activityId)
        {
            var activity = await _repository.GetByIdAsync(moduleId, activityId);
            return activity == null ? null : _mapper.Map<ActivityDto>(activity);
        }

        public async Task<ActivityDto> CreateActivityAsync(ActivityDto dto)
        {
            if (!await _repository.ModuleExists(dto.ModuleId))
                throw new InvalidOperationException($"Module {dto.ModuleId} doesn't exist.");

            var entity = _mapper.Map<Activity>(dto);
            await _repository.AddAsync(entity);
            return _mapper.Map<ActivityDto>(entity);
        }

        public async Task<ActivityDto> UpdateActivityAsync(ActivityDto dto)
        {
            var activity = await _repository.GetByIdAsync(dto.Id);
            if (activity == null) throw new Exception("Activity not found");

            if (activity.ModuleId != dto.ModuleId && !await _repository.ModuleExists(dto.ModuleId))
                throw new InvalidOperationException("Module not found.");

            _mapper.Map(dto, activity);
            await _repository.UpdateAsync(activity);
            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<bool> DeleteActivityAsync(Guid id)
        {
            var activity = await _repository.GetByIdAsync(id);
            if (activity == null) return false;

            await _repository.DeleteAsync(activity);
            return true;
        }
    }
}
