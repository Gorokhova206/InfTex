using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace InfTeh
{
    class MyExtension
    {
        static DataBase_Worker db = new DataBase_Worker();
        public static int addNewextension(string type)//добавление нового расширения
        {
            string insert_query = "insert into extension(type) values('" + type + "')";
            db.execute_query(insert_query);//добавили расширение в базу
            string select_query = "select id from extension where type = '" + type + "'";
            DataTable ext_info = db.select_data(select_query).Tables[0];
            return Convert.ToInt32(ext_info.Rows[0][0]);//возвращаем id под которым оно теперь хранится в базе
        }

        public static string  Existextension(string type)//определение есть ли расширение такого типа
        {
            string select_query = "select id from extension e where e.type ='" + type + "'";
            DataTable ext_info = db.select_data(select_query).Tables[0];
            if (ext_info.Rows.Count > 0)
                return Convert.ToString(ext_info.Rows[0][0]);//возвращаем id расширения 
            else
                return "";//если расширения в базе нет 
        }

        public static DataTable get_extension_image_list()//получение списка изображений расширений
        {
            return db.select_data("select id, image from extension where image is not null order by id").Tables[0];
        }
    }
}
