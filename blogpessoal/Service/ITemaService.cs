using blogpessoal.model;

namespace blogpessoal.Service
{
    public interface ITemaService
    {
        Task<IEnumerable<Tema>> GetAll(); //é uma classe no .NET Framework que realiza tarefas assincronas

        Task<Tema?> GetById(long id);

        Task<IEnumerable<Tema>> GetByDescricao(string descricao);

        Task<Tema?> Create(Tema Tema);

        Task<Tema?> Update(Tema Tema);

        Task Delete(Tema Tema);
    }
}
