using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.xUnitTest.Core.Tests.TestModels
{
    public class PassDataUsingClassData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new()
        { // Id: One Param
            new object[] { 1 },
            new object[] { 2 },
            new object[] { 3 },
            new object[] { 0 },
            new object[] { 100 },
            new object[] { -5 }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
