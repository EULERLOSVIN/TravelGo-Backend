using Domain.Entities;
﻿using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class GetTypesDocumentsRepository : IGetTypesDocumentsRepository
    {
        private ApplicationDbContext _context;
        public GetTypesDocumentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TypeDocumentDto>> GetTypesDocuments()
        {
            var typesDocuments = await _context.TypeDocuments.ToListAsync();
            return typesDocuments.Select(x => new TypeDocumentDto { Id = x.IdTypeDocument, Name = x.Name }).ToList();
        }
    }
}
