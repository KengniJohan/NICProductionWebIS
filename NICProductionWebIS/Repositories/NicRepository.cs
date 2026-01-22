namespace NICProductionWebIS.Repositories
{
    public interface NicRepository
    {
        Task<byte[]?> FromImage(IFormFile? photoFile);

    }
}
