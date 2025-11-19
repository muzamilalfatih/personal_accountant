using personal_accountant.Repositories.Interfaces;

namespace personal_accountant.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepositoryInterface, UserRepository>();
            services.AddScoped<IAccountRepositoryInterface, AccountRepository>();
            services.AddScoped<ITransactionRepositoryInterface, TransactionRepository>();
            services.AddScoped<IConfirmationTokenRepositoryInterface, ConfirmationTokenRepository>();
            return services;
        }
    }
}
