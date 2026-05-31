
namespace APi.DTOs
{
    public class RoleDto
    {
      public string id { get; set; }
      public string name { get; set; }
    }
    public class CreateRoleDto
    {
        public string name { get; set; }
    }
    public class UpdateRoleDto
    {
        public string name { get; set; }
    }
}
