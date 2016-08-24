using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Fabi.LeBibi.Model
{
    [HasSelfValidation()]
    public class Venda
    {
        public Venda()
        {

        }

        public int? Id { get; set; }
        public int? ClienteId { get; set; }
        public double? ValorTotal { get; set; }
        public int? DescontoPorcentagem { get; set; }
        public double? DescontoValor { get; set; }
        public double? ValorFinal { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public Cliente Cliente { get; set; }
    }
}
