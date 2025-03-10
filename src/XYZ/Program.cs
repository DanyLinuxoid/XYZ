using System.Text.Json.Serialization;

using XYZ.Web;
using XYZ.Web.Common.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // For swagger to display enum texts
    });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Billing API", Version = "v1" });
});

ServiceConfiguration.Configure(builder.Services, builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Billing API V1");
    });
}
else
    app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();
app.UseHttpsRedirection();
app.UseRouting();
app.Run();