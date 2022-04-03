using System;

namespace EAudit.Models
{
    public class Log
    {
        public int ID_LOG { get; set; }
        public DateTime TANGGAL_WAKTU { get; set; }
        public string KETERANGAN { get; set; }
        public string NPP { get; set; }
    }
}
