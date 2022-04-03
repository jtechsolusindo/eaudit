using EAudit.Models;
using System.Collections.Generic;

namespace EAudit.ViewModels
{
    public class GlobalVm
    {
        public List<Audit_JadwalKegiatan> jadwalList { get; set; }
        public Audit_JadwalKegiatan jadwalRow { get; set; }
        public List<EmployeeUnAssigned> employeeUnassigned { get; set; }
        public List<AuditorAuditee> auditorList { get; set; }
        public List<AuditorAuditee> auditeeList { get; set; }
        public List<LookUp_Standar_SPMI> spmiList { get; set; }
        public LookUp_Standar_SPMI spmi { get; set; }
        public AuditorAuditee auditorRow { get; set; }
        public AuditorAuditee auditeeRow { get; set; }
    }
}
