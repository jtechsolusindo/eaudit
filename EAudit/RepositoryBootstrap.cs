using EAudit.DAO.Authentication;
using EAudit.DAO.Person;
using EAudit.DAO.LookUp;
using Microsoft.Extensions.DependencyInjection;
using EAudit.DAO;
using EAudit.DAO.AuditorDao;
using EAudit.DAO.AuditeeDao;
using EAudit.DAO.StandarSpmiDao;
using EAudit.DAO.StandarSpmi;
using EAudit.DAO.PegawaiDao;
using E_AuditInternal.Services;
using E_AuditInternal.Models;
using System.Configuration;

namespace EAudit
{
    public static class RepositoryBootstrap
    {
        public static void AppService(this IServiceCollection services)
        {
            // services.AddTransient<DAO.Interfaces.Test.IEmployeeService, DAO.Interfaces.Test.EmployeeService>(); // hanya untuk test, nanti dihapus aja
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IAuthInterface, AuthRepository>();
            services.AddTransient<IEAuditInterface, EAuditRepository>();
            services.AddTransient<IPerson, PersonRepository>();
            services.AddTransient<ILookUp, LookUpRepository>();
            services.AddTransient<IAuditor, AuditorRepository>();
            services.AddTransient<IAuditee, AuditeeRepository>();
            services.AddTransient<IStandarSpmi, StandarSpmiRepository>();
            services.AddTransient<IPegawai, PegawaiRepository>();
            services.AddTransient<AuditInterface, AuditRepository>();

        }
    }
}