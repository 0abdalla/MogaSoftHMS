using Hangfire;
using HealthChecks.UI.Client;
using Hospital_MS.API;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    DashboardTitle = "HMS Jobs Dashboard",
    //IsReadOnlyFunc = (DashboardContext context) => true
});

#region Configure Recurring Jobs - Update Appointment Status

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();
var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
var doctorService = scope.ServiceProvider.GetRequiredService<IDoctorService>();

jobManager.AddOrUpdate(
    "UpdateAppointmentStatus", 
    () => appointmentService.UpdateAppointmentsToCompletedAsync(), Cron.Daily
);


jobManager.AddOrUpdate(
    "ResetDoctorScheduleAppointments",
    () => doctorService.ResetCurrentAppointmentsAsync(),Cron.Daily  
);

#endregion


app.UseCors();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
