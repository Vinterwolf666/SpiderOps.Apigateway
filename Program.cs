using System.Net;
using System.Net.Http;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Forwarder;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .ConfigureHttpClient((context, handler) =>
    {
        handler.SslOptions.RemoteCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
    });


           
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            

            app.UseRouting();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapReverseProxy(); 
                endpoints.MapControllers();  
            });

            app.UseAuthorization(); 

            app.Run();
        }
    }
}
