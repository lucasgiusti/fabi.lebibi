using System.Collections.Generic;
using System.Linq;
using System.Data;
using Fabi.LeBibi.Model;
using Fabi.LeBibi.Data.Library;

namespace Fabi.LeBibi.Data
{
    public class ClienteData : DataBase
    {
        public Cliente RetornaCliente_Id(int id)
        {
            IQueryable<Cliente> query = Context.Clientes;

            query = query.Where(d => d.Id == id);
            return query.FirstOrDefault();
        }
        public IList<Cliente> RetornaClientes()
        {
            IQueryable<Cliente> query = Context.Clientes;

            return query.ToList();
        }

        public void SalvaCliente(Cliente itemGravar)
        {
            Cliente itemBase = Context.Clientes.Where(f => f.Id == itemGravar.Id).FirstOrDefault();
            if (itemBase == null)
            {
                itemBase = Context.Clientes.Create();
                Context.Entry<Cliente>(itemBase).State = System.Data.Entity.EntityState.Added;
            }
            AtualizaPropriedades<Cliente>(itemBase, itemGravar);

            Context.SaveChanges();
            itemGravar.Id = itemBase.Id;
        }
        public void ExcluiCliente(Cliente itemGravar)
        {
            Cliente itemExcluir = Context.Clientes.Where(f => f.Id == itemGravar.Id).FirstOrDefault();

            Context.Entry<Cliente>(itemExcluir).State = System.Data.Entity.EntityState.Deleted;
            Context.SaveChanges();
        }
    }
}
