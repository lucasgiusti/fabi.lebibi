using System.Collections.Generic;
using System.Linq;
using System.Data;
using Fabi.LeBibi.Model;
using Fabi.LeBibi.Data.Library;

namespace Fabi.LeBibi.Data
{
    public class PerfilUsuarioData : DataBase
    {
        public PerfilUsuario RetornaPerfilUsuario_Id(int? id)
        {
            IQueryable<PerfilUsuario> query = Context.PerfilUsuarios;
            if (id.HasValue)
                query = query.Where(d => d.Id == id);
            return query.FirstOrDefault();
        }
        public IList<PerfilUsuario> RetornaPerfilUsuarios()
        {
            IQueryable<PerfilUsuario> query = Context.PerfilUsuarios;
            return query.ToList();
        }
        public IList<PerfilUsuario> RetornaPerfilUsuarios_PerfilId_UsuarioId(int? perfilId, int? usuarioId)
        {
            IQueryable<PerfilUsuario> query = Context.PerfilUsuarios;
            if (perfilId.HasValue)
                query = query.Where(d => d.PerfilId == perfilId);
            if (usuarioId.HasValue)
                query = query.Where(d => d.UsuarioId == usuarioId);
            return query.ToList();
        }
        public List<string> RetornaFuncionalidades_UsuarioId(int usuarioId)
        {
            List<int?> listFuncionalidades =
            Context.PerfilUsuarios.Where(d => d.UsuarioId == usuarioId)
                .Join(Context.Perfis,
                pu => pu.PerfilId,
                p => p.Id,
                (pu, p) => p).SelectMany(p => p.PerfilFuncionalidades).Select(p => p.FuncionalidadeId).Distinct().ToList();

            List<string> listFuncionalidadesStr = new List<string>();
            foreach (int? funcionalidadeId in listFuncionalidades)
                listFuncionalidadesStr.Add(funcionalidadeId.ToString());
            return listFuncionalidadesStr;
        }

        public void SalvaPerfilUsuario(PerfilUsuario itemGravar)
        {
            PerfilUsuario itemBase = Context.PerfilUsuarios.Where(f => f.Id == itemGravar.Id).FirstOrDefault();
            if (itemBase == null)
            {
                itemBase = Context.PerfilUsuarios.Create();

                Context.Entry<PerfilUsuario>(itemBase).State = System.Data.Entity.EntityState.Added;
            }
            AtualizaPropriedades<PerfilUsuario>(itemBase, itemGravar);

            Context.SaveChanges();
            itemGravar.Id = itemBase.Id;

        }
        public void ExcluiPerfilUsuario(PerfilUsuario itemGravar)
        {
            PerfilUsuario itemExcluir = Context.PerfilUsuarios.Where(f => f.Id == itemGravar.Id).FirstOrDefault();
            Context.Entry<PerfilUsuario>(itemExcluir).State = System.Data.Entity.EntityState.Deleted;

            Context.SaveChanges();
        }
    }
}
