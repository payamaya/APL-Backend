namespace Application.Interfaces.Base
{
    public interface ICrudService<TDto>
    {
        Task<TDto> CreateAsync(TDto dto);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(Guid id);
        Task<TDto> UpdateAsync(TDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
