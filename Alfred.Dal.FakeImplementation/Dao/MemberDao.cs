using System.Collections.Generic;
using Alfred.Dal.Entities;
using Ploeh.AutoFixture;

namespace Alfred.Dal.FakeImplementation.Dao
{
    public class MemberDao : IMemberDao
    {
        private readonly IEnumerable<Member> members;

        public MemberDao()
        {
            var fixture = new Fixture();
            members = fixture.Build<Member>()
                .Without(x => x.Communities)
                .Without(x => x.Artifacts)
                .CreateMany(5);
        }

        public IEnumerable<Member> GetMembers()
        {
            return members;
        }
    }
}
