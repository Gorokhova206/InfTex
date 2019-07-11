using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data;

namespace InfTeh
{
    class DataBase_Worker
    {
        string connString = "Server=  localhost; Port=5432; User Id = postgres; Password = admin; Database = InfTeh";

        public void execute_query(string query)//исполнение запросов на обновление и удаление данных
        {
            NpgsqlConnection con = new NpgsqlConnection(connString);
            con.Open();
            NpgsqlCommand comm = new NpgsqlCommand(query, con);
            comm.ExecuteScalar();
            con.Close();
        }

        public DataSet select_data(string query)//выборка данных
        {
            DataSet ds = new DataSet();
            NpgsqlConnection con = new NpgsqlConnection(connString);
            con.Open();
            NpgsqlCommand comm = new NpgsqlCommand(query, con);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(comm);
            da.Fill(ds);
            da.Dispose();
            con.Close();
            return ds;
        }
    }
}
