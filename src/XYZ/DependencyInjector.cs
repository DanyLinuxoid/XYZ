using Microsoft.Extensions.Options;

using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Logic;
using XYZ.DataAccess.Logic.Utility;
using XYZ.Logic.Billing.Factory;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.Billing;
using XYZ.Logic.Features.Billing.Paypal;
using XYZ.Logic.Features.Billing.Paysera;
using XYZ.Models.System.Configuration;

namespace XYZ.Web
{
    /// <summary>
    /// Dependency injection class.
    /// </summary>
    public static class DependencyInjector
    {
        /// <summary>
        /// Registers all classes and returns container.
        /// </summary>
        /// <returns>Container with registered classes</returns>
        public static void RegisterClasses(IServiceCollection services, IConfiguration config)
        {
            // Logic classes can be injected automatically (if all interfaces are stored in one place).
            Dictionary<Type, Type> interfaceToClassForInjection = GetAssemblyClasesForInjection(typeof(BillingLogic));
            InjectManual(services, config); // If we want to use special cases/singletons
            InjectAutomatic(interfaceToClassForInjection, services, config);
        }

        /// <summary>
        ///  Automatic injection, works only if all interfaces are stored in one place.
        /// </summary>
        /// <param name="injectedClases">Class to inject</param>
        /// <param name="services">Asp Collection</param>
        /// <param name="configuration">Asp Configuration</param>
        public static void InjectAutomatic(Dictionary<Type, Type> injectedClases, IServiceCollection services, IConfiguration configuration)
        {
            foreach (var classWithInterfacePair in injectedClases)
            {
                if (services.Any(x => x.ServiceType == classWithInterfacePair.Key) || classWithInterfacePair.Value.Namespace == null || classWithInterfacePair.Key.Namespace == null)
                    continue;

                // So we wouldn't have interface to interface registration
                if (classWithInterfacePair.Key.Namespace.Contains(".Interfaces") && classWithInterfacePair.Value.Namespace.Contains(".Interfaces"))
                    continue;

                services.AddTransient(classWithInterfacePair.Key, classWithInterfacePair.Value);
            }
        }

        /// <summary>
        /// Manual injection of services + Addional configuration.
        /// </summary>
        private static void InjectManual(IServiceCollection services, IConfiguration configuration)
        {
            // DB
            ConfigureDb(services, configuration);

            // Payment factory
            services.AddTransient<IPaymentGatewayLogic, PaypalGatewayLogic>();
            services.AddTransient<IPaymentGatewayLogic, PayseraGatewayLogic>();
            services.AddTransient<IBillingGatewayFactory, BillingGatewayFactory>();
        }

        /// <summary>
        /// Pure db related services configuration.
        /// </summary>
        /// <param name="services">Asp services</param>
        /// <param name="configuration">Asp collection</param>
        private static void ConfigureDb(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDatabaseLogic, DatabaseLogic>();
            services.AddSingleton<IDatabaseCommandExecutionLogic>(provider =>
            {
                var appConfig = provider.GetRequiredService<IOptions<AppConfiguration>>().Value;
                return new DatabaseCommandExecutionLogic(appConfig.ConnectionString);
            });
            services.AddSingleton<IDatabaseQueryExecutionLogic>(provider =>
            {
                var appConfig = provider.GetRequiredService<IOptions<AppConfiguration>>().Value;
                return new DatabaseQueryExecutionLogic(appConfig.ConnectionString);
            });
            services.AddSingleton<IDatabaseUtilityLogic>(provider =>
            {
                var appConfig = provider.GetRequiredService<IOptions<AppConfiguration>>().Value;
                return new DatabaseUtilityLogic(appConfig.ConnectionString);
            });
        }

        /// <summary>
        /// Automatically gets classes from assembly by provided assemblyType, for DI.
        /// </summary>
        /// <param name="assemblyType">Assembly type from where to take classes.</param>
        private static Dictionary<Type, Type> GetAssemblyClasesForInjection(Type assemblyType)
        {
            var logicClasses = assemblyType.Assembly.GetExportedTypes()
                .Where(type => type.Namespace != null &&
                    type.GetInterfaces().Length > 0 &&
                    !type.IsGenericType &&
                    !type.IsEnum &&
                    !GetTypesToIgnoreInAutomaticInjection().Contains(type));

            var injectionHolder = new Dictionary<Type, Type>();
            foreach (var logicClass in logicClasses)
                foreach (var logicInterface in logicClass.GetInterfaces())
                {
                    if (injectionHolder.ContainsKey(logicInterface))
                        continue;

                    injectionHolder.Add(logicInterface, logicClass);
                }

            return injectionHolder;
        }

        /// <summary>
        /// Gets types that should be ignored during automatic injection.
        /// </summary>
        /// <returns>List with types that should be ignored.</returns>
        private static List<Type> GetTypesToIgnoreInAutomaticInjection()
        {
            return new List<Type>()
            {
                typeof(BillingGatewayFactory),
                typeof(PaypalGatewayLogic),
                typeof(PayseraGatewayLogic),
            };
        }
    }
}
