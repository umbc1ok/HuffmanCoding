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
            byte[] bytes = new byte[1048576];
            
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 12345));
            serverSocket.Listen(1);


            Socket clientSocket = serverSocket.Accept();

            
            int bytesRead = clientSocket.Receive(bytes);
            byte[] result = new byte[bytesRead];
            for(int i = 0; i < bytesRead; i++)
            {
                result[i] = bytes[i];
            }
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            serverSocket.Close();
            return result;
        }

        public string ReceiveAndHandleData()
        {
            Huffman f2 = new Huffman();
            byte[] encodedMessageReceived = ReceiveData();
            byte[] SerializedTreeReceived = ReceiveData();
            f2.DeserializeOccurences(SerializedTreeReceived);
            f2.buildATree();
            List<bool> encodedMessageInBools = f2.ConvertBytesToBools(encodedMessageReceived);
            //string result = f2.ConvertBytesToString(encodedMessageReceived);
            string result = f2.Decode(encodedMessageInBools);
            return result;
        }

    }
}
