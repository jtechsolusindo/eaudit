using EAudit.DAO.StandarSpmiDao;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EAudit.DAO.StandarSpmi
{
    public class StandarSpmiRepository : IStandarSpmi
    {
        public IOptions<AppSettings> _options;

        public StandarSpmiRepository(IOptions<AppSettings> options)
        {
            _options = options;
        }

        public async Task<Models.StandarSpmi> getStandarRow(Models.StandarSpmi filter)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            SqlParameter[] parameters = {
            new SqlParameter("@id", System.Data.SqlDbType.Int,10, filter.ID.Value.ToString())
        };
            DBAccess dbAccess = new DBAccess(_options);
            List<Models.StandarSpmi> list = await dbAccess.ExecuteReaderAsync<Models.StandarSpmi>("SP_STANDARSPMI_ROW", parameters);
            return list[0];
        }

        public async Task<List<Models.StandarSpmi>> getStandarList(string search)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            ArrayList listParameters = new ArrayList();
            string query = @"
                           SELECT * FROM (
                                SELECT 
                                  tss.ID_STANDAR  AS ID,
                                  tss.NO_STANDAR  AS NOSTANDAR,
                                  tss.PERNYATAAN  AS PERNYATAAN,
                                  tss.INDIKATOR   AS INDIKATOR 
                                FROM TBL_STANDAR_SPMI tss   
                                WHERE tss.DELETED='N'
                              ) AS TempTable
                            WHERE 1=1 ";
            query += @" AND (TempTable.PERNYATAAN LIKE '%' + COALESCE(@search,'%') + '%' OR
                        TempTable.NOSTANDAR LIKE '%' + COALESCE(@search,'%') + '%' )";
            listParameters.Add(new SqlParameter("@search", System.Data.SqlDbType.VarChar, 50, search));


            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            List<Models.StandarSpmi> list = await dbAccess.ExecuteAsync<Models.StandarSpmi>(query, parameters);
            return list;
        }
    }
}
