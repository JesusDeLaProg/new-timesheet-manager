using Google.Cloud.Firestore;
using new_timesheet_manager_server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var projectId = builder.Configuration["GCP_PROJECT_ID"] ?? "timesheet-manager-project";
builder.Services.AddSingleton(FirestoreDb.Create(projectId));
builder.Services.AddModels();

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));


var app = builder.Build();

var env = app.Environment;

if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
app.MapGrpcService<ActivityServiceImpl>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
