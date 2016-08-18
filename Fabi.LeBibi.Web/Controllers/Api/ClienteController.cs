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
    /// Cliente
    /// </summary>
    public class ClienteController : ApiBase
    {
        ClienteBusiness biz = new ClienteBusiness();

        /// <summary>
        /// Retorna todos os clientes
        /// </summary>
        /// <returns></returns>
        public List<Cliente> Get()
        {
            List<Cliente> ResultadoBusca = new List<Cliente>();
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeClienteConsulta, Constantes.FuncionalidadeNomeClienteConsulta, biz);

                //API
                ResultadoBusca = new List<Cliente>(biz.RetornaClientes());

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
        /// Retorna o cliente com id solicidado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cliente Get(int id)
        {
            Cliente ResultadoBusca = new Cliente();
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeClienteConsulta, Constantes.FuncionalidadeNomeClienteConsulta, biz);

                //API
                ResultadoBusca = biz.RetornaCliente_Id(id);

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
        /// Inclui um cliente
        /// </summary>
        /// <param name="itemSalvar"></param>
        /// <returns></returns>
        public int? Post([FromBody]Cliente itemSalvar)
        {
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeClienteInclusao, Constantes.FuncionalidadeNomeClienteInclusao, biz);

                //API
                biz.SalvaCliente(itemSalvar);
                if (!biz.IsValid())
                    throw new InvalidDataException();

                GravaLog(Constantes.FuncionalidadeNomeClienteInclusao, RetornaEmailAutenticado(), itemSalvar.Id);
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
        /// Altera um determinado cliente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemSalvar"></param>
        /// <returns></returns>
        public Cliente Put(int id, [FromBody]Cliente itemSalvar)
        {
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeClienteEdicao, Constantes.FuncionalidadeNomeClienteEdicao, biz);

                //API
                itemSalvar.Id = id;
                biz.SalvaCliente(itemSalvar);

                if (!biz.IsValid())
                    throw new InvalidDataException();

                GravaLog(Constantes.FuncionalidadeNomeClienteEdicao, RetornaEmailAutenticado(), itemSalvar.Id);
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
        /// Exclui um determinado cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage retorno = null;
            try
            {
                VerificaAutenticacao(Constantes.FuncionalidadeClienteExclusao, Constantes.FuncionalidadeNomeClienteExclusao, biz);

                //API
                Cliente itemExcluir = biz.RetornaCliente_Id(id);

                biz.ExcluiCliente(itemExcluir);
                if (!biz.IsValid())
                    throw new InvalidDataException();

                retorno = RetornaMensagemOk(biz.serviceResult);

                GravaLog(Constantes.FuncionalidadeNomeClienteExclusao, RetornaEmailAutenticado(), itemExcluir.Id);
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