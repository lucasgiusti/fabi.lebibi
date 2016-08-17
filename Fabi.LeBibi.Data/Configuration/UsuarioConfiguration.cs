using System.Data.Entity.ModelConfiguration;
using Fabi.LeBibi.Model;

namespace Fabi.LeBibi.Data.Configuration
{
    public partial class UsuarioConfiguration : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfiguration()
        {
            string Schema = System.Configuration.ConfigurationManager.AppSettings["Schema"];
            if (string.IsNullOrEmpty(Schema))

                this.ToTable("Usuario");
            else
                this.ToTable("Usuario", Schema);
            this.HasKey(i => new { i.Id });
            this.Property(i => i.Id).HasColumnName("Id");
            this.Property(i => i.Nome).HasColumnName("Nome");
            this.Property(i => i.Email).HasColumnName("Email");
            this.Property(i => i.Senha).HasColumnName("Senha");
            this.Property(i => i.Ativo).HasColumnName("Ativo");
            this.Property(i => i.DataInclusao).HasColumnName("DataInclusao");
            this.Property(i => i.DataAlteracao).HasColumnName("DataAlteracao");
            this.Ignore(i => i.NovaSenha);
            this.Ignore(i => i.SenhaConfirmacao);
        }
    }
}
