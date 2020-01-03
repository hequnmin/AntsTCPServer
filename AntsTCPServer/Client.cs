using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace AntsTCPServer
{
    // https://stackoverflow.com/questions/41718342/async-socket-client-using-tcpclient-class-c-sharp
    public class Client
    {
        private TcpClient tcpClient;

        public void Initialize(string ip, int port)
        {
            try
            {
                tcpClient = new TcpClient(ip, port);

                if (tcpClient.Connected)
                    Console.WriteLine("Connected to: {0}:{1}", ip, port);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Initialize(ip, port);
            }
        }

        public void BeginRead()
        {
            var buffer = new byte[4096];
            var ns = tcpClient.GetStream();
            ns.BeginRead(buffer, 0, buffer.Length, EndRead, buffer);
        }

        public void EndRead(IAsyncResult result)
        {
            var buffer = (byte[])result.AsyncState;
            var ns = tcpClient.GetStream();
            var bytesAvailable = ns.EndRead(result);

            Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, bytesAvailable));
            BeginRead();
        }

        public void BeginSend(string xml)
        {
            var bytes = Encoding.ASCII.GetBytes(xml);
            var ns = tcpClient.GetStream();
            ns.BeginWrite(bytes, 0, bytes.Length, EndSend, bytes);
        }

        public void BeginSend(byte[] sendData)
        {
            var ns = tcpClient.GetStream();
            ns.BeginWrite(sendData, 0, sendData.Length, EndSend, sendData);
        }

        public void EndSend(IAsyncResult result)
        {
            var bytes = (byte[])result.AsyncState;
            Console.WriteLine("Sent  {0} bytes to server.", bytes.Length);
            Console.WriteLine("Sent: {0}", Encoding.ASCII.GetString(bytes));
        }
    }

}
