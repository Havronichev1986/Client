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
    public partial class RegistrationForm : Form
    {
        Point lastPoint = new Point();
        public RegistrationForm()
        {
            InitializeComponent();
            LoginBox.Text = "Введите имя";
            LoginBox.ForeColor = Color.Gray;
            PasswordBox.Text = "Введите пароль";
            PasswordBox.ForeColor = Color.Gray;
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        private void LoginBox_Enter(object sender, EventArgs e)
        {
            if (LoginBox.Text == "Введите имя")
            {
                LoginBox.Text = "";
                LoginBox.ForeColor = Color.Black;
            }
        }
        private void LoginBox_Leave(object sender, EventArgs e)
        {
            if (LoginBox.Text == "")
            {
                LoginBox.Text = "Введите имя";
                LoginBox.ForeColor = Color.Gray;
            }
        }
        private void PasswordBox_Enter(object sender, EventArgs e)
        {
            if (PasswordBox.Text == "Введите пароль")
            {
                PasswordBox.Text = "";
                PasswordBox.ForeColor = Color.Black;
            }
        }
        private void PasswordBox_Leave(object sender, EventArgs e)
        {
            if (PasswordBox.Text == "")
            {
                PasswordBox.Text = "Введите пароль";
                PasswordBox.ForeColor = Color.Gray;
            }
        }
        private async void LoginButton_Click(object sender, EventArgs e)
        {
            if (LoginBox.Text == "Введите имя")
            {
                MessageBox.Show("Введите имя!");
                return;
            }
            if (PasswordBox.Text == "Введите пароль")
            {
                MessageBox.Show("Пароль не введен!");
                return;
            }
            if (await CheckUser())
                return;


            TcpClient tcpClient = new TcpClient("127.0.0.1", 123);
            NetworkStream stream = tcpClient.GetStream();
            string login = LoginBox.Text;
            string password = PasswordBox.Text;
            string str = DateTime.Now.ToString();
            string SqlRequest = $"insert into users(Login, Password, Date_time_registration) values('{login}','{password}','{str}');";
            try
            {
                //отправка запроса серверу
                byte[] Sqlbytes = Encoding.UTF8.GetBytes(SqlRequest);
                await stream.WriteAsync(Sqlbytes, 0, Sqlbytes.Length);
                MessageBox.Show("Пользователь успешно зарегестрирован!");
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
        }
        public async Task<Boolean> CheckUser()
        {
            TcpClient tcpClient = new TcpClient("127.0.0.1", 123);
            NetworkStream stream = tcpClient.GetStream();

            string login = LoginBox.Text;
            string SqlRequest = $"select Login from users where `Login` = '{login}'";

            //отправка запроса серверу
            byte[] Sqlbytes = Encoding.UTF8.GetBytes(SqlRequest);
            await stream.WriteAsync(Sqlbytes, 0, Sqlbytes.Length);
            // Получение данных от сервера
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string jsonData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(jsonData);
            //DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(jsonData);


            if (dataTable.Rows.Count > 0)
            {
                MessageBox.Show("Пользователь с таким Login уже существует!");
                return true;
            }
            else
            {
                return false;
                //MessageBox.Show("Аккаунт успешно создан!");
                //Hide();
                //authorizationForm authorization = new authorizationForm();
                //authorization.Show();
            }

        }
        private void label4_Click(object sender, EventArgs e)
        {
            Hide();
            authorizationForm autorizationForm = new authorizationForm();
            autorizationForm.Show();
        }
    }
}
