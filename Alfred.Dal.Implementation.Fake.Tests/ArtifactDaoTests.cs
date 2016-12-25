﻿using System.Collections.Generic;
using System.Linq;
using Alfred.Dal.Entities.Artifacts;
using Alfred.Dal.Implementation.Fake.Dao;
using Alfred.Dal.Implementation.Fake.Mappers;
using Alfred.Shared.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Alfred.Dal.Implementation.Fake.Tests
{
    [TestFixture]
    public class ArtifactDaoTests
    {
        private ArtifactDao _artifactDao;
        private EntityFactory _entityFactory;

        [SetUp]
        public void Setup()
        {
            _entityFactory = new EntityFactory();
            _artifactDao = new ArtifactDao(_entityFactory);
        }

        [Test]
        public void Should_return_artifacts_when_criteria_has_ids_32_or_4()
        {
            var criteria = new ArtifactCriteria
            {
                Ids = new List<int> {32,4},
                PageSize = 20
            };
            var results = _artifactDao.GetArtifacts(criteria).Result;
            results.Should().OnlyContain(e => criteria.Ids.Contains(e.Id));
        }

        [Test]
        public void Should_not_return_artifacts_when_criteria_has_ids_999()
        {
            var criteria = new ArtifactCriteria
            {
                Ids = new List<int> { 999 },
                PageSize = 20
            };
            var results = _artifactDao.GetArtifacts(criteria).Result;
            results.Should().BeEmpty();
        }

        [Test]
        public void Should_not_return_artifacts_when_criteria_has_title_blabla()
        {
            var criteria = new ArtifactCriteria
            {
                Title = "blabla",
                PageSize = 20
            };
            var results = _artifactDao.GetArtifacts(criteria).Result;
            results.Should().BeEmpty();
        }

        [Test]
        public void Should_not_return_artifacts_when_criteria_has_title_cleanCode()
        {
            var criteria = new ArtifactCriteria
            {
                Title = "Clean Code",
                PageSize = 20
            };
            var results = _artifactDao.GetArtifacts(criteria).Result;
            results.Should().OnlyContain(res=>res.Title.Contains(criteria.Title));
        }

        [Test]
        public void Should_not_return_artifacts_when_criteria_has_type_unknown()
        {
            var criteria = new ArtifactCriteria
            {
                Type = (ArtifactType)10,
                PageSize = 20
            };
            var results = _artifactDao.GetArtifacts(criteria).Result;
            results.Should().BeEmpty();
        }

        [Test]
        public void Should_return_artifacts_when_criteria_has_type_BrownBagLaunch()
        {
            var criteria = new ArtifactCriteria
            {
                Type = ArtifactType.BrownBagLaunch,
                PageSize = 20
            };
            var results = _artifactDao.GetArtifacts(criteria).Result;
            results.Should().OnlyContain(x=>x.Type == criteria.Type);
        }

        [Test]
        public void Should_not_return_artifacts_when_criteria_has_status_unknown()
        {
            var criteria = new ArtifactCriteria
            {
                Status = (ArtifactStatus)10,
                PageSize = 20
            };
            var results = _artifactDao.GetArtifacts(criteria).Result;
            results.Should().BeEmpty();
        }

        [Test]
        public void Should_return_artifacts_when_criteria_has_status_inprogress()
        {
            var criteria = new ArtifactCriteria
            {
                Status = ArtifactStatus.InProgress,
                PageSize = 20
            };
            var results = _artifactDao.GetArtifacts(criteria).Result;
            results.Should().OnlyContain(x => x.Status == criteria.Status);
        }

        [Test]
        public void Should_not_return_artifacts_when_criteria_has_memberId_465()
        {
            var criteria = new ArtifactCriteria
            {
                MemberId = 465,
                PageSize = 20
            };
            var results = _artifactDao.GetArtifacts(criteria).Result;
            results.Should().BeEmpty();
        }

        [Test]
        public void Should_return_artifacts_when_criteria_has_memberId_2()
        {
            var criteria = new ArtifactCriteria
            {
                MemberId = 2,
                PageSize = 20
            };
            var results = _artifactDao.GetArtifacts(criteria).Result;
            results.Should().OnlyContain(x => x.MemberId == criteria.MemberId);
        }

        [Test]
        public void Should_not_return_artifacts_when_criteria_has_CommunityId_465()
        {
            var criteria = new ArtifactCriteria
            {
                CommunityId = 465,
                PageSize = 20
            };
            var results = _artifactDao.GetArtifacts(criteria).Result;
            results.Should().BeEmpty();
        }

        [Test]
        public void Should_return_artifacts_when_criteria_has_communityId_2()
        {
            var criteria = new ArtifactCriteria
            {
                CommunityId = 2,
                PageSize = 20
            };
            var results = _artifactDao.GetArtifacts(criteria).Result;
            results.Should().OnlyContain(x => x.CommunityId == criteria.CommunityId);
        }
    }
}
