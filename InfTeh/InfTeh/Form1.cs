using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using System.IO;


namespace InfTeh
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ImageList myImageList = new ImageList();
            DataTable extension_image_list = new DataTable();
            try
            {
                extension_image_list = MyExtension.get_extension_image_list();//получаем список изображений расширений
                for (int i = 0; i < extension_image_list.Rows.Count; i++)
                {
                    myImageList.Images.Add(extension_image_list.Rows[i][0].ToString(), Image.FromFile(Directory.GetCurrentDirectory()+extension_image_list.Rows[i][1].ToString()));
                }
                treeView1.ImageList = myImageList;//закрепляем полученный список за деревом
                File_tree.fill_treeView(treeView1);//заполняем дерево узлами
            }
            catch(NpgsqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            } 
        }

        private void button2_Click(object sender, EventArgs e)//удаление папки
        {
            if (treeView1.SelectedNode.Tag.ToString() == "folder")//если выбранный объект папка
            {
                TreeNode selected_node = treeView1.SelectedNode;//определяем текущий узел
                
                TreeNode parent = treeView1.SelectedNode.Parent;//определяем родительский узел
                try
                {
                    Folder.delete_folder(Convert.ToInt32(selected_node.Name));//удаляем текущий узел
                    if (treeView1.SelectedNode.Index != 0)//если удаленный узел не корневой
                    {
                        parent.Nodes.Clear();
                        File_tree.getChildNodes(parent, treeView1.ImageList);//пересобираем дочерние узлы
                    }
                    else
                    {
                        File_tree.fill_treeView(treeView1);//иначе пересобираем корневые узлы
                    }
                }

                catch (NpgsqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!");
                }
            }
            else
                MessageBox.Show("Выбранный объект не является папкой!", "Внимание!");
        }

        private void button5_Click(object sender, EventArgs e)//удаление файла
        {
            if (treeView1.SelectedNode.Tag.ToString() == "file")//если выбранный узел файл
            {
                TreeNode selected_node = treeView1.SelectedNode;//определяем текущий узел
                TreeNode parent = treeView1.SelectedNode.Parent;//определяем родительский узел
                tabControl1.TabPages[treeView1.SelectedNode.Name].Dispose();//закрываем открытую вкладку с содержимым файла
                try
                {
                    MyFile.delete_file(Convert.ToInt32(selected_node.Name));//удаляем файл
                    parent.Nodes.Clear();
                    File_tree.getChildNodes(parent, treeView1.ImageList);//пересобираем дочерние узлы родителя
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!");
                }


            }
            else
                MessageBox.Show("Выбранный объект не является файлом!", "Внимание!");
        }

        private void button7_Click(object sender, EventArgs e)//переименование папки или файла
        {
            treeView1.LabelEdit = true;//разрешаем редактирование дерева
            treeView1.SelectedNode.BeginEdit(); //начало редактирования названия узла 
        }

        private void treeView1_KeyUp(object sender, KeyEventArgs e)//нажатие на кнопку Enter при завершении переименования
        {
            if (e.KeyCode == Keys.Enter)//если нажата клавиша  Enter
            {
                try
                {
                    if (treeView1.SelectedNode.Tag.ToString() == "folder")
                        Folder.updeate_folder(Convert.ToInt32(treeView1.SelectedNode.Name), treeView1.SelectedNode.Text);//обновляем имя папки в базе
                    else
                        MyFile.updeate_file(Convert.ToInt32(treeView1.SelectedNode.Name), treeView1.SelectedNode.Text);//обновляем имя файла в базе
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!");
                }
                treeView1.LabelEdit = false;//отключаем редактирование дерева
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)//выбор узла дерева
        {
            if (treeView1.SelectedNode.Tag.ToString() == "file")//если выбрали файл 
            {
                TreeNode selected_node = treeView1.SelectedNode;//определяем выбранный узел
                try
                {
                    DataTable file_info = MyFile.get_file_content(Convert.ToInt32(selected_node.Name));//получаем информацию о файле
                    if (!tabControl1.TabPages.ContainsKey(Convert.ToString(selected_node.Name)))//если у нас еще не открыта вкладка с этим файлом
                    {
                        tabControl1.TabPages.Add(Convert.ToString(selected_node.Name), file_info.Rows[0][0].ToString());//добавляем вкладку
                        TextBox content = new TextBox();
                        content.Multiline = true;//разрешаем многострочность
                        content.WordWrap = true;
                        content.ScrollBars = ScrollBars.Vertical;//устанавливаем режим скроллинга
                        content.Text = file_info.Rows[0][1].ToString();//помещаем содержимое файла
                        content.Height = tabControl1.TabPages[0].Height;//устанавливаем высоту
                        content.Width = tabControl1.TabPages[0].Width;//и ширину как у вкладки
                        tabControl1.TabPages[Convert.ToString(selected_node.Name)].Controls.Add(content);//добавляем контент на вкладку
                        tabControl1.Refresh();
                    }
                    tabControl1.SelectedTab = tabControl1.TabPages[Convert.ToString(selected_node.Name)];
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)//добавляем новую папку
        {
            TreeNode created_node = new TreeNode();
            DataTable new_folder_info = new DataTable();
            try
            {
                if (treeView1.Nodes.Count > 0)//если уже есть корневой узел
                {

                    if (treeView1.SelectedNode.Tag.ToString() == "folder")//если выбранный узел папка
                    {
                        TreeNode selected_node = treeView1.SelectedNode;//определяем текущий узел
                        created_node = selected_node.Nodes.Add("Новая папка");//создаем новую папку
                        selected_node.Expand();//раскрываем текущий узел
                        new_folder_info = Folder.add_folder(created_node.Text, Convert.ToInt32(selected_node.Name));//получаем информацию о новой папки
                    }
                    else
                    {
                        MessageBox.Show("Выбранный объект не является папкой, в него нельзя поместить дочернюю папку!", "Внимание!");
                        return;
                    }

                }
                else
                {
                    created_node = treeView1.Nodes.Add("Новая папка");//если корневого узла нет, то создаем его
                    new_folder_info = Folder.add_root_folder(created_node.Text);
                }

                created_node.Name = new_folder_info.Rows[0][0].ToString();//проставляем дефолтное имя
                created_node.Tag = new_folder_info.Rows[0][1];// признак того что это папка
                created_node.ImageKey = new_folder_info.Rows[0][2].ToString();//указываем иконку (невыбранный)
                created_node.SelectedImageKey = new_folder_info.Rows[0][2].ToString();//указываем иконку (выбранный)
                treeView1.LabelEdit = true;//включаем редактирование дерева
                treeView1.SelectedNode = created_node;//делаем новый узел текущим
                created_node.BeginEdit();//начинаем редактирование узла
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }

        private void button4_Click(object sender, EventArgs e)//сохранение файла
        {
            if (treeView1.SelectedNode.Tag.ToString() == "file")//если выбранный узел файл
            {
                SaveFileDialog save_dialog = new SaveFileDialog();//открываем диалоговое окно 
                save_dialog.Filter = "All files (*.*)|*.*";//расширения файлов
                save_dialog.RestoreDirectory = true;//сохраняем ранее выбранную директорию
                save_dialog.FileName = treeView1.SelectedNode.Text;//получаем путь сохранения
                if (save_dialog.ShowDialog() == DialogResult.OK)
                {
                    string path = save_dialog.FileName;
                    try
                    {
                        MyFile.setup_fite(Convert.ToInt32(treeView1.SelectedNode.Name), path);//скачиываем файл
                        MessageBox.Show("Файл сохранен по адресу '" + path + "'", "Внимание!");
                    }
                    catch (NpgsqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Скачивание для данного типа объектов не предусмотрено!", "Внимание!");
            }
            
        }

        private void button3_Click(object sender, EventArgs e)//загрузка файла
        {
            if (treeView1.SelectedNode.Tag.ToString() == "folder")//если выбранный узел файл 
            {
                OpenFileDialog select_dialog = new OpenFileDialog();//открываем диалоговое окно
                select_dialog.RestoreDirectory = true;//сохраняем ранее выбранную директорию
                TreeNode parent = treeView1.SelectedNode;//определяем узел в который будет добавляться новый файл

                if (select_dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        MyFile.load_file(select_dialog, Convert.ToInt32(parent.Name));//загружаем файл
                        parent.Nodes.Clear();
                        File_tree.getChildNodes(parent, treeView1.ImageList);//пересобираем дочерние узлы текущего узла
                        parent.Expand();//раскрываем текущий узел
                    }
                    catch (NpgsqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выбранный файл не является папкой, в него нельзя поместить другой файл! Выберите папку в которую необходимо загрузить файл.", "Внимание!");
            }


        }


    }
}
