﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using Alfred.Constants;
using Alfred.Dal.Entities.Artifact;
using Alfred.Dal.Entities.Base;
using Alfred.Dal.Entities.Community;
using Alfred.Dal.Entities.Member;
using Alfred.Models.Artifacts;
using Alfred.Models.Base;
using Alfred.Models.Communities;
using Alfred.Models.Members;
using Alfred.Shared.Enums;
using Alfred.Shared.Features;

namespace Alfred.Dal.Mappers
{
    public class ModelFactory : IModelFactory
    {
        private readonly ObjectDifferenceManager _objDiffManager;
        private readonly UrlHelper _urlHelper;

        public ModelFactory(ObjectDifferenceManager objDiffManager, Func<HttpRequestMessage> getHttpRequestMessage)
        {
            _objDiffManager = objDiffManager;
            _urlHelper = new UrlHelper(getHttpRequestMessage());
        }

        public Member CreateMember(CreateMemberModel createMemberModel)
        {
            return new Member
            {
                Email = createMemberModel.Email,
                FirstName = createMemberModel.FirstName,
                LastName = createMemberModel.LastName,
                Role = createMemberModel.Role
            };
        }

        public MemberModel CreateMemberModel(Member member)
        {
            if (member != null)
            {
                return new MemberModel
                {
                    Id = member.Id,
                    Email = member.Email,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Role = member.Role
                };
            }
            return null;
        }

        public ArtifactModel CreateArtifactModel(Artifact artifact)
        {
            return new ArtifactModel
            {
                Id = artifact.Id,
                Bonus = artifact.Bonus,
                Reward = artifact.Reward,
                Status = artifact.Status,
                Title = artifact.Title,
                Type = artifact.Type,
                MemberId = artifact.MemberId,
                CommunityId = artifact.CommunityId
            };
        }

        public Artifact CreateArtifact(CreateArtifactModel createArtifactModel)
        {
            return new Artifact
            {
                Title = createArtifactModel.Title,
                Status = ArtifactStatus.ToDo,
                Type = createArtifactModel.Type,
                Reward = createArtifactModel.Reward,
                Bonus = createArtifactModel.Bonus,
                MemberId = createArtifactModel.MemberId,
                CommunityId = createArtifactModel.CommunityId
            };
        }

        public Artifact CreateArtifact(UpdateArtifactModel updateArtifactModel, Artifact oldArtifact)
        {
            var newArtifact = new Artifact
            {
                Id = updateArtifactModel.Id,
                Title = updateArtifactModel.Title,
                Bonus = updateArtifactModel.Bonus,
                Reward = updateArtifactModel.Reward,
                Status = updateArtifactModel.Status,
                Type = updateArtifactModel.Type,
                MemberId = updateArtifactModel.MemberId,
                CommunityId = updateArtifactModel.CommunityId
            };
            return _objDiffManager.UpdateObject(oldArtifact, newArtifact);
        }

        public ArtifactCriteria CreateArtifactCrtieria(ArtifactCriteriaModel criteriaModel)
        {
            return new ArtifactCriteria
            {
                Ids = criteriaModel.Ids?.Select(int.Parse),
                Title = criteriaModel.Title,
                Type = criteriaModel.Type,
                Status = criteriaModel.Status,
                CommunityId = criteriaModel.CommunityId,
                MemberId = criteriaModel.MemberId,
                Page = criteriaModel.Page,
                PageSize = criteriaModel.PageSize
            };
        }

        public Member CreateMember(UpdateMemberModel updateMemberModel, Member originalMember)
        {
            var newMember = new Member
            {
                Id = updateMemberModel.Id,
                Email = updateMemberModel.Email,
                FirstName = updateMemberModel.FirstName,
                LastName = updateMemberModel.LastName,
                Role = updateMemberModel.Role,
                CommunityIds = originalMember.CommunityIds
            };
            return _objDiffManager.UpdateObject(originalMember, newMember);
        }

        public CommunityModel CreateCommunityModel(Community community)
        {
            return new CommunityModel
            {
                Id = community.Id,
                Email = community.Email,
                Name = community.Name,
            };
        }

        public Community CreateCommunity(CreateCommunityModel createCommunityModel)
        {
            return new Community
            {
                Name = createCommunityModel.Name,
                Email = createCommunityModel.Email,
            };
        }

        public Community CreateCommunity(UpdateCommunityModel updateCommunityModel, Community originalCommunity)
        {
            var newCommunity = new Community
            {
                Id = updateCommunityModel.Id,
                Name = updateCommunityModel.Name,
                Email = updateCommunityModel.Email
            };

            return _objDiffManager.UpdateObject(originalCommunity, newCommunity);
        }

        public MemberCriteria CreateMemberCriteria(MemberCriteriaModel criteriaModel)
        {
            return new MemberCriteria
            {
                Ids = criteriaModel.Ids?.Select(int.Parse),
                CommunityId = criteriaModel.CommunityId,
                Email = criteriaModel.Email,
                Name = criteriaModel.Name,
                Role = criteriaModel.Role,
                Page = criteriaModel.Page,
                PageSize = criteriaModel.PageSize
            };
        }

        public CommunityCriteria CreateCommunityCriteria(CommunityCriteriaModel criteriaModel)
        {
            return new CommunityCriteria
            {
                Ids = criteriaModel.Ids?.Select(int.Parse),
                Name = criteriaModel.Name,
                Email = criteriaModel.Email,
                Page = criteriaModel.Page,
                PageSize = criteriaModel.PageSize
            };
        }

        public ArtifactResponseModel CreateArtifactResponseModel(ArtifactResponse artifactResponse)
        {
            return (ArtifactResponseModel)CreateResponseModel(artifactResponse, CreateArtifactModel);
        }

        public CommunityResponseModel CreateCommunityResponseModel(CommunityResponse communityResponse)
        {
            return (CommunityResponseModel)CreateResponseModel(communityResponse, CreateCommunityModel);
        }

        private Dictionary<string, object> ExtractQueryParams(HttpRequestMessage request)
        {
            var result = new Dictionary<string, object>();
            var queryParams = request.RequestUri.ParseQueryString();
            foreach (var key in queryParams.AllKeys)
            {
                result[key] = queryParams[key];
            }
            return result;
        }

        public LinkModel CreateLinkModel(Link link, Dictionary<string, object> queryParams)
        {
            queryParams["page"] = link.Href;
            return new LinkModel
            {
                Href = _urlHelper.Link(AlfredRoutes.GetArtifacts, queryParams),
                Rel = link.Rel
            };
        }

        private BaseResponseModel<TModel> CreateResponseModel<TEntity, TModel>(BaseResponse<TEntity> baseResponse,
            Func<TEntity, TModel> createModel)
        {
            var queryParams = ExtractQueryParams(_urlHelper.Request);
            return new BaseResponseModel<TModel>
            {
                Results = baseResponse.Results?.Select(createModel),
                Links = baseResponse.Links?.Select(l => CreateLinkModel(l, queryParams))
            };
        }
    }
}
