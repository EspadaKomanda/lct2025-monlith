using LctMonolith.Application.Options;
using LctMonolith.Database.UnitOfWork;
using LctMonolith.Services;
using LctMonolith.Services.Contracts;
using LctMonolith.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LctMonolith.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Core domain / gamification services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IStoreService, StoreService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IRankService, RankService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IMissionCategoryService, MissionCategoryService>();
        services.AddScoped<IMissionService, MissionService>();
        services.AddScoped<IRewardService, RewardService>();
        services.AddScoped<IRuleValidationService, RuleValidationService>();
        services.AddScoped<IProgressTrackingService, ProgressTrackingService>();
        services.AddScoped<IDialogueService, DialogueService>();
        services.AddScoped<IInventoryService, InventoryService>();

        services.Configure<S3StorageOptions>(configuration.GetSection("S3"));
        services.AddSingleton<IFileStorageService, S3FileStorageService>();
        services.AddScoped<IProfileService, ProfileService>();

        return services;
    }
}
