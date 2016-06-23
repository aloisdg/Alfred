using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alfred.Dal.Entities.Artifact;
using Alfred.Dal.FakeImplementation.Dao;
using Alfred.Dal.FakeImplementation.EntityDtos;
using Alfred.Dal.Interfaces;

namespace Alfred.Dal.FakeImplementation.Repositories
{
    public class ArtifactRepository : IArtifactRepository
    {
        private readonly IArtifactDao _artifactDao;

        public ArtifactRepository(IArtifactDao artifactDao)
        {
            _artifactDao = artifactDao;
        }

        public async Task<IEnumerable<Artifact>> GetArtifacts()
        {
            var artifactDtos = await Task.FromResult(_artifactDao.GetArtifacts()).ConfigureAwait(false);
            return artifactDtos.Select(TransformToArtifactEntity);
        }

        private static Artifact TransformToArtifactEntity(ArtifactDto artifactDto)
        {
            if (artifactDto == null) return null;

            return new Artifact
            {
                Id = artifactDto.Id,
                Title = artifactDto.Title,
                Bonus = artifactDto.Bonus,
                Reward = artifactDto.Reward,
                Status = (ArtifactStatus)artifactDto.Status,
                Type = (ArtifactType)artifactDto.Type
            };
        }

        public async Task<Artifact> GetArtifact(int id)
        {
            return TransformToArtifactEntity(await Task.FromResult(_artifactDao.GetArtifact(id)).ConfigureAwait(false));
        }

        public async Task<int> SaveArtifact(Artifact artifact)
        {
            var artifactDto = TransformToArtifactDto(artifact);
            return await Task.FromResult(_artifactDao.SaveArtifact(artifactDto));
        }

        private ArtifactDto TransformToArtifactDto(Artifact artifact)
        {
            if (artifact != null)
            {
                return new ArtifactDto
                {
                    Id = artifact.Id,
                    Title = artifact.Title,
                    Bonus = artifact.Bonus,
                    Reward = artifact.Reward,
                    Status = (int) artifact.Status,
                    Type = (int) artifact.Type
                };
            }
            return null;
        }

        public void DeleteArtifact(int id)
        {
            _artifactDao.DeleteArtifact(id);
        }

        public void UpdateArtifact(Artifact artifact)
        {
            var artifactDto = TransformToArtifactDto(artifact);
            _artifactDao.UpdateArtifact(artifactDto);
        }

        public IEnumerable<Artifact> GetMemberArtifacts(int id)
        {
            var artifactDtos = _artifactDao.GetMemberArtifacts(id);
            return artifactDtos.Select(TransformToArtifactEntity);
        }

        public IEnumerable<Artifact> GetCommunityArtifacts(int id)
        {
            return _artifactDao.GetCommunityArtifacts(id).Select(TransformToArtifactEntity);
        }
    }
}
