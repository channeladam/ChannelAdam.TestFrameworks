using System;
using SampleBizTalkMapHelpers.Abstractions;

namespace SampleBizTalkMapHelpers
{
    public class GuidHelper : IGuidHelper
    {
        public string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public string Echo(string value)
        {
            return value;
        }
    }
}
