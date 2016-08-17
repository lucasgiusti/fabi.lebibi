using System.Data.Entity.ModelConfiguration;
using Fabi.LeBibi.Model;

namespace Fabi.LeBibi.Data.Configuration
{
    public partial class ProdutoConfiguration : EntityTypeConfiguration<Produto>
    {
        public ProdutoConfiguration()
        {
            string Schema = System.Configuration.ConfigurationManager.AppSettings["Schema"];
            if (string.IsNullOrEmpty(Schema))

                this.ToTable("Produto");
            else
                this.ToTable("Produto", Schema);
            this.HasKey(i => new { i.Id });
            this.Property(i => i.Id).HasColumnName("Id");
            this.Property(i => i.Codigo).HasColumnName("Codigo");
            this.Property(i => i.Descricao).HasColumnName("Descricao");
            this.Property(i => i.ValorCompra).HasColumnName("ValorCompra");
            this.Property(i => i.MargemLucro).HasColumnName("MargemLucro");
            this.Property(i => i.ValorVenda).HasColumnName("ValorVenda");
            this.Property(i => i.Quantidade).HasColumnName("Quantidade");
            this.Property(i => i.DataInclusao).HasColumnName("DataInclusao");
            this.Property(i => i.DataAlteracao).HasColumnName("DataAlteracao");
        }
    }
}
