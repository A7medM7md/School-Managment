using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.xUnitTest.Core.Tests.TestModels
{
    public class PassDataUsingMemberData
    {
        public static IEnumerable<object[]> GetStudentsIds()
        {
            return new List<object[]>
            { // Id: One Param
                new object[] { 1 },
                new object[] { 2 },
                new object[] { 3 },
                new object[] { 0 },
                new object[] { 100 },
                new object[] { -5 }
            };
        }
    }
}
