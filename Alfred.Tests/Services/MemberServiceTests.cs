using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alfred.Dal.Entities.Artifact;
using Alfred.Dal.Entities.Community;
using Alfred.Dal.Entities.Member;
using Alfred.Dal.Interfaces;
using Alfred.Model;
using Alfred.Model.Members;
using Alfred.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Alfred.Tests.Services
{
    [TestFixture]
    public class MemberServiceTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Initialize()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void Should_return_5_members_when_service_get_all_members()
        {
            var members = _fixture.Build<Member>()
                .Without(x => x.Communities)
                .Without(x => x.Artifacts)
                .CreateMany(5).AsEnumerable();

            var fakeModelFactory = Substitute.For<IModelFactory>();
            var fakeRepo = Substitute.For<IMemberRepository>();
            fakeRepo.GetMembers().ReturnsForAnyArgs(members);

            var memberService = new MemberService(fakeRepo, fakeModelFactory);
            var result = memberService.GetMembers();
            //result.Should().BeOfType<IEnumerable<Member>>();
            result.Count().Should().Be(members.Count());
        }

        [Test]
        public void Should_return_member_2_when_service_gets_member_with_id_2()
        {
            var members = _fixture.Build<Member>()
                .Without(x => x.Communities)
                .Without(x => x.Artifacts)
                .CreateMany(4);

            var memberToSearch = _fixture.Build<Member>()
                .Without(x => x.Communities)
                .Without(x => x.Artifacts)
                .With(x => x.Id, 2)
                .Create();
            members.ToList().Add(memberToSearch);

            var fakeModelFactory = Substitute.For<IModelFactory>();
            var fakeRepo = Substitute.For<IMemberRepository>();
            fakeRepo.GetMember(Arg.Is(2)).Returns(memberToSearch);

            var memberService = new MemberService(fakeRepo, fakeModelFactory);
            var result = memberService.GetMember(2);
            result.Should().BeOfType<Member>();
            result.Id.Should().Be(2);
        }

        [Test]
        public void Should_return_null_when_service_dont_find_member_with_id_2()
        {
            var fakeRepo = Substitute.For<IMemberRepository>();
            fakeRepo.GetMember(Arg.Is(2)).ReturnsNull();

            var fakeModelFactory = Substitute.For<IModelFactory>();
            var memberService = new MemberService(fakeRepo, fakeModelFactory);
            var result = memberService.GetMember(2);
            result.Should().BeNull();
        }

        [Test]
        public void Should_create_member_when_member_has_valid_data()
        {
            var fakeRepo = Substitute.For<IMemberRepository>();
            var fakeModelFactory = Substitute.For<IModelFactory>();
            var createMemberModel = _fixture.Build<CreateMemberModel>().Create();
            var member = GetMember(createMemberModel);
            fakeModelFactory.CreateMember(Arg.Any<CreateMemberModel>()).Returns(member);
            fakeRepo.GetMember(Arg.Is<string>(x => x == createMemberModel.Email)).ReturnsNull();
            var memberService = new MemberService(fakeRepo, fakeModelFactory);

            memberService.CreateMember(createMemberModel);
            fakeModelFactory.Received(1).CreateMember(Arg.Is<CreateMemberModel>(x => x.Email == createMemberModel.Email));
            fakeModelFactory.Received(1).CreateMemberModel(Arg.Is<Member>(x => x.Email == createMemberModel.Email));
            fakeRepo.Received(1).GetMember(Arg.Is<string>(x => x == createMemberModel.Email));
            fakeRepo.Received(1).SaveMember(Arg.Is<Member>(x => x.Email == createMemberModel.Email));
        }

        [Test]
        public void Should_not_create_member_when_member_email_already_used()
        {
            var fakeRepo = Substitute.For<IMemberRepository>();
            var fakeModelFactory = Substitute.For<IModelFactory>();
            var createMemberModel = _fixture.Build<CreateMemberModel>().Create();
            var member = GetMember(createMemberModel);
            fakeModelFactory.CreateMember(Arg.Any<CreateMemberModel>()).Returns(member);
            fakeRepo.GetMember(Arg.Is<string>(x => x == createMemberModel.Email)).Returns(member);
            var memberService = new MemberService(fakeRepo, fakeModelFactory);

            memberService.CreateMember(createMemberModel);
            fakeModelFactory.Received(1).CreateMember(Arg.Is<CreateMemberModel>(x => x.Email == createMemberModel.Email));
            fakeModelFactory.DidNotReceive().CreateMemberModel(Arg.Is<Member>(x => x.Email == createMemberModel.Email));
            fakeRepo.Received(1).GetMember(Arg.Is<string>(x => x == createMemberModel.Email));
            fakeRepo.DidNotReceive().SaveMember(Arg.Is<Member>(x => x.Email == createMemberModel.Email));
        }

        private Member GetMember(CreateMemberModel createMemberModel)
        {
            return new Member
            {
                Email = createMemberModel.Email,
                FirstName = createMemberModel.FirstName,
                LastName = createMemberModel.LastName,
                Role = createMemberModel.Role,
                Communities = Enumerable.Empty<Community>(),
                Artifacts = Enumerable.Empty<Artifact>()
            };
        }
    }
}
