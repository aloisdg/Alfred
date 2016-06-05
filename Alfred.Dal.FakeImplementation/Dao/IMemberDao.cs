﻿using System.Collections.Generic;
using Alfred.Dal.Entities;

namespace Alfred.Dal.FakeImplementation.Dao
{
    public interface IMemberDao
    {
        IEnumerable<Member> GetMembers();
    }
}