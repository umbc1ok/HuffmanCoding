using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Client
    {
        public static void SendData(string address, byte[] bytes)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(address, 12345);

            
            clientSocket.Send(bytes);

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        public static void HandleAndSendData(string message,string IPAddress)
        {
            Huffman f = new Huffman();
            List<bool> temp = f.EncodeAString(message);
            byte[] encodedMessage = f.ConvertBoolsToBytes(temp);
            SendData(IPAddress, encodedMessage);
            byte[] SerializedTree = f.SerializeOccurences();
            SendData(IPAddress, SerializedTree);
        }
    }
}
