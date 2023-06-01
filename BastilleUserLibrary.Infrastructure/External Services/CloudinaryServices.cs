using BastilleUserService.Core.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BastilleUserLibrary.Infrastructure.External_Services
{
    public class CloudinaryServices : ICloudinaryServices
    {
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _configuration;

        public CloudinaryServices(IServiceProvider provider, IConfiguration configuration)
        {
            _configuration=configuration;
            _cloudinary= new Cloudinary(new Account(_configuration["Cloudinary:Name"], _configuration["Cloudinary:APIKey"], _configuration["Cloudinary:APISecret"]));
            
        }

        private bool ValidateImage(IFormFile image)
        {
            var status = false;
            string[] listOfExtensions = { ".jpg", ".jpeg", ".png" };
            if (image == null) return status;
            for (int i = 0; i < listOfExtensions.Length; i++)
            {
                if (image.FileName.EndsWith(listOfExtensions[i]))
                {
                    status = true;
                    break;
                }
            }
            return status;
        }


        public async Task<UploadResult> UploadImage(IFormFile file)
        {
            var result = ValidateImage(file);
            var sa = _configuration["Cloudinary:Name"];
            var uploadResult = new ImageUploadResult();
            if (!result)
            {
                return default;
            }
            var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            using var imageStream = file.OpenReadStream();
            var parameters = new ImageUploadParams()
            {
                File = new FileDescription(fileName, imageStream),
                PublicId = fileName
            };
            uploadResult = await _cloudinary.UploadAsync(parameters);
            return uploadResult;
        }

        public async Task<UploadResult> UpdateByPublicId(IFormFile file, string publicId)
        {
            var uploadResult = new ImageUploadResult();
            await using var imageStream = file.OpenReadStream();
            var parameters = new ImageUploadParams()
            {
                File = new FileDescription(publicId, imageStream),
                PublicId = publicId,
                Overwrite = true,
                UniqueFilename = true
            };

            uploadResult = await _cloudinary.UploadAsync(parameters);
            return uploadResult;
        }

        public async Task<bool> DeleteByPublicId(string publicId)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId);
                await _cloudinary.DestroyAsync(deleteParams);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
