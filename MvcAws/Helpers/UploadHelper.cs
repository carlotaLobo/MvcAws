using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAws.Helpers
{
    public class UploadHelper
    {
        PathService path;

        public UploadHelper(PathService path)
        {
            this.path = path;
        }
        public async Task<String> UploadLocal(IFormFile file , Folder folder)
        {
            String path = this.path.MapPath(file.FileName, folder);
            using(var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return path;
        }
    }
}
