using blogpessoal.model;

namespace blogpessoal.Service
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll(); //é uma classe no .NET Framework que realiza tarefas assincronas

        Task<User?> GetById(long id);

        Task<User?> GetByUsuario(string usuario);

        Task<User?> Create(User usuario);

        Task<User?> Update(User usuario);


    }
}
