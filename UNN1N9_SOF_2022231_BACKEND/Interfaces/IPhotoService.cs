using CloudinaryDotNet.Actions;

namespace UNN1N9_SOF_2022231_BACKEND.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string PublicId);
        string TransformImage(string imageUrl);
    }
}
