using System.Collections.Generic;
using System.Linq;
using Fabi.LeBibi.Model.Results;
using Fabi.LeBibi.Business.Library;
using Fabi.LeBibi.Data;
using Fabi.LeBibi.Model;

namespace Fabi.LeBibi.Business
{
    public class PerfilFuncionalidadeBusiness : BusinessBase
    {
        public PerfilFuncionalidade RetornaPerfilFuncionalidade_Id(int id)
        {
            LimpaValidacao();
            PerfilFuncionalidade RetornoAcao = null;
            if (IsValid())
            {
                using (PerfilFuncionalidadeData data = new PerfilFuncionalidadeData())
                {
                    RetornoAcao = data.RetornaPerfilFuncionalidade_Id(id);
                }
            }

            return RetornoAcao;
        }
        public IList<PerfilFuncionalidade> RetornaPerfilFuncionalidades()
        {
            LimpaValidacao();
            IList<PerfilFuncionalidade> RetornoAcao = new List<PerfilFuncionalidade>();
            if (IsValid())
            {
                using (PerfilFuncionalidadeData data = new PerfilFuncionalidadeData())
                {
                    RetornoAcao = data.RetornaPerfilFuncionalidades();
                }
            }

            return RetornoAcao;
        }
        public IList<PerfilFuncionalidade> RetornaPerfilFuncionalidades_PerfilId_FuncionalidadeId(int? perfilId, int? funcionalidadeId)
        {
            LimpaValidacao();
            IList<PerfilFuncionalidade> RetornoAcao = new List<PerfilFuncionalidade>();
            if (IsValid())
            {
                using (PerfilFuncionalidadeData data = new PerfilFuncionalidadeData())
                {
                    RetornoAcao = data.RetornaPerfilFuncionalidades_PerfilId_FuncionalidadeId(perfilId, funcionalidadeId);
                }
            }

            return RetornoAcao;
        }

        public void SalvaPerfilFuncionalidade(PerfilFuncionalidade itemGravar)
        {
            LimpaValidacao();
            ValidateService(itemGravar);
            ValidaRegrasSalvar(itemGravar);
            if (IsValid())
            {
                using (PerfilFuncionalidadeData data = new PerfilFuncionalidadeData())
                {
                    data.SalvaPerfilFuncionalidade(itemGravar);
                    IncluiSucessoBusiness("PerfilFuncionalidade_SalvaAcessoOK");
                }
            }
        }
        public void ExcluiPerfilFuncionalidade(PerfilFuncionalidade itemGravar)
        {
            LimpaValidacao();
            ValidateService(itemGravar);
            ValidaRegrasExcluir(itemGravar);
            if (IsValid())
            {
                using (PerfilFuncionalidadeData data = new PerfilFuncionalidadeData())
                {
                    data.ExcluiPerfilFuncionalidade(itemGravar);
                    IncluiSucessoBusiness("PerfilFuncionalidade_ExcluiAcessoOK");
                }
            }
        }

        public void ValidaRegrasSalvar(PerfilFuncionalidade itemGravar)
        {

        }
        public void ValidaRegrasExcluir(PerfilFuncionalidade itemGravar)
        {
            ValidaExistencia(itemGravar);
        }
        public void ValidaExistencia(PerfilFuncionalidade itemGravar)
        {
            if (itemGravar == null)
                IncluiErroBusiness("PerfilFuncionalidade_NaoEncontrado");
        }
    }
}
