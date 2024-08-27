using UI.Ticket.WebUI.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcConfiguration(builder.Configuration);

var app = builder.Build();

app.UseMvcConfiguration();

app.Run();
