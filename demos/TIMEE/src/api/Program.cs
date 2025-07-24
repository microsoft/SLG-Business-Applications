using System;

namespace TIMEEAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Set up builder
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls("http://0.0.0.0:6302");
            builder.Services.AddControllers();

            //Add datastore as singleton
            ChatDB cdb = new ChatDB();
            builder.Services.AddSingleton(cdb);

            //Run the app
            var app = builder.Build();
            app.MapControllers();
            app.Run();
        }
    }
}