using Dapper;
using System.Collections.Generic;
using System.Data;

namespace JLP.DB
{
    public interface IProductDAL
    {
        int Create(Product product);

        bool Update(Product product);

        bool Delete(int id);

        Product Get(int id);

        IEnumerable<Product> Query(int pageIndex, int pageSize);
    }

    public class ProductDAL : IProductDAL
    {
        private IDbConnection _dbConnection;

        public ProductDAL(IDbConnection dbConnection)
        {
            this._dbConnection = dbConnection;
        }


        public int Create(Product product)
        {
            if (this._dbConnection.State != ConnectionState.Open)
                this._dbConnection.Open();

            return this._dbConnection.Insert(product).Value;
        }

        public bool Delete(int id)
        {
            if (this._dbConnection.State != ConnectionState.Open)
                this._dbConnection.Open();
            return this._dbConnection.Delete<Product>(id) > 0;
        }

        public Product Get(int id)
        {
            if (this._dbConnection.State != ConnectionState.Open)
                this._dbConnection.Open();
            return this._dbConnection.Get<Product>(id);
        }

        public IEnumerable<Product> Query(int pageIndex, int pageSize)
        {
            pageIndex = pageIndex - 1;
            if (this._dbConnection.State != ConnectionState.Open)
                this._dbConnection.Open();
            string sql = $"SELECT * FROM `product` ORDER BY ID DESC LIMIT @pageIndex,@pageSize";
            return this._dbConnection.Query<Product>(sql, new { pageIndex=pageIndex, pageSize=pageSize });
        }

        public bool Update(Product product)
        {
            if (this._dbConnection.State != ConnectionState.Open)
                this._dbConnection.Open();
            return this._dbConnection.Update<Product>(product) > 0;
        }
    }
}
