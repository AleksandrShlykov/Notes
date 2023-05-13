using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using FluentValidation;
using Notes.Application.Common.Behaviors;

namespace Notes.Application
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddAplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddValidatorsFromAssemblies(new[] {Assembly.GetExecutingAssembly()});
            services.AddTransient(typeof (IPipelineBehavior<,>), typeof (ValidatorBehaviors<,>));
            return services;
        }
    }
}
