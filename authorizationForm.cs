using Newtonsoft.Json;
using System;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace CLIENT
{
    public partial class authorizationForm : Form
    {
        Point lastPoint = new Point();

        public authorizationForm()
        {
            InitializeComponent();
            this.PasswordBox.AutoSize = false;
            this.PasswordBox.Size = new Size(this.PasswordBox.Size.Width, 35);
        }
        private void authorizationForm_Load(object sender, EventArgs e)
        {

        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - lastPoint.X;
                Top += e.Y - lastPoint.Y;
            }
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        private async void LoginButton_Click(object sender, EventArgs e)
        {
            TcpClient tcpClient = new TcpClient("127.0.0.1", 123);
            NetworkStream stream = tcpClient.GetStream();
            string login = LoginBox.Text;
            string password = PasswordBox.Text;
            string SqlRequest = $"select * from users where `Login` = '{login}' and `Password` = '{password}'";
            try
            {
                //отправка запроса серверу
                byte[] Sqlbytes = Encoding.UTF8.GetBytes(SqlRequest);
                await stream.WriteAsync(Sqlbytes, 0, Sqlbytes.Length);
                // Получение данных от сервера
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string jsonData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(jsonData);


                if (dataTable.Rows.Count > 0)
                {
                    Hide();
                    Form1 Form = new Form1();
                    Form.Show();
                }
                else
                {
                    MessageBox.Show("Такого пользователя не существует или не верно введены Login или Password");
                }
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
        private void label4_Click(object sender, EventArgs e) // переход в окно регистрации
        {
            Hide();
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.Show();
        }
    }
}
