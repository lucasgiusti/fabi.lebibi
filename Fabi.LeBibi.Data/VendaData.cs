using System.Collections.Generic;
using System.Linq;
using System.Data;
using Fabi.LeBibi.Model;
using Fabi.LeBibi.Data.Library;

namespace Fabi.LeBibi.Data
{
    public class VendaData : DataBase
    {
        public Venda RetornaVenda_Id(int id)
        {
            IQueryable<Venda> query = Context.Vendas;

            query = query.Where(d => d.Id == id);
            return query.FirstOrDefault();
        }
        public IList<Venda> RetornaVendas_ClienteId(int clienteId)
        {
            IQueryable<Venda> query = Context.Vendas;

            query = query.Where(d => d.ClienteId == clienteId);
            return query.ToList();
        }
        public IList<Venda> RetornaVendas()
        {
            IQueryable<Venda> query = Context.Vendas;

            return query.ToList();
        }

        public void SalvaVenda(Venda itemGravar)
        {
            Venda itemBase = Context.Vendas.Where(f => f.Id == itemGravar.Id).FirstOrDefault();
            if (itemBase == null)
            {
                itemBase = Context.Vendas.Create();
                Context.Entry<Venda>(itemBase).State = System.Data.Entity.EntityState.Added;
            }
            AtualizaPropriedades<Venda>(itemBase, itemGravar);

            Context.SaveChanges();
            itemGravar.Id = itemBase.Id;
        }
        public void ExcluiVenda(Venda itemGravar)
        {
            Venda itemExcluir = Context.Vendas.Where(f => f.Id == itemGravar.Id).FirstOrDefault();

            Context.Entry<Venda>(itemExcluir).State = System.Data.Entity.EntityState.Deleted;
            Context.SaveChanges();
        }
    }
}
