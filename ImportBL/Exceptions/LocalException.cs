using System;
using System.Collections.Generic;
using System.Text;

namespace ImportBL.Exceptions
{
    public class LocalException : Exception
    {
        public string ClassInfo { get; }
        public string Details { get; }
        public string OriginalMessage { get; set; }

        public LocalException(string classInfo, string details)
        {
            ClassInfo = classInfo;
            Details = details;
        }

        public LocalException(string classInfo, string details, string originalMessage) : this(classInfo, details)
        {
            OriginalMessage = originalMessage;
        }
    }
}
