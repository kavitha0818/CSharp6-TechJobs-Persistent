using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechJobs6Persistent.ViewModels
{
    public class AddJobViewModel
    {
        [Required(ErrorMessage = "Job name is required")]
        public string Name { get; set; }

        [Display(Name = "Employer")]
        public int EmployerId { get; set; }

        public List<SelectListItem> Employers { get; set; }

        // Default constructor
        public AddJobViewModel()
        {
            Employers = new List<SelectListItem>();
        }
    }
}
