using System.Collections.Generic;

namespace EAudit.Models
{
    public class MenuBuilder
    {
        public string renderMenu(string role)
        {
            List<Modules> modules = this.GenerateMenus(role);
            string htmlOut = string.Empty;
            List<string> vs = new List<string>();
            List<Modules> newModules = modules.FindAll(a => a.menuRole == role);
            for (int idx = 0; idx < newModules.Count; idx++)
            {
                htmlOut = htmlOut + $"<li class='nav-item'>";
                htmlOut = htmlOut + $"<a href='{newModules[idx].menuLink}'><i class='nav-icon fas {newModules[idx].menuIcon}'></i><p>{newModules[idx].menuTitle}</p></a>";
                //if (newModules[idx].subModules != null && newModules[idx].subModules.Count > 0)
                //{

                //    vs.Add($"<ul class='nav nav-treeview'>");
                //    for (int idxC = 0; idxC < newModules[idx].subModules.Count; idxC++)
                //    {
                //        vs.Add($"<li class='nav-item'>");
                //        vs.Add($"<a href='{newModules[idx].subModules[idxC].menuLink}'><i class='far fa-circle nav-icon'></i><p>{newModules[idx].subModules[idxC].menuTitle}</p></a>");
                //        vs.Add($"</li>");
                //    }
                //    vs.Add($"</ul>");
                //}
                htmlOut = htmlOut + $"</li>";
            }
            // htmlOut = string.Join("", vs);
            return htmlOut;
        }
        public List<Modules> renderMenuList(string role)
        {
            List<Modules> modules = this.GenerateMenus(role);
            string htmlOut = string.Empty;
            List<string> vs = new List<string>();
            List<Modules> newModules = modules.FindAll(a => a.menuRole == role);
            return newModules;
        }
        private List<Modules> GenerateMenus(string role)
        {
            List<Modules> modules = new List<Modules>();
            List<SubModules> subModules_01 = new List<SubModules>();
            subModules_01.Add(new SubModules { menuTitle = "Auditor", menuIcon = "fa-user", menuLink = "/auditor/data" });
            subModules_01.Add(new SubModules { menuTitle = "Auditee", menuIcon = "fa-user", menuLink = "/auditee/data" });

            List<SubModules> subModules_Report = new List<SubModules>();
            subModules_Report.Add(new SubModules { menuTitle = "Temuan Audit", menuIcon = "fa-user-tie", menuLink = "/audit/temuan" });
            subModules_Report.Add(new SubModules { menuTitle = "Tanggapan Audit", menuIcon = "fa-user-tie", menuLink = "/audit/tanggapan" });

            modules.Add(new Modules { menuTitle = "Data User", menuIcon = "fa-users", menuLink = "javascript:void(0)", menuRole = "Admin", subModules = subModules_01 });
            modules.Add(new Modules { menuTitle = "Jadwal Penugasan", menuIcon = "fa-calendar", menuLink = "/audit/job_schedule", menuRole = "Admin", subModules = new List<SubModules>() });
            modules.Add(new Modules { menuTitle = "Standar SPMI", menuIcon = "fa-certificate", menuLink = "/configuration/lookup/standar_spmi", menuRole = "Admin", subModules = new List<SubModules>() });
            modules.Add(new Modules { menuTitle = "Laporan", menuIcon = "fa-file-alt", menuLink = "javascript:void(0)", menuRole = "Admin", subModules = subModules_Report });
            modules.Add(new Modules { menuTitle = "Log", menuIcon = "fa-file-signature", menuLink = "/configuration/log", menuRole = "Admin" });

            modules.Add(new Modules { menuTitle = "Jadwal Penugasan", menuIcon = "fa-calendar", menuLink = "/audit/job_schedule", menuRole = "Auditor", subModules = new List<SubModules>() });
            modules.Add(new Modules { menuTitle = "Temuan Audit", menuIcon = "fa-clipboard-list", menuLink = "/audit/temuan", menuRole = "Auditor", subModules = new List<SubModules>() });
            modules.Add(new Modules { menuTitle = "Daftar Tanggapan", menuIcon = "fa-clipboard-list", menuLink = "/audit/tanggapan", menuRole = "Auditor", subModules = new List<SubModules>() });
            modules.Add(new Modules { menuTitle = "Verifikasi Auditor", menuIcon = "fa-check-circle", menuLink = "/audit/verifikasi", menuRole = "Auditor", subModules = new List<SubModules>() });

            modules.Add(new Modules { menuTitle = "Temuan Audit", menuIcon = "fa-clipboard-list", menuLink = "/audit/temuan", menuRole = "Auditee", subModules = new List<SubModules>() });
            modules.Add(new Modules { menuTitle = "Tanggapan", menuIcon = "fa-reply", menuLink = "/audit/tanggapan", menuRole = "Auditee", subModules = new List<SubModules>() });
            modules.Add(new Modules { menuTitle = "Verifikasi Auditor", menuIcon = "fa-check-circle", menuLink = "/audit/verifikasi", menuRole = "Auditee", subModules = new List<SubModules>() });
            return modules;
        }
    }
    public class Modules
    {
        public string menuTitle { get; set; }
        public string menuDescription { get; set; }
        public string menuIcon { get; set; }
        public string menuRole { get; set; }
        public string menuLink { get; set; }
        public List<SubModules> subModules { get; set; }
    }
    public class SubModules
    {
        public string menuTitle { get; set; }
        public string menuDescription { get; set; }
        public string menuIcon { get; set; }
        public string menuRole { get; set; }
        public string menuLink { get; set; }
    }
}
