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
    public class ProdutoBusiness : BusinessBase
    {
        public Produto RetornaProduto_Id(int id)
        {
            LimpaValidacao();
            Produto RetornoAcao = null;
            if (IsValid())
            {
                using (ProdutoData data = new ProdutoData())
                {
                    RetornoAcao = data.RetornaProduto_Id(id);
                }
            }

            return RetornoAcao;
        }
        public Produto RetornaProduto_Codigo(string codigo)
        {
            LimpaValidacao();
            Produto RetornoAcao = null;
            if (IsValid())
            {
                using (ProdutoData data = new ProdutoData())
                {
                    RetornoAcao = data.RetornaProduto_Codigo(codigo);
                }
            }

            return RetornoAcao;
        }
        public IList<Produto> RetornaProdutos()
        {
            LimpaValidacao();
            IList<Produto> RetornoAcao = new List<Produto>();
            if (IsValid())
            {
                using (ProdutoData data = new ProdutoData())
                {
                    RetornoAcao = data.RetornaProdutos();
                }
            }

            return RetornoAcao;
        }

        public void SalvaProduto(Produto itemGravar)
        {
            LimpaValidacao();
            ValidateService(itemGravar);
            ValidaRegrasSalvar(itemGravar);
            if (IsValid())
            {
                using (ProdutoData data = new ProdutoData())
                {
                    data.SalvaProduto(itemGravar);
                    IncluiSucessoBusiness("Produto_SalvaProdutoOK");
                }
            }
        }
        public void ExcluiProduto(Produto itemGravar)
        {
            LimpaValidacao();
            ValidateService(itemGravar);
            ValidaRegrasExcluir(itemGravar);
            if (IsValid())
            {
                using (ProdutoData data = new ProdutoData())
                {
                    data.ExcluiProduto(itemGravar);
                    IncluiSucessoBusiness("Produto_ExcluiProdutoOK");
                }
            }
        }

        public void ValidaRegrasSalvar(Produto itemGravar)
        {
            if (IsValid() && string.IsNullOrWhiteSpace(itemGravar.Codigo))
                IncluiErroBusiness("Produto_Codigo");

            if (IsValid() && string.IsNullOrWhiteSpace(itemGravar.Descricao))
                IncluiErroBusiness("Produto_Descricao");

            if (IsValid() && !itemGravar.Quantidade.HasValue)
                IncluiErroBusiness("Produto_Quantidade");

            if (IsValid() && !itemGravar.ValorCompra.HasValue)
                IncluiErroBusiness("Produto_ValorCompra");

            if (IsValid() && !itemGravar.MargemLucro.HasValue)
                IncluiErroBusiness("Produto_MargemLucro");

            if (IsValid() && !itemGravar.ValorVenda.HasValue)
                IncluiErroBusiness("Produto_ValorVenda");

            if (IsValid())
            {
                Produto itemBase = RetornaProduto_Codigo(itemGravar.Codigo);
                if (itemBase != null && itemGravar.Id != itemBase.Id)
                    IncluiErroBusiness("Produto_CadastroDuplicado");
            }
            if (IsValid())
            {
                if (itemGravar.Id.HasValue)
                {
                    Produto itemBase = RetornaProduto_Id((int)itemGravar.Id);
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
        public void ValidaRegrasExcluir(Produto itemGravar)
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
        public void ValidaExistencia(Produto itemGravar)
        {
            if (itemGravar == null)
                IncluiErroBusiness("Produto_NaoEncontrado");
        }
    }
}
