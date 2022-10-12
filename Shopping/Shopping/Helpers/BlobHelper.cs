using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Shopping.Models;

namespace Shopping.Helpers
{
	public class BlobHelper : IBlobHelper
	{
        public readonly CloudBlobClient _blobClient;
        public BlobHelper(IConfiguration configuration)
		{			
			string keys = configuration["Blob:ConnectionString"];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(keys);
            _blobClient = storageAccount.CreateCloudBlobClient();

        }
        public async Task DeleteBlobAsync(Guid id, string containerName)
		{
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);//Referenciamos el contenedor
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{id}");//Referenciamos el blob
            await blockBlob.DeleteAsync();//Eliminamos el blob

        }

        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
		{
            Stream stream = file.OpenReadStream();//stream es un arreglo en memoria del archivo
			return await UploadBlobAsync(stream, containerName);
			
        }

		public async Task<Guid> UploadBlobAsync(byte[] file, string containerName)
		{
            MemoryStream stream = new MemoryStream(file);
            return await UploadBlobAsync(stream, containerName);

        }

        public async Task<Guid> UploadBlobAsync(string image, string containerName)
		{
            Stream stream = File.OpenRead(image);
            return await UploadBlobAsync(stream, containerName);

        }

        
        private async Task<Guid> UploadBlobAsync(Stream stream, string containerName)
        {
            Guid name = Guid.NewGuid();//Se sube la imagen como un GUID- Código único
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);//Obtenemos el nombre del contenedor
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{name}");//Creamos el blob dentro del contenedor
            await blockBlob.UploadFromStreamAsync(stream);//Subimos la foto en el blob creado
            return name;
        }

       
    }
}
