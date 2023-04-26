using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Logic
{
    public class Server
    {
        public static byte[] ReceiveData()
        {
            byte[] bytes = new byte[1024];
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 12345));
            serverSocket.Listen(1);


            Socket clientSocket = serverSocket.Accept();

            
            int bytesRead = clientSocket.Receive(bytes);


            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            serverSocket.Close();
            return bytes;
        }
    }
}
