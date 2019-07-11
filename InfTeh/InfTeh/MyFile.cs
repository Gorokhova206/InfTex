using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace InfTeh
{
    class MyFile
    {
        static DataBase_Worker db = new DataBase_Worker();

        public static void delete_file(int  file_id)//удаление файла
        {
            string query = "delete from file where id=" + file_id;
            db.execute_query(query); 
        }

        public static void updeate_file(int id, string new_name)//переименование файла
        {
            string query = "update file set name = '" + new_name + "' where id=" + id;
            db.execute_query(query);
        }

        public static DataTable get_file_content(int  file_id)// получение имени файла с расщирением и контента
        {
            string query = "select fl.name || '.' || e.type, content from file fl join extension e on e.id = fl.extension_id where fl.id =" + file_id;
            DataSet content = db.select_data(query);
            return content.Tables[0];
        }

        public static void setup_fite(int file_id, string path)// скачивание файла
        {
            string query = "select fl.id, fl.name, e.type, fl.content from file fl left join extension e on e.id = fl.extension_id where fl.id = " + file_id;
            DataTable file_info = db.select_data(query).Tables[0];
            //string path = @"D:\InfTeh\InfTeh\Dounloads\"+ file_info.Rows[0][1].ToString()+"."+ file_info.Rows[0][2].ToString();
            if (File.Exists(path))
            {
                File.Delete(path);//если этот файл уже есть по указанному пути, то удалить и заменить новым
            }
            using (FileStream fs = File.Create(path))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(file_info.Rows[0][3].ToString());
                fs.Write(info, 0, info.Length);
            }
        }

        public static  void load_file(OpenFileDialog selected_file, int folder_id)// загрузка(добавление) файла
        {
            string file_path;
            string file_content;
            string file_name;
            string file_extension;
            string file_extension_id;

            file_path = selected_file.FileName;//полный путь до загружаемого файла
            file_name = file_path.Substring(file_path.LastIndexOf("\\")+1, file_path.LastIndexOf(".") - 1 - file_path.LastIndexOf("\\") );// имя файла
            file_extension = file_path.Substring(file_path.LastIndexOf(".") + 1);//тип расширение
            var fileStream = selected_file.OpenFile();
            file_extension_id = MyExtension.Existextension(file_extension);//получаем id расширения
            if (file_extension_id == "")//если такого расширения нет в базе
            {
                file_extension_id = Convert.ToString(MyExtension.addNewextension(file_extension));//добавляем его
            }

            using (StreamReader reader = new StreamReader(fileStream))//добавляем файл в базу
            {
                file_content = reader.ReadToEnd();
                file_content = file_content.Replace("'", "''");
                string query = "insert into file(name, description,extension_id,folder_id,content) values('" + file_name+"',null,'"+ file_extension_id + "',"+ folder_id + ",'"+ file_content + "')";
                db.execute_query(query);
            }
        }

        public static DataTable get_file_list(int folder_id)//получение списка дочерних файлов
        {
            string query = "select fl.id, fl.name||'.'||e.type, fl.description, e.id, 'file' from file fl left join extension e on e.id = fl.extension_id where folder_id = " + folder_id;
            return db.select_data(query).Tables[0];
        }
    }
}
