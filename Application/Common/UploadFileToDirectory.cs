using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Application.Common;

public class UploadFileToDirectory
{

    private readonly IHostingEnvironment _environment;

    public UploadFileToDirectory(IHostingEnvironment environment)
    {
        _environment = environment;
    }

    public UploadDto Upload(IFormFile file,string directory)
    {
        if (file != null)
        {
            string folder = $@"" + directory + "/";
            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, folder);
            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }

            if (file == null || file.Length == 0)
            {
                return new UploadDto()
                {
                    Status = false,
                    FileNameAddress = "",
                };
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsRootFolder, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return new UploadDto()
            {
                FileNameAddress = folder + fileName,
                Status = true,
            };
        }
        return null;
    }

    public class UploadDto
    {
        public bool Status { get; set; }
        public string? FileNameAddress { get; set; }
    }
}
