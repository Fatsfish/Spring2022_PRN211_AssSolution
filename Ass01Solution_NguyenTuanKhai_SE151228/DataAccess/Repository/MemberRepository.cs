using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MemberRepository:IMemberRepository
    {
        public MemberObject GetMemberByID(int id) => MemberDAO.Instance.GetMemberByID(id);
        public MemberObject GetMemberByName(string name) => MemberDAO.Instance.GetMemberByName(name);
        public IEnumerable<MemberObject> GetMembers() => MemberDAO.Instance.GetMemberList;
        public void InsertMember(MemberObject member) => MemberDAO.Instance.Add(member);
        public void DeleteMember(int id) => MemberDAO.Instance.Remove(id);
        public void UpdateMember(MemberObject member) => MemberDAO.Instance.Update(member);
        public List<MemberObject> GetMemberByCityAndCountry(string city, string country) => MemberDAO.Instance.GetMemberByCityAndCountry(city, country);

    }
}
