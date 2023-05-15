using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;


namespace ClassAssignment___Week_12___JESSICA___T
{
    public partial class Form1 : Form
    {
        MySqlConnection conn;
        MySqlCommand sqlCommand;
        MySqlDataAdapter sqlAdapter;
        MySqlDataReader sqlReader;
        string sqlQuery;
        DataTable dtTeamName = new DataTable();
        DataTable dtNationality = new DataTable();
        DataTable dtposition = new DataTable();
        DataTable dtPlayer = new DataTable();
        DataTable dtManager = new DataTable();
 
        public Form1()
        {
            string connection = "server=localhost;uid=root;pwd=Njmborahae11!;database=premier_league";
            conn = new MySqlConnection(connection);

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //COMBOBOX TEAMS (ADD PLAYER)
            sqlQuery = "SELECT team_name, team_id FROM team;";
            sqlCommand = new MySqlCommand(sqlQuery, conn);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtTeamName);
            comboBox_Teams.DataSource = dtTeamName;
            comboBox_Teams.DisplayMember = "team_name";
            comboBox_Teams.ValueMember = "team_id";

            //COMBO BOX TEAMS (CHANGE MANAGER)
            dtTeamName = new DataTable();
            sqlQuery = "SELECT team_name, team_id FROM team;";
            sqlCommand = new MySqlCommand(sqlQuery, conn);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtTeamName);
            comboBox_Teams2.DataSource = dtTeamName;
            comboBox_Teams2.DisplayMember = "team_name";
            comboBox_Teams2.ValueMember = "team_id";

            //COMBO BOX TEAMS (REMOVE PLAYER)
            dtTeamName = new DataTable();
            sqlQuery = "SELECT team_name, team_id FROM team;";
            sqlCommand = new MySqlCommand(sqlQuery, conn);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtTeamName);
            comboBox_Teams3.DataSource = dtTeamName;
            comboBox_Teams3.DisplayMember = "team_name";
            comboBox_Teams3.ValueMember = "team_id";

            //COMBO BOX NATIONALITY (ADD PLAYER)
            sqlQuery = "SELECT nation, nationality_id FROM nationality;";
            sqlCommand = new MySqlCommand(sqlQuery, conn);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtNationality);
            comboBox_Nationality.DataSource = dtNationality;
            comboBox_Nationality.DisplayMember = "nation";
            comboBox_Nationality.ValueMember = "nationality_id";

            //COMBO BOX PLAYING POS (ADD PLAYER)
            sqlQuery = "SELECT playing_pos FROM player GROUP BY playing_pos;";
            sqlCommand = new MySqlCommand(sqlQuery, conn);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtposition);
            comboBox_Position.DataSource = dtposition;
            comboBox_Position.ValueMember = "playing_pos";
        }

        //ADD PLAYER
        private void button_Add_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox_Height.Text) || !string.IsNullOrEmpty(textBox_IDPlayer.Text)
                || !string.IsNullOrEmpty(textBox_NamePlayer.Text) || !string.IsNullOrEmpty(textBox_NumberPlayer.Text)
                || !string.IsNullOrEmpty(textBox_Weight.Text))
            {
                string ID = textBox_IDPlayer.Text;
                string Name = textBox_NamePlayer.Text;
                string Height = textBox_Height.Text;
                string Weight = textBox_Weight.Text;
                string Number = textBox_NumberPlayer.Text;
                string MysQlCommand = $"insert into player value ('{ID}','{Number}','{Name}', '{comboBox_Nationality.SelectedValue}', '{comboBox_Position.SelectedValue}'," +
                    $"'{Height}', '{Weight}', '{dateTimePicker_Birthdate.Text}', '{comboBox_Teams.SelectedValue}', 1, 0)";
                try
                {
                    conn.Open();
                    sqlCommand = new MySqlCommand(MysQlCommand, conn);
                    sqlReader = sqlCommand.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Belum lengkap kak");
            }
        }

        //EDIT MANAGER
        private void updateDGVManager()
        {
            //UPDATE DGV 1
            dtManager = new DataTable();
            sqlQuery = "SELECT m.manager_id, m.manager_name, n.nation AS nation, m.birthdate FROM manager m, team t, nationality n WHERE m.manager_id = t.manager_id\r\nAND m.nationality_id = n.nationality_id AND t.team_id = '" + comboBox_Teams2.SelectedValue + "';";
            sqlCommand = new MySqlCommand(sqlQuery, conn);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtManager);
            dataGridView1.DataSource = dtManager;

            //UPDATE DGV 2
            dtManager = new DataTable();
            sqlQuery = "SELECT manager_id, manager_name, n.nation, m.birthdate  FROM manager m, nationality n WHERE m.nationality_id = n.nationality_id AND m.working = 0;";
            sqlCommand = new MySqlCommand(sqlQuery, conn);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtManager);
            dataGridView2.DataSource = dtManager;
        }
        private void comboBox_Teams2_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDGVManager();
        }
        private void button1_Click(object sender, EventArgs e) //BUTTON UPDATE MANAGER
        {
            //GANTI TEAM ID NYA MANAGER
            string id = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            string MysQlCommand = $"update team set manager_id = '" + id + "' WHERE team_id = '" + comboBox_Teams2.SelectedValue + "';";
            try
            {
                conn.Open();
                sqlCommand = new MySqlCommand(MysQlCommand, conn);
                sqlReader = sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            //UPDATE WORKING MANAGER JADI 0
            string id2 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string MysQlCommand2 = $"update manager set working = 0 WHERE manager_id = '" + id2 + "';";
            try
            {
                conn.Open();
                sqlCommand = new MySqlCommand(MysQlCommand2, conn);
                sqlReader = sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            //UPDATE MANAGER WORKING JADI 1
            string MysQlCommand3 = $"update manager set working = 1 WHERE manager_id =  '" + id + "';";
            try
            {
                conn.Open();
                sqlCommand = new MySqlCommand(MysQlCommand3, conn);
                sqlReader = sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
                updateDGVManager(); //UPDATEEEE
            }
        }

        //REMOVE PLAYER
        private void comboBox_Teams3_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDGVRemovePlayer();
        }

        private void updateDGVRemovePlayer()
        {
            dtPlayer = new DataTable();
            sqlQuery = "select p.player_id, p.team_number, p.player_name, n.nation, p.playing_pos, p.height, p.weight, p.birthdate, t.team_name FROM player p, nationality n, team t \r\nWHERE p.nationality_id = n.nationality_id AND t.team_id = p.team_id AND p.team_id = '" + comboBox_Teams3.SelectedValue + "' AND p.status = 1;";
            sqlCommand = new MySqlCommand(sqlQuery, conn);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtPlayer);
            dataGridView3.DataSource = dtPlayer;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView3.Rows.Count > 12)
            {
                string id = dataGridView3.CurrentRow.Cells[0].Value.ToString(); 
                string MysQlCommand = $"update player set status = 0 where player_id = '{id}'";
                try
                {
                    conn.Open();
                    sqlCommand = new MySqlCommand(MysQlCommand, conn);
                    sqlReader = sqlCommand.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                    updateDGVRemovePlayer();
                }
            }
            else
            {
                MessageBox.Show("Gaboleh kurang dari 11");
            }
        }
    }
}
