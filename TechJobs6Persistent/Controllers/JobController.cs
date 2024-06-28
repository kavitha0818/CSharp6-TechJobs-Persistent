using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechJobs6Persistent.Data;
using TechJobs6Persistent.Models;
using TechJobs6Persistent.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechJobs6Persistent.Controllers
{
    public class JobController : Controller
    {
        private JobDbContext context;

        public JobController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/Fetch-employer information
        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        // GET: /Job/Add
        public IActionResult Add()
        {
            // Fetch all employers from the database
            List<Employer> employers = context.Employers.ToList();

            // Create an instance of AddJobViewModel
            var viewModel = new AddJobViewModel();

            // Populate the Employers list in AddJobViewModel with SelectListItems
            viewModel.Employers = employers
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name
                })
                .ToList();

            // Pass viewModel to the Add view
            return View(viewModel);
        }

        // POST: /Job/Add
        [HttpPost]
        public IActionResult Add(AddJobViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Create new Job object
                Job newJob = new Job
                {
                    Name = viewModel.Name,
                    EmployerId = viewModel.EmployerId, 
                };

                // Add to database
                context.Jobs.Add(newJob);
                context.SaveChanges();

                // Redirect to Index or another action after successful creation
                return Redirect("/");
            }

            // If model state is not valid, reload the form with errors
            // Fetch employers again and populate viewModel
            viewModel.Employers = context.Employers
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name
                })
                .ToList();

            return View(viewModel);
        }
         //Fetch all jobs from the database pass -list to delete view
        public IActionResult Delete()
        {
            ViewBag.jobs = context.Jobs.ToList();

            return View();
        }

        [HttpPost]
        //Delete job from database- save change- return jobIndex
        public IActionResult Delete(int[] jobIds)
        {
            foreach (int jobId in jobIds)
            {
                Job theJob = context.Jobs.Find(jobId);
                context.Jobs.Remove(theJob);
            }

            context.SaveChanges();

            return Redirect("/Job");
        }
        //Fetch the job with id-include employer&skills - create JobDetailViewModel
        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs.Include(j => j.Employer).Include(j => j.Skills).Single(j => j.Id == id);

            JobDetailViewModel jobDetailViewModel = new JobDetailViewModel(theJob);

            return View(jobDetailViewModel);

        }
    }
}

