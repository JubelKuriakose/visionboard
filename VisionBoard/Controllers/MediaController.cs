using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using VisionBoard.DAL;
using VisionBoard.Models;
using VisionBoard.Utilis;

namespace VisionBoard.Controllers
{
    public class MediaController : Controller
    {

        private readonly IErrorLogRepository errorLogRepository;

        public MediaController(IErrorLogRepository errorLogRepository)
        {
            this.errorLogRepository = errorLogRepository;
        }

        public async Task<IActionResult> CustomCrop(string source)
        {
            try
            {
                ViewBag.Source = source;
                string requestType = Request.Headers[HeaderNames.XRequestedWith];
                return View();
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        [HttpPost]
        public async Task<IActionResult> CustomCrop(string filename, string source, IFormFile blob)
        {
            string newfileName = string.Empty;
            string filepath = string.Empty;

            try
            {
                using (var image = Image.Load(blob.OpenReadStream()))
                {
                    string systemFileExtenstion = filename.Substring(filename.LastIndexOf('.'));

                    image.Mutate(x => x.Resize(345, 289));
                    newfileName = $"{"Photo_345_289"}_{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{systemFileExtenstion}";
                    filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\{newfileName}";
                    image.Save(filepath);

                }
                return Json(new { Message = "SUCCESS", Source = source, SelectedImage = newfileName });

            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return Json(new { Message = "ERROR", Source = source, SelectedImage = string.Empty });

        }


    }
}
