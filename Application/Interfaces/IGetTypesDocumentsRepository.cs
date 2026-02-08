using Application.DTOs;

namespace Application.Interfaces
{
    public interface IGetTypesDocumentsRepository
    {
        Task<List<TypeDocumentDto>> GetTypesDocuments();
    }
}
