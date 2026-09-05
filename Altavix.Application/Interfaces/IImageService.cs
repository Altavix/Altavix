namespace Altavix.Application.Interfaces;

public interface IImageService
{
    Task<string> SaveImageAsync(string base64Image, CancellationToken cancellationToken = default);
}
