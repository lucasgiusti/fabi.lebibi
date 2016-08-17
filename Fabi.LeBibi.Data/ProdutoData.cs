using System.Collections.Generic;
using System.Linq;
using System.Data;
using Fabi.LeBibi.Model;
using Fabi.LeBibi.Data.Library;

namespace Fabi.LeBibi.Data
{
    public class ProdutoData : DataBase
    {
        public Produto RetornaProduto_Id(int id)
        {
            IQueryable<Produto> query = Context.Produtos;

            query = query.Where(d => d.Id == id);
            return query.FirstOrDefault();
        }
        public Produto RetornaProduto_Codigo(string codigo)
        {
            IQueryable<Produto> query = Context.Produtos;

            if (!string.IsNullOrEmpty(codigo))
                query = query.Where(d => d.Codigo == codigo);
            return query.FirstOrDefault();
        }
        public IList<Produto> RetornaProdutos()
        {
            IQueryable<Produto> query = Context.Produtos;

            return query.ToList();
        }

        public void SalvaProduto(Produto itemGravar)
        {
            Produto itemBase = Context.Produtos.Where(f => f.Id == itemGravar.Id).FirstOrDefault();
            if (itemBase == null)
            {
                itemBase = Context.Produtos.Create();
                Context.Entry<Produto>(itemBase).State = System.Data.Entity.EntityState.Added;
            }
            AtualizaPropriedades<Produto>(itemBase, itemGravar);

            Context.SaveChanges();
            itemGravar.Id = itemBase.Id;
        }
        public void ExcluiProduto(Produto itemGravar)
        {
            Produto itemExcluir = Context.Produtos.Where(f => f.Id == itemGravar.Id).FirstOrDefault();

            Context.Entry<Produto>(itemExcluir).State = System.Data.Entity.EntityState.Deleted;
            Context.SaveChanges();
        }
    }
}
