using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ResourcitModels.Models;
using ResourcitModels.ViewModels;

namespace ResourcitWebWithCore.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly IConfiguration _config;

        public ResourcesController(IConfiguration configuration)
        {
            _config = configuration;
        }
        
        public async Task<IActionResult> Index()
        {
            List<Resource> resources = new List<Resource>();
            using(var httpClient = new HttpClient())
            {
                
                using(var response = await httpClient.GetAsync("https://localhost:44364/api/resources"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    resources = JsonConvert.DeserializeObject<List<Resource>>(apiResponse);
                }
            }
            return View(resources);
        }

        [HttpGet]
        public IActionResult AddResource()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddResource(Resource resource)
        {
            if (ModelState.IsValid)
            {
                Resource addedResource = new Resource();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Key", _config.GetValue<string>("Authentication:Keys"));
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(resource), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:44364/api/resources", stringContent))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        addedResource = JsonConvert.DeserializeObject<Resource>(apiResponse);
                    }
                }
                return RedirectToAction("Index"); 
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditResource(int resourceId)
        {
            Resource res = new Resource();
            using(var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:44364/api/resources/{resourceId}"))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return View("Error", new ErrorData() { statusCode = response.StatusCode.ToString(), errorMessage = "Unable to edit! Please try again later" });
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    res = JsonConvert.DeserializeObject<Resource>(apiResponse);
                }
            }
            return View("EditResource", new ExtendResourceInfo() { resource=res });
        }
        [HttpPost]
        public async Task<IActionResult> EditResource(ExtendResourceInfo resourceInfo)
        {
            if (ModelState.IsValid)
            {
                //Resource addedResource = new Resource();
                if (resourceInfo.ProfilePhoto != null)
                {
                    var apiResponse = "";
                    resourceInfo.resource.ProfilePhotoPath = resourceInfo.ProfilePhoto.FileName;
                    using (var httpClient = new HttpClient())
                    {
                        var form = new MultipartFormDataContent();
                        using (var fileStream = resourceInfo.ProfilePhoto.OpenReadStream())
                        {
                            form.Add(new StreamContent(fileStream), "file", resourceInfo.ProfilePhoto.FileName);
                            using (var response = await httpClient.PostAsync("https://localhost:44364/api/resources/UploadFile", form))
                            {
                                response.EnsureSuccessStatusCode();
                                apiResponse = await response.Content.ReadAsStringAsync();
                            }
                        }
                    }
                }
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Key", _config.GetValue<string>("Authentication:Keys"));
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(resourceInfo.resource), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync($"https://localhost:44364/api/resources/{resourceInfo.resource.ResourceId}", stringContent))
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
            return View("EditResource", resourceInfo);
        }

        public async Task<IActionResult> DeleteResource(int id)
        {
            using(var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Key", _config.GetValue<string>("Authentication:Keys"));
                using (var response = await httpClient.DeleteAsync($"https://localhost:44364/api/resources/{id}"))
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