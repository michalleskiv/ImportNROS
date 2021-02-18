using System;
using System.Collections.Generic;

namespace ImportBL.Exceptions
{
    public class LocalException : Exception
    {
        public List<string> ErrorList { get; set; } = new List<string>();

        public LocalException(string originalMessage)
        {
            ErrorList.Add(originalMessage);
        }

        public LocalException(List<string> errorList)
        {
            ErrorList = errorList;
        }
    }
}
