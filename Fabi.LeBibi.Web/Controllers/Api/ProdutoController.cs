using System;
using System.Collections.Generic;
using System.Web.Http;
using System.IO;
using Fabi.LeBibi.Web.Library;
using System.Net.Http;
using System.Net;
using Fabi.LeBibi.Model;
using Fabi.LeBibi.Model.Dominio;
using System.Web;
using Fabi.LeBibi.Business;

namespace Fabi.LeBibi.Web.Controllers.Api
{
    /// <summary>
    /// Produto
    /// </summary>
    public class ProdutoController : ApiBase
    {
        ProdutoBusiness biz = new ProdutoBusiness();

        /// <summary>
        /// Retorna todos os produtos
        /// </summary>
        /// <returns></returns>
        public List<Produto> Get()
        {
            List<Produto> ResultadoBusca = new List<Produto>();
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeProdutoConsulta, Constantes.FuncionalidadeNomeProdutoConsulta, biz);

                //API
                ResultadoBusca = new List<Produto>(biz.RetornaProdutos());

                if (!biz.IsValid())
                    throw new InvalidDataException();
            }
            catch (InvalidDataException)
            {
                GeraErro(HttpStatusCode.InternalServerError, biz.serviceResult);
            }
            catch (UnauthorizedAccessException)
            {
                GeraErro(HttpStatusCode.Unauthorized, biz.serviceResult);
            }
            catch (Exception ex)
            {
                GeraErro(HttpStatusCode.BadRequest, ex);
            }

            return ResultadoBusca;
        }

        /// <summary>
        /// Retorna o produto com id solicidado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Produto Get(int id)
        {
            Produto ResultadoBusca = new Produto();
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeProdutoConsulta, Constantes.FuncionalidadeNomeProdutoConsulta, biz);

                //API
                ResultadoBusca = biz.RetornaProduto_Id(id);

                if (!biz.IsValid())
                    throw new InvalidDataException();
            }
            catch (InvalidDataException)
            {
                GeraErro(HttpStatusCode.InternalServerError, biz.serviceResult);
            }
            catch (UnauthorizedAccessException)
            {
                GeraErro(HttpStatusCode.Unauthorized, biz.serviceResult);
            }
            catch (Exception ex)
            {
                GeraErro(HttpStatusCode.BadRequest, ex);
            }

            return ResultadoBusca;
        }

        /// <summary>
        /// Inclui um produto
        /// </summary>
        /// <param name="itemSalvar"></param>
        /// <returns></returns>
        public int? Post([FromBody]Produto itemSalvar)
        {
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeProdutoInclusao, Constantes.FuncionalidadeNomeProdutoInclusao, biz);

                //API
                biz.SalvaProduto(itemSalvar);
                if (!biz.IsValid())
                    throw new InvalidDataException();

                GravaLog(Constantes.FuncionalidadeNomeProdutoInclusao, RetornaEmailAutenticado(), itemSalvar.Id);
            }
            catch (InvalidDataException)
            {
                GeraErro(HttpStatusCode.InternalServerError, biz.serviceResult);
            }
            catch (UnauthorizedAccessException)
            {
                GeraErro(HttpStatusCode.Unauthorized, biz.serviceResult);
            }
            catch (Exception ex)
            {
                GeraErro(HttpStatusCode.BadRequest, ex);
            }

            return itemSalvar.Id;
        }

        /// <summary>
        /// Altera um determinado produto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemSalvar"></param>
        /// <returns></returns>
        public Produto Put(int id, [FromBody]Produto itemSalvar)
        {
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeProdutoEdicao, Constantes.FuncionalidadeNomeProdutoEdicao, biz);

                //API
                itemSalvar.Id = id;
                biz.SalvaProduto(itemSalvar);

                if (!biz.IsValid())
                    throw new InvalidDataException();

                GravaLog(Constantes.FuncionalidadeNomeProdutoEdicao, RetornaEmailAutenticado(), itemSalvar.Id);
            }
            catch (InvalidDataException)
            {
                GeraErro(HttpStatusCode.InternalServerError, biz.serviceResult);
            }
            catch (UnauthorizedAccessException)
            {
                GeraErro(HttpStatusCode.Unauthorized, biz.serviceResult);
            }
            catch (Exception ex)
            {
                GeraErro(HttpStatusCode.BadRequest, ex);
            }

            return itemSalvar;
        }

        /// <summary>
        /// Exclui um determinado produto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage retorno = null;
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeProdutoExclusao, Constantes.FuncionalidadeNomeProdutoExclusao, biz);

                //API
                Produto itemExcluir = biz.RetornaProduto_Id(id);

                biz.ExcluiProduto(itemExcluir);
                if (!biz.IsValid())
                    throw new InvalidDataException();

                retorno = RetornaMensagemOk(biz.serviceResult);

                GravaLog(Constantes.FuncionalidadeNomeProdutoExclusao, RetornaEmailAutenticado(), itemExcluir.Id);
            }
            catch (InvalidDataException)
            {
                retorno = RetornaMensagemErro(HttpStatusCode.InternalServerError, biz.serviceResult);
            }
            catch (UnauthorizedAccessException)
            {
                retorno = RetornaMensagemErro(HttpStatusCode.Unauthorized, biz.serviceResult);
            }
            catch (Exception ex)
            {
                retorno = RetornaMensagemErro(HttpStatusCode.BadRequest, ex);
            }

            return retorno;
        }


    }
}