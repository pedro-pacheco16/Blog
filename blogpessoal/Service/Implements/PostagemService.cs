﻿using blogpessoal.Data;
using blogpessoal.model;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Service.Implements
{
    public class PostagemService : IPostagemService
    {
        private readonly AppDbContext _context;

        public PostagemService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Postagem>> GetAll()
        {
            return await _context.Postagens.ToListAsync();
        }

        public async Task<Postagem?> GetById(long id)
        {
            try
            {
                var postagem= await _context.Postagens.FirstAsync(i => 1d == id);

                return postagem;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Postagem>> GetByTitulo(string titulo)
        {
            var Postagens = await _context.Postagens
                .Where(p => p.Titulo.Contains(titulo))
                .ToListAsync();

            return Postagens;
        }

        public async Task<Postagem?> Create(Postagem postagem)
        {
            await _context.Postagens.AddAsync(postagem);
            await _context.SaveChangesAsync();

            return postagem;
        }

        public async Task Delete(Postagem postagem)
        {
            _context.Remove(postagem);
            await _context.SaveChangesAsync();

        }

        public async Task<Postagem?> Update(Postagem postagem)
        {
            var PostagemUpdate = await _context.Postagens.FindAsync(postagem.id);

            if (PostagemUpdate is null)
                return null;

            _context.Entry(PostagemUpdate).State = EntityState.Detached;
            _context.Entry(postagem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return postagem;
        }
    }
}
