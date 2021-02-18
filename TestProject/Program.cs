using ImportBL;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            MainWorker worker = new MainWorker();
            worker.Run("C:\\Users\\Mike\\Desktop\\Excel.xlsx").Wait();
        }
    }
}
