using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly string _imageDirectory = @"C:\Users\yamke\Downloads\Deepfashion\fashion-product-images-small\images";

    [HttpGet("{id}")]
    public IActionResult GetImage(string id)
    {
        var supportedExtensions = new[] { ".jpg", ".jpeg", ".png" };

        foreach (var ext in supportedExtensions)
        {
            var filePath = Path.Combine(_imageDirectory, $"{id}{ext}");
            if (System.IO.File.Exists(filePath))
            {
                var contentType = ext switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    _ => "application/octet-stream"
                };
                var bytes = System.IO.File.ReadAllBytes(filePath);
                return File(bytes, contentType);
            }
        }

        return NotFound();
    }
}

