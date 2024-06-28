using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechJobs6Persistent.Data;
using TechJobs6Persistent.Models;
using TechJobs6Persistent.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechJobs6Persistent.Controllers
{
    public class EmployerController : Controller
    {
        private readonly JobDbContext context;

     //JobDbContext - injected into the controller
        public EmployerController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/

        // GET: /Employer/ retrives all employers from the database
        [HttpGet]
        public IActionResult Index()
        {
            List<Employer> employers = context.Employers.ToList();
            return View(employers);
        }

        [HttpGet]
        //initializes - Add a new employer 
        public IActionResult Create()
        {
            AddEmployerViewModel viewModel = new AddEmployerViewModel();
            return View(viewModel);
        }

        // POST: /Employer/ProcessCreateEmployerForm/form data(viewModel)
        [HttpPost]
        public IActionResult ProcessCreateEmployerForm(AddEmployerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Employer newEmployer = new Employer
                {
                    Name = viewModel.Name,
                    Location = viewModel.Location,
                };

                context.Employers.Add(newEmployer);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            // return to screen if data is not valid
            return View("Create", viewModel);
        }

        public IActionResult About(int id)
        {
            Employer employer = context.Employers.Find(id);

            if (employer == null)
            {
                return NotFound(); // or handle not found scenario
            }

            return View(employer);
        }

    }
}
