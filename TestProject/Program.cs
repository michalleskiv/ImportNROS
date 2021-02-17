using System;
using ImportBL;
using ImportBL.Models;

namespace TestProject
{
    class Program
    {
        static void GetDataTest()
        {
            var dataReceiver = new TabidooDataReceiver("https://app.tabidoo.cloud/api/v2",
                "ac792ff0-384f-458e-8519-89b1a9fcd7aa",
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxY2Y4ZjEzNi04NDU0LTQxZjctYTBkYS1mY2VjZTkwMGEwM2UiLCJ1bmlxdWVfbmFtZSI6Imxlc2tpdm1pa2VAZ21haWwuY29tIiwicHVycG9zZSI6IkFQSVRva2VuIiwiYXBpVG9rZW5JZCI6IjIwMmU0NjVlLTRkZTktNDgyNi04YmQ1LTlkMGQwYWVmYjc3MCIsIm5iZiI6MTYxMjk2NTE2MCwiZXhwIjo0NzY4NjM4NzYwLCJpYXQiOjE2MTI5NjUxNjB9.VPHc0xgXi-NoLH5Sjq1U7Unm_-OsAe51w43USF21fg4");
            var contacts = dataReceiver.GetTable<Contact>("7f1a6cf4-86c5-4a1a-a479-2f04af786ba7").Result;

            foreach (var contact in contacts)
            {
                Console.WriteLine($"{contact.Email}, {contact.CisloUctu}");
            }

            Console.WriteLine();

            var subjects = dataReceiver.GetTable<Subject>("d343880c-f25b-4650-a9e9-d85407d2144a").Result;

            foreach (var subject in subjects)
            {
                Console.WriteLine($"{subject.Id}, {subject.Ico}");
            }

            Console.WriteLine();

            var gifts = dataReceiver.GetTable<Gift>("3043dcc9-010b-4111-8ead-27821670bbda").Result;

            foreach (var gift in gifts)
            {
                Console.WriteLine($"{gift.CisloUctu}, {gift.Castka}");
            }
        }

        static void ReadDataTest()
        {
            var fileReader = new FileReader();

            var gifts = fileReader.ReadGifts("Excel.xlsx");

            foreach (var gift in gifts)
            {
                Console.WriteLine($"{gift.Castka}, {gift.CisloUctu}, {gift.DatumDaru}");
            }
        }

        static void Main(string[] args)
        {
            ReadDataTest();
        }
    }
}
