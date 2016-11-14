using System.Collections.Generic;
using System.Threading.Tasks;
using Alfred.Model.Artifacts;

namespace Alfred.Services
{
    public interface IArtifactService
    {
        Task<IEnumerable<ArtifactModel>> GetArtifacts();
        Task<ArtifactModel> GetArtifact(int id);
        Task<int> CreateArtifact(CreateArtifactModel createArtifactModel);
        Task<ArtifactModel> UpdateArtifact(UpdateArtifactModel updateArtifactModel);
        Task<bool> DeleteArtifact(int id);
    }
}
