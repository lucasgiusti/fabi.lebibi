using System.Data.Entity.ModelConfiguration;
using Fabi.LeBibi.Model;

namespace Fabi.LeBibi.Data.Configuration
{
    public partial class VendaConfiguration : EntityTypeConfiguration<Venda>
    {
        public VendaConfiguration()
        {
            string Schema = System.Configuration.ConfigurationManager.AppSettings["Schema"];
            if (string.IsNullOrEmpty(Schema))

                this.ToTable("Venda");
            else
                this.ToTable("Venda", Schema);
            this.HasKey(i => new { i.Id });
            this.Property(i => i.Id).HasColumnName("Id");
            this.Property(i => i.ClienteId).HasColumnName("ClienteId");
            this.Property(i => i.ValorTotal).HasColumnName("ValorTotal");
            this.Property(i => i.DescontoPorcentagem).HasColumnName("DescontoPorcentagem");
            this.Property(i => i.DescontoValor).HasColumnName("DescontoValor");
            this.Property(i => i.ValorFinal).HasColumnName("ValorFinal");
            this.Property(i => i.DataInclusao).HasColumnName("DataInclusao");
            this.Property(i => i.DataAlteracao).HasColumnName("DataAlteracao");
        }
    }
}
