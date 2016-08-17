using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Resources;
using System.Reflection;
using Fabi.LeBibi.Model.Resource;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Fabi.LeBibi.Model
{

    public partial class PerfilUsuario
    {
        public PerfilUsuario()
        {
        }
        public int? Id { get; set; }
        public int? PerfilId { get; set; }
        public int? UsuarioId { get; set; }
    }
}
