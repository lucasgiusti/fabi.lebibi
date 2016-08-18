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
    public class ClienteBusiness : BusinessBase
    {
        public Cliente RetornaCliente_Id(int id)
        {
            LimpaValidacao();
            Cliente RetornoAcao = null;
            if (IsValid())
            {
                using (ClienteData data = new ClienteData())
                {
                    RetornoAcao = data.RetornaCliente_Id(id);
                }
            }

            return RetornoAcao;
        }
        public IList<Cliente> RetornaClientes()
        {
            LimpaValidacao();
            IList<Cliente> RetornoAcao = new List<Cliente>();
            if (IsValid())
            {
                using (ClienteData data = new ClienteData())
                {
                    RetornoAcao = data.RetornaClientes();
                }
            }

            return RetornoAcao;
        }

        public void SalvaCliente(Cliente itemGravar)
        {
            LimpaValidacao();
            ValidateService(itemGravar);
            ValidaRegrasSalvar(itemGravar);
            if (IsValid())
            {
                using (ClienteData data = new ClienteData())
                {
                    data.SalvaCliente(itemGravar);
                    IncluiSucessoBusiness("Cliente_SalvaClienteOK");
                }
            }
        }
        public void ExcluiCliente(Cliente itemGravar)
        {
            LimpaValidacao();
            ValidateService(itemGravar);
            ValidaRegrasExcluir(itemGravar);
            if (IsValid())
            {
                using (ClienteData data = new ClienteData())
                {
                    data.ExcluiCliente(itemGravar);
                    IncluiSucessoBusiness("Cliente_ExcluiClienteOK");
                }
            }
        }

        public void ValidaRegrasSalvar(Cliente itemGravar)
        {
            if (IsValid() && string.IsNullOrWhiteSpace(itemGravar.Nome))
                IncluiErroBusiness("Cliente_Nome");
            if (IsValid() && !string.IsNullOrWhiteSpace(itemGravar.Email))
            {
                UtilitarioBusiness util = new UtilitarioBusiness();
                if (!util.IsValidEmail(itemGravar.Email))
                    IncluiErroBusiness("Cliente_EmailInvalido");
            }
            if (IsValid())
            {
                if (itemGravar.Id.HasValue)
                {
                    Cliente itemBase = RetornaCliente_Id((int)itemGravar.Id);
                    ValidaExistencia(itemBase);
                    if (IsValid())
                    {
                        itemGravar.DataInclusao = itemBase.DataInclusao;
                        itemGravar.DataAlteracao = DateTime.Now;
                    }
                }
                else
                    itemGravar.DataInclusao = DateTime.Now;
            }


        }
        public void ValidaRegrasExcluir(Cliente itemGravar)
        {
            if (IsValid())
                ValidaExistencia(itemGravar);

            if (IsValid())
            {
                //PerfilUsuarioBusiness biz = new PerfilUsuarioBusiness();
                //var PerfisAssociados = biz.RetornaPerfilUsuarios_PerfilId_UsuarioId(null, itemGravar.Id);

                //if (PerfisAssociados.Count > 0)
                //    IncluiErroBusiness("Usuario_CadastroUtilizado");
            }
        }
        public void ValidaExistencia(Cliente itemGravar)
        {
            if (itemGravar == null)
                IncluiErroBusiness("Cliente_NaoEncontrado");
        }
    }
}
