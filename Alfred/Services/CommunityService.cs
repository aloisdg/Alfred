using System.Collections.Generic;
using System.Linq;
using Alfred.Dal.Interfaces;
using Alfred.Model;
using Alfred.Model.Communities;

namespace Alfred.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly ICommunityRepository _communityRepo;
        private readonly IModelFactory _modelFactory;

        public CommunityService(ICommunityRepository communityRepo, IModelFactory modelFactory)
        {
            _communityRepo = communityRepo;
            _modelFactory = modelFactory;
        }

        public IEnumerable<CommunityModel> GetCommunities()
        {
            var communityEntities = _communityRepo.GetCommunities();
            return communityEntities.Select(x => _modelFactory.CreateCommunityModel(x));
        }

        public CommunityModel GetCommunity(int id)
        {
            var communityEntity = _communityRepo.GetCommunity(id);
            if (communityEntity != null)
            {
                return _modelFactory.CreateCommunityModel(communityEntity);
            }
            return null;
        }

        public int CreateCommunity(CreateCommunityModel createCommunityModel)
        {
            var community = _modelFactory.CreateCommunity(createCommunityModel);
            if (community != null && !IsEmailUsed(community.Email))
            {
                return _communityRepo.SaveCommunity(community);
            }
            return -1;
        }

        public CommunityModel UpdateCommunity(UpdateCommunityModel updateCommunityModel)
        {
            var community = _modelFactory.CreateCommunity(updateCommunityModel);
            if (community != null && IsEmailUsed(community.Email))
            {
                _communityRepo.UpdateCommunity(community);
                return _modelFactory.CreateCommunityModel(community);
            }
            return null;
        }

        public bool DeleteCommunity(int id)
        {
            var community = _communityRepo.GetCommunity(id);
            if (community != null)
            {
                _communityRepo.DeleteCommunity(id);
                return true;
            }
            return false;
        }

        private bool IsEmailUsed(string email)
        {
            return _communityRepo.GetCommunity(email) != null;
        }
    }
}