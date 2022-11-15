using Demo_WebAPI_01.BLL.Interfaces;
using Demo_WebAPI_01.BLL.Models;
using Isopoh.Cryptography.Argon2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_WebAPI_01.BLL.Services
{
    public class MemberService: IMemberService
    {
        #region FakeDB
        private static List<Member> _Members = new List<Member>()
        {
            new Member() { 
                Id=1, 
                Email="daisy.duck@epicura.be", 
                Pseudo="Daisy", 
                HashPwd="$argon2id$v=19$m=65536,t=3,p=1$w2oAgHGMcXgXnBdHeACX3Q$1wbionGgGQ8509Ve5O6A4APighPd456mYl64OiT+Pbc",
                Role="User"
            },
            new Member() {
                Id=1,
                Email="donald.duck@epicura.be",
                Pseudo="Donald",
                HashPwd="$argon2id$v=19$m=65536,t=3,p=1$TcHcBA9TF6Ld8CEbGe/7IQ$kjJfQytTFpJni/aoXt8CMwdRy9dc8JgfvjmHW+Ly/WM",
                Role="Admin"
            }
        };
        private static int _LastId = 2;
        #endregion

        public Member? Login(string pseudo, string pwd)
        {
            Member? target = _Members.SingleOrDefault(m => string.Equals(m.Pseudo, pseudo, StringComparison.OrdinalIgnoreCase));

            if (target is null)
                return null;

            if (!Argon2.Verify(target.HashPwd, pwd))
                return null;

            return new Member()
            {
                Id = target.Id,
                Email = target.Email,
                Pseudo = target.Pseudo,
                Role = target.Role
            };
        }

        public Member? Register(Member memberData)
        {
            if (_Members.Any(m => string.Equals(m.Pseudo, memberData.Pseudo, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Le membre exist déjà");

            Member newMember = new Member()
            {
                Id = ++_LastId,
                Email = memberData.Email,
                Pseudo = memberData.Pseudo,
                HashPwd = Argon2.Hash(memberData.HashPwd),
                Role = memberData.Role
            };
            _Members.Add(newMember);

            return newMember;
        }
    }
}
