using School.Infrastructure.Bases;

namespace School.Infrastructure.Abstracts.Views
{
    public interface IViewRepository<T> : IGenericRepositoryAsync<T> where T : class
    {
    }
}
