using System.Collections.Generic;
using System.Linq;
using Fabi.LeBibi.Model.Results;
using Fabi.LeBibi.Business.Library;
using Fabi.LeBibi.Data;
using Fabi.LeBibi.Model;
using System;
using Fabi.LeBibi.Model.Dominio;
using System.Web.Security;

namespace Fabi.LeBibi.Business
{
    public class LogBusiness : BusinessBase
    {
        public IList<Log> RetornaLogs()
        {
            LimpaValidacao();
            IList<Log> RetornoAcao = new List<Log>();
            if (IsValid())
            {
                using (LogData data = new LogData())
                {
                    RetornoAcao = data.RetornaLogs();
                }
            }

            return RetornoAcao;
        }
        public bool ExisteLog_UsuarioId(int id)
        {
            LimpaValidacao();
            bool RetornoAcao = false;
            if (IsValid())
            {
                using (LogData data = new LogData())
                {
                    RetornoAcao = data.ExisteLog_UsuarioId(id);
                }
            }

            return RetornoAcao;
        }

        public void SalvaLog(Log itemGravar)
        {
            ValidateService(itemGravar);
            ValidaRegrasNegocioLog(itemGravar);
            if (IsValid())
            {
                using (LogData data = new LogData())
                {
                    data.SalvaLog(itemGravar);
                }
            }
        }

        private void ValidaRegrasNegocioLog(Log itemGravar)
        {
        }
    }
}
