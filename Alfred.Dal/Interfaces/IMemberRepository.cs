using System.Collections.Generic;
using Alfred.Dal.Entities;

namespace Alfred.Dal.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetMembers();
        Member GetMember(int id);
        void SaveMember(Member member);
        void DeleteMember(int id);
    }
}
