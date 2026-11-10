using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace CapaWeb.Helpers
{
    public class FileStorage : IFileStorage
    {
        private readonly Cloudinary _cloudinary;
        public FileStorage(IConfiguration configuration)
        {
            var account = new Account(
                configuration["CloudinarySettings:CloudName"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            string imageUrl = string.Empty;

            // Validar que el archivo no sea nulo ni esté vacío
            if (file == null || file.Length == 0)
            {
                return imageUrl;
            }

            // Usamos solo el Guid como PublicId (sin extensión)
            // var publicId = Guid.NewGuid().ToString();

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        // FileDescription acepta el nombre original, Cloudinary lo ignora para el guardado final si le pasas un PublicId
                        File = new FileDescription(file.FileName, stream)

                        // PublicId SIN la extensión
                        //PublicId = publicId,

                        // Opcional: Auto permite subir imágenes, pdfs, etc. sin cambiar el código
                        //ResourceType = ResourceType.Auto 
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        imageUrl = uploadResult.SecureUrl.ToString();
                    }
                }
            }
            catch (Exception)
            {
                // Log the exception (ex) if needed
                imageUrl = string.Empty;
            }

            return imageUrl;
        }
    }
}
