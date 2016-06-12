using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alfred.Dal.Entities.Artifact;
using Alfred.Dal.Interfaces;
using Alfred.Model;
using Alfred.Model.Artifacts;
using Alfred.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Alfred.Tests.Services
{
    [TestFixture]
    public class ArtifactServiceTests
    {
        private Fixture _fixture;
        private IModelFactory _modelFactory;
        private IArtifactRepository _artifactRepo;
        private ArtifactService _artifactService;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _modelFactory = Substitute.For<IModelFactory>();
            _artifactRepo = Substitute.For<IArtifactRepository>();
            _artifactService = new ArtifactService(_artifactRepo, _modelFactory);
        }

        [Test]
        public void Should_return_5_artifacts_when_repo_has_5_artifacts()
        {
            var artifacts = _fixture.Build<Artifact>()
                .Without(x => x.Member)
                .Without(x => x.Community)
                .CreateMany(5);
            _artifactRepo.GetArtifacts().Returns(artifacts);
            _modelFactory.CreateArtifactModel(Arg.Any<Artifact>()).Returns(GetArtifactModel(artifacts.FirstOrDefault()));

            var results = _artifactService.GetArtifacts();
            results.FirstOrDefault().Should().BeOfType<ArtifactModel>();
            results.Count().Should().Be(artifacts.Count());
        }

        [Test]
        public void Should_return_artifact_with_id_2_when_get_artifact_with_id_2()
        {
            var artifactWithIdTwo = _fixture.Build<Artifact>()
                .Without(x => x.Member)
                .Without(x => x.Community)
                .With(x => x.Id, 2)
                .Create();

            _artifactRepo.GetArtifact(Arg.Is(2)).Returns(artifactWithIdTwo);
            _modelFactory.CreateArtifactModel(Arg.Is<Artifact>(x => x.Id == 2)).Returns(GetArtifactModel(artifactWithIdTwo));

            var result = _artifactService.GetArtifact(2);
            result.Should().BeOfType<ArtifactModel>();
            result.Title.Should().Be(artifactWithIdTwo.Title);
            result.Status.Should().Be(artifactWithIdTwo.Status);
        }

        [Test]
        public void Should_return_null_when_no_artifact_with_id_2_exists()
        {
            _artifactRepo.GetArtifact(Arg.Is(2)).ReturnsNull();

            var result = _artifactService.GetArtifact(2);
            result.Should().BeNull();
        }

        private ArtifactModel GetArtifactModel(Artifact artifact)
        {
            return new ArtifactModel
            {
                Title = artifact.Title,
                Reward = artifact.Reward,
                Bonus = artifact.Bonus,
                Type = artifact.Type,
                Status = artifact.Status
            };
        }
    }
}
