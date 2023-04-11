using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using UNN1N9_SOF_2022231_BACKEND.Helpers;
using UNN1N9_SOF_2022231_BACKEND.Interfaces;

namespace UNN1N9_SOF_2022231_BACKEND.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(400).Width(400).Crop("fill").Gravity("face"),
                    Folder = "user-pics"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }
        
        public string TransformImage(string imageUrl)
        {
            Transformation transformation = new Transformation().Height(400).Width(400).Crop("fill").Gravity("face");
            ;
            ImageUploadResult uploadResult = _cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(imageUrl),
                Transformation = transformation,
                Folder = "user-pics"
            });
            ;
            //string transformedImageUrl = _cloudinary.Api.UrlImgUp.Transform(transformation).BuildUrl(imageUrl);
            string transformedImageUrl = _cloudinary.Api.UrlImgUp.Transform(transformation).BuildUrl(uploadResult.PublicId + "." + uploadResult.Format);

            ;
            return transformedImageUrl;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
