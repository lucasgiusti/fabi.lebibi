using System;
using System.IO;
using System.Configuration;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Fabi.LeBibi.Business.Library
{
    public class UtilitarioBusiness
    {
        public static void GravaArquivoTexto(string path, bool append, string conteudo)
        {
            StreamWriter writer = new StreamWriter(path, append, System.Text.Encoding.UTF8);
            try
            {
                writer.Write(conteudo);
                writer.Flush();
            }
            finally
            {
                writer.Close();
            }
        }
        public static void GravaLog(string path, string mensagem)
        {
            path += "LOG" + " " + DateTime.Today.Date.ToShortDateString().Replace("/", "-").Replace(" ", "").Replace("00:00:00", "") + ".txt";
            GravaArquivoTexto(path, true, mensagem);
        }
        public static void EnviaEmail(string nomeRemetente, string emailRemetente, string emailDestinatario, string assuntoMensagem, string conteudoMensagem)
        {
            MailMessage objEmail = new MailMessage();

            //Define o Campo From e ReplyTo do e-mail.
            objEmail.From = new System.Net.Mail.MailAddress(nomeRemetente + "<" + emailRemetente + ">");

            //Define os destinatários do e-mail.
            objEmail.To.Add(emailDestinatario);

            //Define a prioridade do e-mail.
            objEmail.Priority = System.Net.Mail.MailPriority.Normal;

            //Define o formato do e-mail HTML (caso não queira HTML alocar valor false)
            objEmail.IsBodyHtml = true;

            //Define título do e-mail.
            objEmail.Subject = assuntoMensagem;

            //Define o corpo do e-mail.
            objEmail.Body = conteudoMensagem;

            //Para evitar problemas de caracteres "estranhos", configuramos o charset para "ISO-8859-1"
            objEmail.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            objEmail.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");




            // Caso queira enviar um arquivo anexo
            //Caminho do arquivo a ser enviado como anexo
            //string arquivo = Server.MapPath("arquivo.jpg");

            // Ou especifique o caminho manualmente
            //string arquivo = @"e:\home\LoginFTP\Web\arquivo.jpg";

            // Cria o anexo para o e-mail
            //Attachment anexo = new Attachment(arquivo, System.Net.Mime.MediaTypeNames.Application.Octet);

            // Anexa o arquivo a mensagemn
            //objEmail.Attachments.Add(anexo);

            //Cria objeto com os dados do SMTP
            System.Net.Mail.SmtpClient objSmtp = new System.Net.Mail.SmtpClient();

            //Alocamos o endereço do host para enviar os e-mails, localhost(recomendado) 
            objSmtp.Host = RetornaChaveConfig("smtpServer");
            objSmtp.Port = Convert.ToInt32(RetornaChaveConfig("smtpPort"));

            bool utilizarCredencial = Convert.ToBoolean(RetornaChaveConfig("utilizarCredencial"));

            if (utilizarCredencial)
            {
                objSmtp.EnableSsl = true;
                objSmtp.Credentials = new System.Net.NetworkCredential(RetornaChaveConfig("credencialEmail"), RetornaChaveConfig("senhaCredencialEmail"));
            }

            //Enviamos o e-mail através do método .send()
            try
            {
                objSmtp.Send(objEmail);
            }
            catch
            {
                throw;
            }
            finally
            {
                objEmail.Dispose();
            }
        }
        public static string RetornaChaveConfig(string nome)
        {
            if (ConfigurationManager.AppSettings[nome] != null)
                return ConfigurationManager.AppSettings[nome].ToString();
            else
                return null;
        }
        public static string RetornaExceptionMessages(Exception e, string msgs = "")
        {
            if (e == null) return string.Empty;
            if (msgs == "") msgs = e.Message;
            if (e.InnerException != null)
                msgs += "\r\nInnerException: " + RetornaExceptionMessages(e.InnerException);
            return msgs;
        }

        bool invalidMail = false;
        public bool IsValidEmail(string strIn)
        {
            invalidMail = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalidMail)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalidMail = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}
