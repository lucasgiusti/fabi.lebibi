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
    public class VendaBusiness : BusinessBase
    {
        public Venda RetornaVenda_Id(int id)
        {
            LimpaValidacao();
            Venda RetornoAcao = null;
            if (IsValid())
            {
                using (VendaData data = new VendaData())
                {
                    RetornoAcao = data.RetornaVenda_Id(id);
                }
            }

            return RetornoAcao;
        }
        public IList<Venda> RetornaVendas_ClienteId(int clienteId)
        {
            LimpaValidacao();
            IList<Venda> RetornoAcao = new List<Venda>();
            if (IsValid())
            {
                using (VendaData data = new VendaData())
                {
                    RetornoAcao = data.RetornaVendas_ClienteId(clienteId);
                }
            }

            return RetornoAcao;
        }
        public IList<Venda> RetornaVendas()
        {
            LimpaValidacao();
            IList<Venda> RetornoAcao = new List<Venda>();
            if (IsValid())
            {
                using (VendaData data = new VendaData())
                {
                    RetornoAcao = data.RetornaVendas();
                }
            }

            return RetornoAcao;
        }

        public void SalvaVenda(Venda itemGravar)
        {
            LimpaValidacao();
            ValidateService(itemGravar);
            ValidaRegrasSalvar(itemGravar);
            if (IsValid())
            {
                using (VendaData data = new VendaData())
                {
                    data.SalvaVenda(itemGravar);
                    IncluiSucessoBusiness("Venda_SalvaVendaOK");
                }
            }
        }
        public void ExcluiVenda(Venda itemGravar)
        {
            LimpaValidacao();
            ValidateService(itemGravar);
            ValidaRegrasExcluir(itemGravar);
            if (IsValid())
            {
                using (VendaData data = new VendaData())
                {
                    data.ExcluiVenda(itemGravar);
                    IncluiSucessoBusiness("Venda_ExcluiVendaOK");
                }
            }
        }

        public void ValidaRegrasSalvar(Venda itemGravar)
        {
            if (IsValid() && !itemGravar.ValorTotal.HasValue)
                IncluiErroBusiness("Venda_ValorTotal");

            if (IsValid() && !itemGravar.ValorFinal.HasValue)
                IncluiErroBusiness("Venda_ValorFinal");

            if (IsValid())
            {
                if (itemGravar.Id.HasValue)
                {
                    Venda itemBase = RetornaVenda_Id((int)itemGravar.Id);
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
        public void ValidaRegrasExcluir(Venda itemGravar)
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
        public void ValidaExistencia(Venda itemGravar)
        {
            if (itemGravar == null)
                IncluiErroBusiness("Venda_NaoEncontrada");
        }
    }
}
