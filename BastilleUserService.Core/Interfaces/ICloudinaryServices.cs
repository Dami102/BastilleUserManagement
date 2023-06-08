using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BastilleUserService.Core.Interfaces
{
    public interface ICloudinaryServices
    {
        Task<bool> DeleteByPublicId(string publicId);
        Task<UploadResult> UpdateByPublicId(IFormFile file, string publicId);
        Task<UploadResult> UploadImage(IFormFile file);
    }
}