using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Drawing;

namespace InfTeh
{
    class File_tree
    {

        public static void getChildNodes(TreeNode node, ImageList tree_images)//получение дочерних узлов дерева
        {
            DataTable folder_list = Folder.get_folder_list(Convert.ToInt32(node.Name));//получаем список дочерних папок текущего узла
            DataTable file_list = MyFile.get_file_list(Convert.ToInt32(node.Name));//получаем список дочерних файлов текущего узла

            if (folder_list.Rows.Count == 0 && file_list.Rows.Count == 0) { return; }//если дочерних объектов нет, выходим

            for (int i = 0; i < folder_list.Rows.Count; ++i)//добавляем в текущий узел дочерние папки 
            {
                TreeNode Childnode = new TreeNode();
                Childnode.Text = folder_list.Rows[i][1].ToString();
                Childnode.Name = folder_list.Rows[i][0].ToString();
                Childnode.Tag = folder_list.Rows[i][4].ToString();//проставляем признак того, что этот узел папка Tag = 'folder'
                if (folder_list.Rows[i][3].ToString() != "" && tree_images.Images.ContainsKey(Convert.ToString(folder_list.Rows[i][3])))/*если пришло значение расширения(в данном случае - folder) 
                и если есть изображение соответствующее этому расширению*/
                    { 
                    Childnode.ImageIndex = Convert.ToInt32(folder_list.Rows[i][3]);//проставляем изображение для невыбранного
                    Childnode.SelectedImageIndex = Convert.ToInt32(folder_list.Rows[i][3]);// и для выбранного состояния
                }

                node.Nodes.Add(Childnode);//добавляем узел
                getChildNodes(Childnode, tree_images);//заполняем его потомков
            }

            for (int i = 0; i < file_list.Rows.Count; ++i)//добавляем в текущий узел дочерние папки 
            {
                TreeNode Childnode = new TreeNode();
                Childnode.Text = file_list.Rows[i][1].ToString();
                Childnode.Name = file_list.Rows[i][0].ToString();
                Childnode.Tag = file_list.Rows[i][4].ToString();//проставляем признак того, что этот узел файл Tag = 'file'
                if (file_list.Rows[i][3].ToString() != "" && tree_images.Images.ContainsKey(Convert.ToString(file_list.Rows[i][3]))) //если пришло значение расширения и если есть изображение соответствующее этому расширению 
                {
                    Childnode.ImageIndex = Convert.ToInt32(file_list.Rows[i][3]);//для невыбранного состояния
                    Childnode.SelectedImageIndex = Convert.ToInt32(file_list.Rows[i][3]);//для выбранного состояния
                }
                if (file_list.Rows[i][2].ToString() != null)//если есть текст подсказки, указываем его
                    Childnode.ToolTipText = file_list.Rows[i][2].ToString();

                node.Nodes.Add(Childnode);
                
            }

        }

        public static void fill_treeView(TreeView tree)//заполнение дерева 
        {

            DataTable root_list = Folder.get_root_folder_list();//получаем корневые узлы
            tree.Nodes.Clear();

            for (int i = 0; i < root_list.Rows.Count; i++)
            {
                TreeNode root = new TreeNode();
                root.Text = root_list.Rows[i][1].ToString();
                root.Name = root_list.Rows[i][0].ToString();
                root.Tag = root_list.Rows[i][3].ToString();//проставляем признак того, что этот узел папка Tag = 'folder'
                root.ImageIndex = Convert.ToInt32(root_list.Rows[i][2]);//указываем изображение узла (невыбранный)
                root.SelectedImageIndex = Convert.ToInt32(root_list.Rows[i][2]);//указываем изображение узла (выбранный)
                tree.Nodes.Add(root);
                getChildNodes(root, tree.ImageList);////заполняем потомков

            }
        }

        


    }
}
