//using Newtonsoft.Json;
//using System;
//using System.Data;
//using System.IO;
//using System.Net.Sockets;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Text;
//using System.Windows.Forms;

//namespace CLIENT
//{
//    public class databaseOutput
//    {
//        DataSet dataSet;
//        string str = " ";
//        public DataSet connectServer()
//        {
//            try
//            {
//                TcpClient tcpClient = new TcpClient("127.0.0.1", 80); // подключение к серверу
//                NetworkStream stream = tcpClient.GetStream();  // создаем поток 


//                // Получение данных от сервера
//                byte[] buffer = new byte[1024];
//                int bytesRead = stream.Read(buffer, 0, buffer.Length);
//                string jsonData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

//                // Десериализация JSON данных
//                var data = JsonConvert.DeserializeObject(jsonData);





//                //string SqlRequest = "SELECT name FROM stock";
//                //byte[] requestByte = Encoding.UTF8.GetBytes(SqlRequest);
//                //stream.Write(requestByte, 0, requestByte.Length);
//                //byte[] bytes = new byte[1024];
//                //stream.Read(bytes, 0, bytes.Length);
//                //BinaryFormatter binaryFormatter = new BinaryFormatter();
//                //dataSet = (DataSet)binaryFormatter.Deserialize(stream); // из потока десериализовали данные в формат DataSet


//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Error: " + ex.Message);
//            }
//            return dataSet;
//        }

        
//    }
//}
