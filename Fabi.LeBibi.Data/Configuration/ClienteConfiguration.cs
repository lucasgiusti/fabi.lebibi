using System.Data.Entity.ModelConfiguration;
using Fabi.LeBibi.Model;

namespace Fabi.LeBibi.Data.Configuration
{
    public partial class ClienteConfiguration : EntityTypeConfiguration<Cliente>
    {
        public ClienteConfiguration()
        {
            string Schema = System.Configuration.ConfigurationManager.AppSettings["Schema"];
            if (string.IsNullOrEmpty(Schema))

                this.ToTable("Cliente");
            else
                this.ToTable("Cliente", Schema);
            this.HasKey(i => new { i.Id });
            this.Property(i => i.Id).HasColumnName("Id");
            this.Property(i => i.Nome).HasColumnName("Nome");
            this.Property(i => i.Email).HasColumnName("Email");
            this.Property(i => i.Telefone1).HasColumnName("Telefone1");
            this.Property(i => i.Telefone2).HasColumnName("Telefone2");
            this.Property(i => i.DataInclusao).HasColumnName("DataInclusao");
            this.Property(i => i.DataAlteracao).HasColumnName("DataAlteracao");
        }
    }
}
