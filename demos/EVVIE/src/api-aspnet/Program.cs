using System;

namespace EVVIE_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Set up builder
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls("http://0.0.0.0:5000");
            builder.Services.AddControllers();
            
            //Run the app
            var app = builder.Build();
            app.MapControllers();
            app.Run();
        }
    }
}