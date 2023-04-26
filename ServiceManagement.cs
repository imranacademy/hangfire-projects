namespace HangFireWebApi.Services
{
    public class ServiceManagement : IServiceManagement
    {
        public void GenerateMerchandise()
        {
            Console.WriteLine($"GenerateMerchandise {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public void SendEmail()
        {
            Console.WriteLine($"SendEmail {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public void SyncData()
        {
            Console.WriteLine($"SyncData {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public void UpdateDatabase()
        {
            Console.WriteLine($"UpdateDatabase {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }
    }
}
