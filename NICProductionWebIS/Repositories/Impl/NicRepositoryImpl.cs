
namespace NICProductionWebIS.Repositories.Impl
{
    public class NicRepositoryImpl : NicRepository
    {
        public async Task<byte[]?> FromImage(IFormFile? photoFile)
        {
            if (photoFile == null || photoFile.Length <= 0) return null;
            using var ms = new MemoryStream();
            await photoFile.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
