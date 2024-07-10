using BadmintonReservationBusiness;
using BadmintonReservationData;
using GrpcDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddGrpc();
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<CustomerBusiness>();
builder.Services.AddScoped<CourtBusiness>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<CourtService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
