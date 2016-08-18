using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Fabi.LeBibi.Model
{
    [HasSelfValidation()]
    public class Produto
    {
        public Produto()
        {
            
        }

        public int? Id { get; set; }
        public string Codigo { get; set; }
        [SelfValidation]
        private void ValidaCodigo(Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResults results)
        {
            if (Codigo != null && Codigo.Length > 50)
            {
                Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult result =
                      new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult(Resource.Mensagem.Produto_CodigoTamanho, this, "Codigo", null, null);
                results.AddResult(result);
            }
        }
        public string Descricao { get; set; }
        [SelfValidation]
        private void ValidaDescricao(Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResults results)
        {
            if (Descricao != null && Descricao.Length > 500)
            {
                Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult result =
                      new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult(Resource.Mensagem.Produto_DescricaoTamanho, this, "Descricao", null, null);
                results.AddResult(result);
            }
        }
        public int? Quantidade { get; set; }
        public double? ValorCompra { get; set; }
        public double? MargemLucro { get; set; }
        public double? ValorVenda { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
