using E_AuditInternal.Services;
using EAudit.Controllers.Modules;
using EAudit.DAO;
using EAudit.DAO.AuditeeDao;
using EAudit.DAO.AuditorDao;
using EAudit.DAO.PegawaiDao;
using EAudit.DAO.StandarSpmiDao;
using EAudit.Helpers;
using EAudit.Models;
using EAudit.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EAudit.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController: ControllerBase
    {
        private IAuditor _auditorRepository;
        private IAuditee _auditeeRepository;
        private IStandarSpmi _standarSpmiRepository;
        private IPegawai _pegawaiRepository;
        private AuditInterface _auditRepository;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IMailService mailService;
        private static bool is_debug = true;
        //private static string email_debug = "muzakisyahrul100@gmail.com";
        private static string email_debug = "lawrencenoman@gmail.com";
        public ApiController(IWebHostEnvironment appEnvironment, IMailService mailService,
            AuditInterface auditRepository,IAuditor auditorRepository, IAuditee auditeeRepository,
            IStandarSpmi standarSpmiRepository, IPegawai pegawaiRepository)
        {
            _appEnvironment = appEnvironment;
            this.mailService = mailService;
            _auditRepository = auditRepository;
            _auditorRepository = auditorRepository;
            _auditeeRepository = auditeeRepository;
            _standarSpmiRepository = standarSpmiRepository;
            _pegawaiRepository = pegawaiRepository;
        }

        // AUDITOR
        [Route("auditor/list")]
        public async Task<IActionResult> AuditorList()
        {
            var search = "";
            if(Request.ContentType != null)
            {
                var req = Request.Form;
                if (req.ContainsKey("inputSearch")) search = req["inputSearch"];
            }
            List<Auditor> list_data = await _auditorRepository.getAuditorList(search);
            var json = System.Text.Json.JsonSerializer.Serialize(list_data);
            return Ok(json);
        }

        [Route("auditor/save")]
        public IActionResult AuditorSave()
        {
            AjaxResponse response = new AjaxResponse();
            try
            {
                var req = Request.Form;
                Auditor data = new Auditor();
                if (req.ContainsKey("txtID"))
                {
                    if(!string.IsNullOrEmpty(req["txtID"])) data.ID = int.Parse(req["txtID"]);
                }
                if (req.ContainsKey("txtNPP")) data.NPP = req["txtNPP"].ToString();
                if (req.ContainsKey("txtKode")) data.KODE = req["txtKode"].ToString();
                if (req.ContainsKey("txtProgramStudi")) data.PRODI = req["txtProgramStudi"].ToString();
                if (req.ContainsKey("txtEmail")) data.EMAIL = req["txtEmail"].ToString();
                _auditorRepository.AuditorSave(data);
                response.result = "ok";
                response.message = "Auditor Berhasil Disimpan.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }

            return Ok(response);
        }


        [Route("auditor/detail/{id}")]
        public async Task<IActionResult> AuditorRow(string id)
        {
            Auditor filter = new Auditor();
            if (!string.IsNullOrEmpty(id))
            {
                filter.ID = int.Parse(id);
            }
            Auditor data = await _auditorRepository.getAuditorRow(filter);
            var json = System.Text.Json.JsonSerializer.Serialize(data);
            return Ok(json);
        }

        [HttpPost]
        [Route("auditor/delete")]
        public IActionResult AuditorDelete([FromBody] Auditor data)
        {
            AjaxResponse response = new AjaxResponse();
            try
            {
                _auditorRepository.AuditorDelete(data);
                response.result = "ok";
                response.message = "Auditor Berhasil Dihapus.";
            }
            catch (Exception e)
            {
                response.result = "ok";
                response.message = e.Message;
            }

            return Ok(response);
        }

        //AUDITEE
        [Route("auditee/list")]
        public async Task<IActionResult> AuditeeList()
        {
            List<Auditee> list_data = await _auditeeRepository.getAuditeeList("");
            var json = System.Text.Json.JsonSerializer.Serialize(list_data);
            return Ok(json);
        }
        

        // STANDAR SPMI
        [Route("standar_spmi/list")]
        public async Task<IActionResult> StandarSpmilist()
        {
            List<StandarSpmi> list_data = await _standarSpmiRepository.getStandarList("");
            var json = System.Text.Json.JsonSerializer.Serialize(list_data);
            return Ok(json);
        }

        [Route("standar_spmi/detail/{id}")]
        public async Task<IActionResult> StandarSpmiRow(string id)
        {
            StandarSpmi filter = new StandarSpmi();
            if (!string.IsNullOrEmpty(id))
            {
                filter.ID = int.Parse(id);
            }
            StandarSpmi data = await _standarSpmiRepository.getStandarRow(filter);
            var json = System.Text.Json.JsonSerializer.Serialize(data);
            return Ok(json);
        }

        //UNSUR MANAJEMEN
        [Route("unsur_manajemen/list")]
        public async Task<IActionResult> UnsurManajemenList()
        {
            var search = "";
            List<UnsurManajemen> result = await _auditRepository.UnsurManajemenList(search);
            var json = System.Text.Json.JsonSerializer.Serialize(result);
            return Ok(json);
        }


        //PEGAWAI
        [Route("pegawai_unassigned_opsi")]
        public async Task<IActionResult> PegawaiUnassigned()
        {
            List<Pegawai> list_data = await _pegawaiRepository.getPegawaiUnassigned("");
            List<dynamic> list_data_fix= new List<dynamic>();

            for (int i = 0; i < list_data.Count; i++)
            {
                SelectOption dataTemp = new SelectOption();
                dataTemp.id = list_data[i].NPP;
                dataTemp.text = list_data[i].NAMA;
                list_data_fix.Add(dataTemp);
            }
            var json = System.Text.Json.JsonSerializer.Serialize(list_data_fix);
            return Ok(json);
        }


        //AUDIT
        //TEMUAN AUDIT
        [Route("audit/temuan/list")]
        public async Task<IActionResult> TemuanAuditList()
        {
            var search = "";
            if (Request.ContentType != null)
            {
                var req = Request.Form;
                if (req.ContainsKey("inputSearch")) search = req["inputSearch"];
            }
            UserLoggedIn _userLoggedIn = CommonHelper.userLoggedIn((ClaimsIdentity)User.Identity);
            string id_auditor = _userLoggedIn.id_auditor;
            string id_auditee = _userLoggedIn.id_auditee;
            string role = _userLoggedIn.role;
            List<AuditTemuan> result = await _auditRepository.getTemuanAuditList(search, role, id_auditor,id_auditee);
            var json = System.Text.Json.JsonSerializer.Serialize(result);
            return Ok(json);
        }

        [Route("audit/temuan/detail/{id}")]
        public async Task<IActionResult> TemuanAuditRow(string id)
        {
            AuditTemuan filter = new AuditTemuan();
            if (!string.IsNullOrEmpty(id))
            {
                filter.ID_TEMUAN = int.Parse(id);
            }
            AuditTemuan data = await _auditRepository.TemuanAuditRow(filter);
            var json = System.Text.Json.JsonSerializer.Serialize(data);
            return Ok(json);
        }

        [HttpPost]
        [Route("audit/temuan/save")]
        public IActionResult TemuanAuditSave()
        {
            var req = Request.Form;
            AjaxResponse response = new AjaxResponse();
            try
            {
                _auditRepository.temuanSave(
                    req["id_edit"],
                    req["id_auditor"],
                    req["id_auditee"],
                    req["jenis"],
                    req["uraian"],
                    req["no_standar"]
                );
                response.result = "ok";
                response.message = "Temuan Audit Berhasil Disimpan.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("audit/temuan/delete")]
        public IActionResult TemuanAuditDelete()
        {
            var req = Request.Form;
            AjaxResponse response = new AjaxResponse();
            try
            {
                _auditRepository.temuanDelete(
                    req["id"]
                );
                response.result = "ok";
                response.message = "Temuan Audit Berhasil Dihapus.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("audit/temuan/kirim")]
        public IActionResult TemuanAuditKirim()
        {
            var req = Request.Form;
            AjaxResponse response = new AjaxResponse();
            try
            {
                _auditRepository.temuanKirim(
                    req["id"]
                );
                response.result = "ok";
                response.message = "Temuan Audit Berhasil Dikirim.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }
            return Ok(response);
        }

        
        //Tanggapan Audit
        [HttpPost]
        [Route("audit/tanggapan/save")]
        public async Task<IActionResult> TanggapanAuditSave()
        {
            var req = Request.Form;
            string filePath = "";
            string fileNameUnik = "";
            string mimeType = "";
            string uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "file_audit");
            
            for (int i = 0; i < req.Files.Count; i++)
            {
                IFormFile file = req.Files[i]; //Uploaded file
                long fileSize = file.Length;
                string fileName = file.FileName;
                fileNameUnik = req["nama_file"] +" "+$@"{DateTime.Now.Ticks}."+ req["tipe_file"];
                mimeType = file.ContentType;
                if (fileSize > 0)
                   {
                    filePath = Path.Combine(uploadsFolder, fileNameUnik);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            //var response = new { result = fileNameUnik };
            AjaxResponse response = new AjaxResponse();
            try
            {
                _auditRepository.TanggapanSave(
                    req["id_edit"],
                    req["jenis"],
                    req["akar_masalah"],
                    req["analisis"],
                    req["koreksi"],
                    req["korektif"],
                    req["tipe_file"],
                    fileNameUnik,
                    req["link"],
                    req["nama_file"],
                    req["id_temuan"]
                );
                response.result = "ok";
                response.message = "Tanggapan Berhasil Disimpan.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }
            return Ok(response);
        }

        [Route("audit/tanggapan/list")]
        public async Task<IActionResult> TanggapanAuditList()
        {
            var search = "";
            if (Request.ContentType != null)
            {
                var req = Request.Form;
                if (req.ContainsKey("inputSearch")) search = req["inputSearch"];
            }
            UserLoggedIn _userLoggedIn = CommonHelper.userLoggedIn((ClaimsIdentity)User.Identity);
            string id_auditor = _userLoggedIn.id_auditor;
            string id_auditee = _userLoggedIn.id_auditee;
            string role = _userLoggedIn.role;
            List<TanggapanAudit> result = await _auditRepository.TanggapanList(search, role, id_auditor, id_auditee);
            var json = System.Text.Json.JsonSerializer.Serialize(result);
            return Ok(json);
        }


        [Route("audit/tanggapan/detail/{id}")]
        public async Task<IActionResult> TanggapanAuditRow(string id)
        {
            TanggapanAudit filter = new TanggapanAudit();
            if (!string.IsNullOrEmpty(id))
            {
                filter.ID_TANGGAPAN = int.Parse(id);
            }
            TanggapanAudit data = await _auditRepository.TanggapanAuditRow(filter);
            var json = System.Text.Json.JsonSerializer.Serialize(data);
            return Ok(json);
        }

        [HttpPost]
        [Route("audit/tanggapan/delete")]
        public IActionResult TanggapanAuditDelete()
        {
            var req = Request.Form;
            AjaxResponse response = new AjaxResponse();
            try
            {
                _auditRepository.TanggapanDelete(
                    req["id"]
                );
                response.result = "ok";
                response.message = "Tanggapan Audit Berhasil Dihapus.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }
            return Ok(response);
        }

        [Route("audit/tanggapan/kirim")]
        public async Task<IActionResult> TanggapanAuditKirim()
        {
            var req = Request.Form;
            AjaxResponse response = new AjaxResponse();
            try
            {
                TanggapanAudit data = await _auditRepository.TanggapanKirim(req["id"]);
                MailRequest request = new MailRequest();
                request.ToEmail = is_debug? email_debug: data.EMAIL;
                var xData = data;
                    request.Subject = "Tanggapan Temuan Baru";
                    request.Body = String.Format(@"Halo, {0}<br/>Email ini adalah pemberitahuan bahwa ada Tanggapan dari temuan Anda:
                    <br/>
                    <table width='100%'>
                    <tr>
                    <td>Akar permasalahan</td>
                    <td valign='top'>: {1}</td>
                    </tr>
                    <tr>
                    <td>Koreksi</td>
                    <td valign='top'>: {2}</td>
                    </tr>
                    <tr>
                    <td>Korektif</td>
                    <td valign='top'>: {3}</td>
                    </tr>
                    <tr>
                    <td>Tanggal ditanggapi</td>
                    <td valign='top'>: {4}</td>
                    </tr>
                    </table>", xData.NAMA, xData.ANALISIS, xData.KOREKSI, xData.KOREKTIF, xData.TANGGALTANGGAPAN);
                    await mailService.SendEmailAsync(request);
                    response.result = "ok";
                    response.message = "Tanggapan Audit Berhasil Dikirim.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("audit/tanggapan/verifikasi")]
        public IActionResult TanggapanAuditVerifikasi()
        {
            var req = Request.Form;            
            AjaxResponse response = new AjaxResponse();
            try
            {
                 _auditRepository.TanggapanVerifikasi(
                    req["id_tanggapan"],
                    req["konfirmasi"],
                    req["catatan"],
                    req["uraian"]
                );
                response.result = "ok";
                response.message = "Tanggapan Berhasil Diverifikasi.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("audit/tanggapan/selesaikan")]
        public IActionResult TanggapanAuditSelesaikan()
        {
            var req = Request.Form;
            AjaxResponse response = new AjaxResponse();
            try
            {
                _auditRepository.TanggapanSelesaikan(
                    req["id"]
                );
                response.result = "ok";
                response.message = "Tanggapan Audit Berhasil Diselesaikan.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }
            return Ok(response);
        }

        // VERIFIKASI AUDITOR
        // Menampilkan Riwayat Verifikasi
        [Route("audit/verifikasi/list")]
        public async Task<IActionResult> VerifikasiAuditList()
        {
            var search = "";
            if (Request.ContentType != null)
            {
                var req = Request.Form;
                if (req.ContainsKey("inputSearch")) search = req["inputSearch"];
            }
            UserLoggedIn _userLoggedIn = CommonHelper.userLoggedIn((ClaimsIdentity)User.Identity);
            string id_auditor = _userLoggedIn.id_auditor;
            string id_auditee = _userLoggedIn.id_auditee;
            string role = _userLoggedIn.role;
            List<VerifikasiAuditor> result = await _auditRepository.VerifikasiAuditorList(search, role, id_auditor, id_auditee);
            var json = System.Text.Json.JsonSerializer.Serialize(result);
            return Ok(json);
        }

        // VERIFIKASI Log
        // Menampilkan Log History
        [Route("log/list")]
        public async Task<IActionResult> LogHistoryList()
        {
            var search = "";
            if (Request.ContentType != null)
            {
                var req = Request.Form;
                if (req.ContainsKey("inputSearch")) search = req["inputSearch"];
            }
            List<Log> result = await _auditRepository.LogList(search);
            var json = System.Text.Json.JsonSerializer.Serialize(result);
            return Ok(json);
        }

    }
}
