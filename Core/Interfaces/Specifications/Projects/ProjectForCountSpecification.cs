using Core.Specifications;
using Core.Entities;
using Core.Interfaces.Specifications.Projects;

public class ProjectsCountSpecification : BaseSpecification<Projects>
{
    public ProjectsCountSpecification(ProjectsSpecParams specParams)
   : base(x =>
       x.is_active &&
       (
           string.IsNullOrEmpty(specParams.Search) ||
           x.project_name.ToLower().Contains(specParams.Search)
              || x.project_code.ToLower().Contains(specParams.Search)
       ))
    {
       
    }
}
