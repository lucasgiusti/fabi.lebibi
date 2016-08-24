using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Fabi.LeBibi.Model
{
    [HasSelfValidation()]
    public class Cliente
    {
        public Cliente()
        {

        }

        public int? Id { get; set; }
        public string Nome { get; set; }
        [SelfValidation]
        private void ValidaNome(Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResults results)
        {
            if (Nome != null && Nome.Length > 100)
            {
                Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult result =
                      new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult(Resource.Mensagem.Produto_NomeTamanho, this, "Nome", null, null);
                results.AddResult(result);
            }
        }
        public string Email { get; set; }
        [SelfValidation]
        private void ValidaEmail(Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResults results)
        {
            if (Email != null && Email.Length > 100)
            {
                Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult result =
                      new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult(Resource.Mensagem.Produto_EmailTamanho, this, "Email", null, null);
                results.AddResult(result);
            }
        }
        public string Telefone1 { get; set; }
        [SelfValidation]
        private void ValidaTelefone1(Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResults results)
        {
            if (Telefone1 != null && Telefone1.Length > 20)
            {
                Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult result =
                      new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult(Resource.Mensagem.Produto_Telefone1Tamanho, this, "Telefone1", null, null);
                results.AddResult(result);
            }
        }
        public string Telefone2 { get; set; }
        [SelfValidation]
        private void ValidaTelefone2(Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResults results)
        {
            if (Telefone2 != null && Telefone2.Length > 20)
            {
                Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult result =
                      new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult(Resource.Mensagem.Produto_Telefone2Tamanho, this, "Telefone2", null, null);
                results.AddResult(result);
            }
        }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public IList<Venda> Vendas { get; set; }
    }
}
