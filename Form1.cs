using Newtonsoft.Json;
using System;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLIENT
{
    public partial class Form1 : Form
    {
        Point lastPoint = new Point();

        public Form1()
        {
            InitializeComponent();
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            string SqlRequest = "SELECT * FROM stock";
            await DatabaseOutput(SqlRequest);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            string str1 = car_brandBox.Text;
            string str2 = car_numberBox.Text;
            string str3 = Client_full_nameBox.Text;
            string str4 = phone_numberBox.Text;
            string str5 = DateTime.Now.ToString();
            string SqlRequest = $"insert into stock(NameAuto, NumberAuto, Client_full_name, Phone_number, Date_time) values('{str1}','{str2}','{str3}','{str4}','{str5}'); SELECT * FROM stock;";

            await DatabaseOutput(SqlRequest);

            car_brandBox.Clear();
            car_numberBox.Clear();
            Client_full_nameBox.Clear();
            phone_numberBox.Clear();
        }
        private async void button4_Click(object sender, EventArgs e)
        {
            string str2 = car_numberBoxDelete.Text;
            string SqlRequest = $"delete from stock where NumberAuto = '{str2}'; SELECT * FROM stock;";

            await DatabaseOutput(SqlRequest);

            car_numberBoxDelete.Clear();
        }
        public async Task<DataTable> DatabaseOutput(string SqlRequest)
        {
            DataTable dataTable = new DataTable();

            TcpClient tcpClient = new TcpClient("127.0.0.1", 123); // подключение к серверу
            NetworkStream stream = tcpClient.GetStream();
            try
            {
                //отправка запроса серверу
                byte[] bytes = new byte[1024];
                bytes = Encoding.UTF8.GetBytes(SqlRequest);
                await stream.WriteAsync(bytes, 0, bytes.Length);
                // Получение данных от сервера
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string jsonData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                dataTable = JsonConvert.DeserializeObject<DataTable>(jsonData);

                #region наведение красоты
                // Вывод 
                dataGridView1.RowHeadersVisible = false;// столбец слева
                dataGridView1.AllowUserToAddRows = false;// строка снизу
                dataGridView1.DataSource = dataTable;
                dataGridView1.Columns["Id"].Visible = false; // столбец с id  
                // авторазмер таблицы под содержимое
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                tcpClient.Dispose();
                tcpClient.Close();
                stream.Dispose();
                stream.Close();
            }
            return dataTable;
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - lastPoint.X;
                Top += e.Y - lastPoint.Y;
            }
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
    }
}
