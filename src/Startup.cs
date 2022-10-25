/* NOT CURRENTLY USING THIS FILE
*/


using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using APIHomeworkBW.Controllers;

namespace APIHomeworkBW;

public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        //app.UseSwagger();
        //app.UseSwaggerUI();
        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
    }
}