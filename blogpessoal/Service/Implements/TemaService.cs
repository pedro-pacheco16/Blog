using blogpessoal.Data;
using blogpessoal.model;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Service.Implements
{
    public class TemaService : ITemaService
    {
        private readonly AppDbContext _context;

        public TemaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tema>> GetAll()
        {
            return await _context.Temas
                .Include(t => t.Postagem)
                .ToListAsync();
        }

        public async Task<Tema?> GetById(long id)
        {
            try
            {
                var tema = await _context.Temas
                    .Include(t => t.Postagem)
                    .FirstAsync(i => i.Id == id);



                return tema;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Tema>> GetByDescricao(string descricao)
        {
            var Temas = await _context.Temas
                .Include(t => t.Postagem)
                .Where(t => t.Descricao.Contains(descricao))
                .ToListAsync();

            return Temas;
        }
        public async Task<Tema?> Create(Tema Tema)
        {
            await _context.Temas.AddAsync(Tema);
            await _context.SaveChangesAsync();

            return Tema;
        }

        public async Task Delete(Tema Tema)
        {
            _context.Remove(Tema);
            await _context.SaveChangesAsync();
        }


        public async Task<Tema?> Update(Tema Tema)
        {
            var temaUpdate = await _context.Temas.FindAsync(Tema.Id);

            if (temaUpdate is null)
                return null;

            _context.Entry(temaUpdate).State = EntityState.Detached;
            _context.Entry(Tema).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Tema;
        }
    }
}
