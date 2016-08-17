using System;
using System.Resources;
using System.Reflection;

namespace Fabi.LeBibi.Business
{
    public static class MensagemBusiness
    {
        public static string RetornaMensagens(string codMensagem)
        {
            ResourceManager Mensagem = new ResourceManager("Fabi.LeBibi.Business.Resource.Mensagem", Assembly.GetExecutingAssembly());
            return Mensagem.GetString(codMensagem, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }
        public static string RetornaMensagens(string codMensagem, string[] complementos)
        {
            return String.Format(RetornaMensagens(codMensagem), complementos);
        }
    }
}