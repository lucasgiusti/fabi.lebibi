using System.Collections.Generic;
using System.Linq;
using Fabi.LeBibi.Model.Results;
using Fabi.LeBibi.Business.Library;
using Fabi.LeBibi.Data;
using Fabi.LeBibi.Model;
using System;
using Fabi.LeBibi.Model.Dominio;
using System.Web.Security;
using System.Text;

namespace Fabi.LeBibi.Business
{
    public class EmailBusiness : BusinessBase
    {
        public void GeraEmailEsqueciSenha(Usuario itemGravar, string novaSenha)
        {
            Email email = new Email();
            email.Assunto = Constantes.AssuntoEmailEsqueciSenha;
            email.Corpo = string.Format(Constantes.CorpoEmailEsqueciSenha, itemGravar.Nome, novaSenha);
            email.DataInclusao = DateTime.Now;
            email.FuncionalidadeId = Convert.ToInt32(Constantes.FuncionalidadeAlterarSenha);
            email.Destinatario = itemGravar.Email;
            SalvaEmail(email);
        }

        public IList<Email> RetornaEmails(bool Enviado)
        {
            LimpaValidacao();
            IList<Email> RetornoAcao = new List<Email>();
            if (IsValid())
            {
                using (EmailData data = new EmailData())
                {
                    RetornoAcao = data.RetornaEmails(Enviado);
                }
            }
            return RetornoAcao;
        }
        public void SalvaEmail(Email itemGravar)
        {
            LimpaValidacao();
            ValidateService(itemGravar);
            ValidrRegrasNegocioSalvar(itemGravar);
            if (IsValid())
            {
                using (EmailData data = new EmailData())
                {
                    data.SalvaEmail(itemGravar);
                    IncluiSucessoBusiness("Email_SalvaEmailOK");
                }
            }
        }
        
        public void EnviaEmails()
        {
            var emails = RetornaEmails(false);
            emails.ToList().ForEach(a =>
            {
                try
                {
                    UtilitarioBusiness.EnviaEmail(UtilitarioBusiness.RetornaChaveConfig("nomeRemetente"), UtilitarioBusiness.RetornaChaveConfig("emailRemetente"), a.Destinatario, a.Assunto, a.Corpo);
                    a.DataEnvio = DateTime.Now;
                    a.DataAlteracao = DateTime.Now;
                    SalvaEmail(a);
                }
                catch (Exception ex)
                {
                    IncluiErroBusiness(MensagemBusiness.RetornaMensagens("Email_ErroEnvio", new string[] { a.Id.ToString(), UtilitarioBusiness.RetornaExceptionMessages(ex) }), true);
                }
            });

            if (!IsValid())
            {
                StringBuilder texto = new StringBuilder();
                serviceResult.Messages.ForEach(a => texto.AppendLine(a.Message));
                UtilitarioBusiness.GravaArquivoTexto(UtilitarioBusiness.RetornaChaveConfig("caminhoArquivoEmailLogErro") + "\\" + string.Format(UtilitarioBusiness.RetornaChaveConfig("nomeArquivoEmailLogErro"), DateTime.Now.ToString("yyyMMddHHmmss")), false, texto.ToString());
            }
        }

        private void ValidrRegrasNegocioSalvar(Email itemGravar)
        {
        }
    }
}
