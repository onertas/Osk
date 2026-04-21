using OskApi.Entities;
using OskApi.Entities.Personnel;

namespace OskApi.Dtos.Personnel
{
    public class UpdatePersonnelDto : CreatePersonnelDto
    {
        public Guid Id { get; set; }
    }
}
