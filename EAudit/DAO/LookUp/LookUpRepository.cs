using EAudit.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EAudit.DAO.LookUp
{
    public class LookUpRepository : ILookUp
    {
        public IOptions<AppSettings> _options;

        public LookUpRepository(IOptions<AppSettings> options)
        {
            _options = options;
        }

       
        public async Task<List<LookUp_Standar_SPMI>> getStandarSPMI_List(string search)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            SqlParameter[] parameters = {
                new SqlParameter("@search", System.Data.SqlDbType.VarChar, 50, search)
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<LookUp_Standar_SPMI> list = await dbAccess.ExecuteReaderAsync<LookUp_Standar_SPMI>("SP_STANDARSPMI_LIST", parameters);

            return list;
        }

        public async Task<LookUp_Standar_SPMI> getStandarSPMI_Row(LookUp_Standar_SPMI filter)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.Int,10, filter.ID.Value.ToString())
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<LookUp_Standar_SPMI> list = await dbAccess.ExecuteReaderAsync<LookUp_Standar_SPMI>("SP_STANDARSPMI_ROW", parameters);

            return list[0];
        }

        

        public async void StandarSPMI_Save(LookUp_Standar_SPMI data)
        {
            DBOutput output = new DBOutput();
            output.status = true;

            DBAccess dbAccess = new DBAccess(_options);
            if (data.ID.HasValue)
            {

                SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.VarChar, 10, data.ID.Value.ToString()),
                new SqlParameter("@nostandar", System.Data.SqlDbType.VarChar, 255, data.NOSTANDAR),
                new SqlParameter("@pernyataan", System.Data.SqlDbType.Text, 255, data.PERNYATAAN),
                new SqlParameter("@indikator", System.Data.SqlDbType.VarChar, 255, data.INDIKATOR)
            };
                await dbAccess.ExecuteQuery("SP_STANDARSPMI_UPDATE", parameters);
            }
            else
            {
                SqlParameter[] parameters = {
                new SqlParameter("@nostandar", System.Data.SqlDbType.VarChar, 50, data.NOSTANDAR),
                new SqlParameter("@pernyataan", System.Data.SqlDbType.VarChar, 50, data.PERNYATAAN),
                new SqlParameter("@indikator", System.Data.SqlDbType.VarChar, 50, data.INDIKATOR)
            };

                await dbAccess.ExecuteQuery("SP_STANDARSPMI_SAVE", parameters);
            }


        }

        public async void StandarSPMI_Delete(LookUp_Standar_SPMI data)
        {
            DBOutput output = new DBOutput();
            output.status = true;

            DBAccess dbAccess = new DBAccess(_options);
            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.VarChar, 10, data.ID.Value.ToString()),
            };

            await dbAccess.ExecuteQuery("SP_STANDARSPMI_DELETE", parameters);

        }

    }
}
