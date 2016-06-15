using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alfred.Model.Artifacts;

namespace Alfred.Services
{
    public interface IArtifactService
    {
        IEnumerable<ArtifactModel> GetArtifacts();
        ArtifactModel GetArtifact(int id);
        ArtifactModel CreateArtifact(CreateArtifactModel createArtifactModel);
        ArtifactModel UpdateArtifact(UpdateArtifactModel updateArtifactModel);
        bool DeleteArtifact(int id);
    }
}
