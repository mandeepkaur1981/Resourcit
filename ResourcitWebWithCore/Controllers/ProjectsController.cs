using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ResourcitModels.Models;

namespace ResourcitWebWithCore.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IConfiguration _config;

        public ProjectsController(IConfiguration configuration)
        {
            _config = configuration;
        }
        public async Task<IActionResult> Index()
        {
            List<Project> projects = new List<Project>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44364/api/projects"))
                {

                    string apiResponse = await response.Content.ReadAsStringAsync();
                    projects = JsonConvert.DeserializeObject<List<Project>>(apiResponse);
                    //XmlSerializer x = new XmlSerializer(typeof(Project), new XmlRootAttribute
                    //{
                    //    ElementName = "ArrayOfProject",
                    //    Namespace = "http://schemas.datacontract.org/2004/07/ResourcitModels.Models"
                    //});
                    //using (StringReader sr = new StringReader(apiResponse))
                    //{
                    //    XmlDocument xd = new XmlDocument();

                    //    xd.LoadXml(apiResponse);

                    //    projects.Add((Project)x.Deserialize(sr));
                    //}
                }
            }
            return View(projects);
        }
        [HttpGet]
        public IActionResult AddProject()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProject(Project project)
        {
            if (ModelState.IsValid)
            {
                Project addedProject = new Project();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Key", _config.GetValue<string>("Authentication:Keys"));
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(project), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:44364/api/projects", stringContent))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        addedProject = JsonConvert.DeserializeObject<Project>(apiResponse);
                    }
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditProject(int projectId)
        {
            Project project = new Project();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:44364/api/projects/{projectId}"))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return View("Error", new ErrorData() { statusCode = response.StatusCode.ToString(), errorMessage = "Unable to edit! Please try again later" });
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    project = JsonConvert.DeserializeObject<Project>(apiResponse);
                }
            }
            return View("EditProject", project);
        }
        [HttpPost]
        public async Task<IActionResult> EditProject(Project project)
        {
            if (ModelState.IsValid)
            {
                
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Key", _config.GetValue<string>("Authentication:Keys"));
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(project), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync($"https://localhost:44364/api/projects/{project.ProjectId}", stringContent))
                    {
                        if (response.StatusCode != HttpStatusCode.NoContent)
                        {
                            return View("Error", new ErrorData() { statusCode = response.StatusCode.ToString(), errorMessage = "Unable to edit! Please try again later" });
                        }
                        //string apiResponse = await response.Content.ReadAsStringAsync();
                        //addedResource = JsonConvert.DeserializeObject<Resource>(apiResponse);
                    }
                }

                return RedirectToAction("Index");
            }
            return View("EditProject", project);
        }
        public async Task<IActionResult> DeleteProject(int id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Key", _config.GetValue<string>("Authentication:Keys"));
                using (var response = await httpClient.DeleteAsync($"https://localhost:44364/api/projects/{id}"))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return View("Error", new ErrorData() { statusCode = response.StatusCode.ToString(), errorMessage = "Unable to edit! Please try again later" });
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}