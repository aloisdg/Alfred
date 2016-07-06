using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alfred.Dal.Interfaces;
using Alfred.Model;
using Alfred.Model.Members;

namespace Alfred.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IModelFactory _modelFactory;

        public MemberService(IMemberRepository memberRepository, IModelFactory modelFactory)
        {
            _memberRepository = memberRepository;
            _modelFactory = modelFactory;
        }
        public async Task<IEnumerable<MemberModel>> GetMembers()
        {
            var memberEntities = await _memberRepository.GetMembers();
            return memberEntities.Select(x => _modelFactory.CreateMemberModel(x));
        }

        public async Task<MemberModel> GetMember(int id)
        {
            var memberEntity = await _memberRepository.GetMember(id);
            return _modelFactory.CreateMemberModel(memberEntity);
        }

        public async Task<int> CreateMember(CreateMemberModel createMemberModel)
        {
            var member = _modelFactory.CreateMember(createMemberModel);
            if (member != null && !IsEmailUsed(member.Email))
            {
                return await _memberRepository.SaveMember(member);
            }
            return -1;
        }

        public async Task<bool> DeleteMember(int id)
        {
            var member = await _memberRepository.GetMember(id);
            if (member != null)
            {
                await Task.Run(() => _memberRepository.DeleteMember(id));
                return true;
            }
            return false;
        }

        public async Task<MemberModel> UpdateMember(UpdateMemberModel updateMemberModel)
        {
            var originalMember = await _memberRepository.GetMember(updateMemberModel.Id);
            if (originalMember != null)
            {
                var member = _modelFactory.CreateMember(updateMemberModel, originalMember);
                if (member != null)
                {
                    await Task.Run(() => _memberRepository.UpdateMember(member));
                    return _modelFactory.CreateMemberModel(member);
                }
            }
            return null;
        }

        private bool IsEmailUsed(string email)
        {
            return _memberRepository.GetMember(email) != null;
        }
    }
}