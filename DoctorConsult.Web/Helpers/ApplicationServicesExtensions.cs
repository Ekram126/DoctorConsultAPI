


using ArticleConsult.Core.Repositories;
using BannerConsult.Core.Repositories;
using BannerConsult.Domain.Interfaces;
using DoctorConsult.Core.Repositories;
using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using Microsoft.AspNetCore.Identity;
using SectionConsult.Core.Repositories;
using SpecialistConsult.Core.Repositories;
using videoConsult.Core.Repositories;

namespace DoctorConsult.Web.Helpers
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ISpecialistRepository, SpecialistApi>();
            services.AddScoped<IArticleRepository, ArticleApi>();
            services.AddScoped<IBannerRepository, BannerApi>();
            services.AddScoped<IVideoRepository, VideoApi>();
            services.AddScoped<IDoctorRepository, DoctorApi>();
            services.AddScoped<IPatientRepository, PatientApi>();
            services.AddScoped<IRequestRepository, RequestApi>();
            services.AddScoped<IRequestTrackingRepository, RequestTrackingApi>();
            services.AddScoped<IRequestStatusRepository, RequestStatusApi>();
            services.AddScoped<IRequestDocumentRepository, RequestDocumentApi>();
            services.AddScoped<IContactUsRepository, ContactUsApi>();
            services.AddScoped<ICountryRepository, CountryApi>();
            services.AddScoped<IPersonalDataRepository, PersonalDataApi>();
            services.AddScoped<ISectionRepository, SectionApi>();
           // services.AddScoped<IPasswordValidator<ApplicationUser>, PasswordValidatorService>();
            return services;
        }
    }
}
