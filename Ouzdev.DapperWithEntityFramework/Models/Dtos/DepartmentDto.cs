using System.ComponentModel.DataAnnotations;

namespace Ouzdev.DapperWithEntityFramework.Models.Dtos
{
    public class DepartmentDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}














































