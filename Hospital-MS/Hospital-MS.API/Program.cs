using Hospital_MS.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//
    //app.MapOpenApi();
//}

app.UseHttpsRedirection();

app.UseCors(); // Use default CORS policy

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
