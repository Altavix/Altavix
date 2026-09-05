using Altavix.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Altavix.Persistence.Services;

public class ImageService : IImageService
{
    private readonly string _targetDir;

    public ImageService(IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(environment);

        var webRoot = environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot");
        _targetDir = Path.Combine(webRoot, "images", "products");

        Directory.CreateDirectory(_targetDir);
    }

    public async Task<string> SaveImageAsync(string base64Image, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(base64Image))
        {
            return string.Empty;
        }

        if (base64Image.StartsWith("/images/", StringComparison.OrdinalIgnoreCase) ||
            base64Image.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            base64Image.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return base64Image;
        }

        var trimmed = base64Image.Trim();

        if (trimmed.StartsWith("/images/", StringComparison.OrdinalIgnoreCase) ||
            trimmed.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            trimmed.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return trimmed;
        }

        var commaIndex = trimmed.IndexOf(',');
        var cleanBase64 = commaIndex >= 0 ? trimmed[(commaIndex + 1)..].Trim() : trimmed;

        byte[] bytes = Convert.FromBase64String(cleanBase64);

        using var memoryStream = new MemoryStream(bytes);
        using var image = await Image.LoadAsync(memoryStream, cancellationToken);

        if (image.Width > 1000 || image.Height > 1000)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(1000, 1000),
                Mode = ResizeMode.Max
            }));
        }

        Directory.CreateDirectory(_targetDir);

        var fileName = $"{Guid.NewGuid():N}.webp";
        var filePath = Path.Combine(_targetDir, fileName);

        await image.SaveAsWebpAsync(filePath, cancellationToken);

        return $"/images/products/{fileName}";
    }
}
