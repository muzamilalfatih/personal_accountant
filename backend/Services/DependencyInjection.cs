using personal_accountant.Services.Interfaces;

namespace personal_accountant.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IAccountServiceInterface,  AccountService>();
            services.AddScoped<ITransactionServiceInterface, TransactionService>();
            services.AddScoped<IUserServiceInterface, UserService>();
            services.AddScoped<IPasswordServiceInterface, PasswordService>();
            services.AddScoped<IAuthenticationServiceInterface, AuthenticationService>();
            services.AddScoped<IPasswordServiceInterface, PasswordService>();
            services.AddScoped<ITokenServiceInterface, TokenService>();
            services.AddScoped<IConfirmationTokenServiceInterface, ConfirmationTokenService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            return services;
        }
    }
}
