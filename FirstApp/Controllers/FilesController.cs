using FirstApp.DBConnection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[Controller]")]
    public class FilesController : Controller
    {
        private readonly DBService _dbService;
        public FilesController(DBService dBService) 
        {
            _dbService = dBService;
        }

        [HttpPost]
        public async Task<IActionResult> postFile(IFormFile file )
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest();
            }

            byte[] fileData;
            using(var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileData = memoryStream.ToArray();
            }

            int result = 0;
            result = _dbService.AddFile(file.FileName, fileData);

            if (result != 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);

                //File.WriteAllBytes($"E:\\Tech\\DOT_NET\\FirstApp\\FirstApp\\Assets\\{file.FileName}", fileData);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", result.ToString() + fileExtension);
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Assets")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Assets"));
                }

                // Copy the file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return Ok();
        }
    }
}
