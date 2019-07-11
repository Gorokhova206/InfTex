using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace InfTeh
{
    class Folder
    {
        static DataBase_Worker db = new DataBase_Worker();

        public static DataTable add_folder(string name, int parent_id)//добавление новой папки
        {
            string insert_query = "insert into folder(name, parent_id) values('" + name + "'," + parent_id + ")";
            db.execute_query(insert_query);//добавили папку
            string select_query = "select id, 'folder', 1 from folder where name = '" + name + "' and parent_id = " + parent_id;
            DataTable folder_info = db.select_data(select_query).Tables[0];//вернули id под которым она теперь хранится в базе
            return folder_info;
        }

        public static DataTable add_root_folder(string name)//добавление корневой папки
        {
            string insert_query = "insert into folder(name) values('" + name + "')";
            db.execute_query(insert_query);//добавили папку
            string select_query = "select id, 'folder', 1 from folder where name = '" + name + "' ";
            DataTable folder_info = db.select_data(select_query).Tables[0];//вернули id под которым она теперь хранится в базе 
            return folder_info;
        }

        public static void delete_folder(int folder_id)//удаление папки
        {
            string query = "delete from folder where id=" + folder_id;
            db.execute_query(query);
        }

        public static void updeate_folder(int id, string new_name)//переименование папки
        {
            string query = "update folder set name = '" + new_name + "' where id=" + id;
            db.execute_query(query);
        }

        public static DataTable get_folder_list(int folder_id)//получение списка дочерних папок 
        {
            string query = "select fol.id, fol.name, null, e.id, 'folder' from folder fol, extension e where fol.parent_id =" + folder_id + " and e.type = 'folder'";
            return db.select_data(query).Tables[0];
        }

        public static DataTable get_root_folder_list()//получение списка корневых папок
        {
            string query = "select fol.id, fol.name, e.id, 'folder' from folder fol, extension e where parent_id is null and e.type = 'folder'";
            return db.select_data(query).Tables[0];
        }
    }
}
