using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntsTCPServer
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //}
        static TcpConnection connection = new TcpConnection();
        static void Main(string[] args)
        {
            connection.Listen(8000); //starts a TCP listener on port 23

            while (!connection.TcpIsConnected)  //wait until a client connects
            {
                System.Threading.Thread.Sleep(100);
            }

            Console.Write("remote endpoint address: " + connection.RemoteEndpointAddress + Environment.NewLine); //prints the remote endpoint IP to the console

            //bool successfulSendFlag = connection.Send("Hello world!"); //the server sends "Hello World!" to the remote peer and returns if the operation was successful

            //if (successfulSendFlag)
            //{
            //    Console.Write("String successfully sent." + Environment.NewLine);
            //}
            //else
            //{
            //    Console.Write("String NOT successfully sent." + Environment.NewLine);
            //}

            while (true)
            {
                string tempReceiveString = "";

                while (tempReceiveString == "")
                {
                    tempReceiveString = connection.GetReceivedString(); //returns the received string or an empty string, if nothing got received so far
                }

                byte[] tempReceiveByte = Encoding.ASCII.GetBytes(tempReceiveString);
                string tempReceiveHexString = Byte2HexString(tempReceiveByte, " ");

                Console.Write("Received: " + tempReceiveString + Environment.NewLine); //prints the received string to the console
                Console.Write("Received: " + tempReceiveHexString + Environment.NewLine); //prints the received string to the console

                byte[] sendData = new byte[16];
                sendData[0] = 0xa2;
                sendData[1] = 0x0e;
                sendData[2] = 0x01;
                sendData[3] = 0x01;
                sendData[4] = 0xaa;
                connection.Send(sendData);
            }

            Console.ReadLine(); //keeps the console alive
        }

        public static string Byte2HexString(byte[] bytBuf, string strSeparator)
        {

            string strLine = "";
            string strSingle;
            if (bytBuf != null)
            {
                for (int i = 0; i < bytBuf.Length; i++)
                {
                    strSingle = string.Format("{0:X}", bytBuf[i]);
                    if (strSingle.Length == 1) strSingle = "0" + strSingle;
                    if (strLine.Length <= 0)
                    {
                        strLine = strSingle;
                    }
                    else
                    {
                        strLine = strLine + strSeparator + strSingle;
                    }

                }
            }
            return strLine;
        }


    }
}
