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
    /// EsqueciSenha
    /// </summary>
    public class EsqueciSenhaController : ApiBase
    {
        UsuarioBusiness biz = new UsuarioBusiness();

        /// <summary>
        /// Gera nova senha de um determinado usuário
        /// </summary>
        /// <param name="itemSalvar"></param>
        /// <returns></returns>
        public int? Post([FromBody]Usuario itemSalvar)
        {
            try
            {
                //API
                biz.GeraNovaSenha(itemSalvar);
                if (!biz.IsValid())
                    throw new InvalidDataException();

                if (itemSalvar != null)
                {
                    itemSalvar.Senha = null;
                    itemSalvar.SenhaConfirmacao = null;
                }
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
    }
}