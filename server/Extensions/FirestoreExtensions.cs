using new_timesheet_manager_server.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FirestoreExtensions
    {
        public static IServiceCollection AddModels(this IServiceCollection services)
        {
            services.AddTransient<ActivityModel>();
            services.AddTransient<ClientModel>();
            services.AddTransient<EmployeeModel>();
            services.AddTransient<PhaseModel>();
            services.AddTransient<ProjectModel>();
            services.AddTransient<TimesheetModel>();
            services.AddTransient<UserModel>();

            return services;
        }
    }
}
