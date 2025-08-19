using GymManagement.Application;
using GymManagement.Infrastructure;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddApplication()
    .AddInfrastructure();

WebApplication app = builder.Build();
{
    app.UseExceptionHandler();
    app.AddInfrastructureMiddleware();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    using (IServiceScope scope = app.Services.CreateScope())
    {
        GymDbContext dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    app.Run();
}
