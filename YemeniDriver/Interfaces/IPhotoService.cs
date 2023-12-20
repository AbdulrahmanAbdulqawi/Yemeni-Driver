using CloudinaryDotNet.Actions;

namespace YemeniDriver.Interfaces
{
    /// <summary>
    /// Represents a service for managing photos using Cloudinary.
    /// </summary>
    public interface IPhotoService
    {
        /// <summary>
        /// Adds a photo to Cloudinary asynchronously.
        /// </summary>
        /// <param name="file">The form file representing the photo to be uploaded.</param>
        /// <returns>An asynchronous operation that returns the result of the photo upload.</returns>
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        /// <summary>
        /// Deletes a photo from Cloudinary asynchronously.
        /// </summary>
        /// <param name="publicId">The public identifier of the photo in Cloudinary.</param>
        /// <returns>An asynchronous operation that returns the result of the photo deletion.</returns>
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
