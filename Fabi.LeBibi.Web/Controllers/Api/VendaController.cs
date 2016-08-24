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
    /// Venda
    /// </summary>
    public class VendaController : ApiBase
    {
        VendaBusiness biz = new VendaBusiness();

        /// <summary>
        /// Retorna todas as vendas
        /// </summary>
        /// <returns></returns>
        public List<Venda> Get()
        {
            List<Venda> ResultadoBusca = new List<Venda>();
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeVendaConsulta, Constantes.FuncionalidadeNomeVendaConsulta, biz);

                //API
                ResultadoBusca = new List<Venda>(biz.RetornaVendas());

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
        /// Retorna a venda com id solicidado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Venda Get(int id)
        {
            Venda ResultadoBusca = new Venda();
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeVendaConsulta, Constantes.FuncionalidadeNomeVendaConsulta, biz);

                //API
                ResultadoBusca = biz.RetornaVenda_Id(id);

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
        /// Inclui uma venda
        /// </summary>
        /// <param name="itemSalvar"></param>
        /// <returns></returns>
        public int? Post([FromBody]Venda itemSalvar)
        {
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeVendaInclusao, Constantes.FuncionalidadeNomeVendaInclusao, biz);

                //API
                biz.SalvaVenda(itemSalvar);
                if (!biz.IsValid())
                    throw new InvalidDataException();

                GravaLog(Constantes.FuncionalidadeNomeVendaInclusao, RetornaEmailAutenticado(), itemSalvar.Id);
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
        /// Altera uma determinada venda
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemSalvar"></param>
        /// <returns></returns>
        public Venda Put(int id, [FromBody]Venda itemSalvar)
        {
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeVendaEdicao, Constantes.FuncionalidadeNomeVendaEdicao, biz);

                //API
                itemSalvar.Id = id;
                biz.SalvaVenda(itemSalvar);

                if (!biz.IsValid())
                    throw new InvalidDataException();

                GravaLog(Constantes.FuncionalidadeNomeVendaEdicao, RetornaEmailAutenticado(), itemSalvar.Id);
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
        /// Exclui uma determinada venda
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage retorno = null;
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeVendaExclusao, Constantes.FuncionalidadeNomeVendaExclusao, biz);

                //API
                Venda itemExcluir = biz.RetornaVenda_Id(id);

                biz.ExcluiVenda(itemExcluir);
                if (!biz.IsValid())
                    throw new InvalidDataException();

                retorno = RetornaMensagemOk(biz.serviceResult);

                GravaLog(Constantes.FuncionalidadeNomeVendaExclusao, RetornaEmailAutenticado(), itemExcluir.Id);
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