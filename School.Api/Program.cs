using Microsoft.Extensions.Options;
using School.Api;
using School.Core.Middlewares;

var builder = WebApplication.CreateBuilder(args);

#region DI

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAppDependencies(builder.Configuration);

#region Allow Cors

builder.Services.AddCors(options =>
{
    options.AddPolicy("_cors",
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

#endregion


#endregion

var app = builder.Build();

#region Middlewares

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


#region Request Localization Middleware

var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);

#endregion 



app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseCors("_cors");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
