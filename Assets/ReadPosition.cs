using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

class ReadPosition
{
    public static void Start()
    {
        TcpClient client;
        TcpListener server = null;
        try
        {
            Int32 port = 32779;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            server = new TcpListener(localAddr, port);

            server.Start();

            Byte[] bytes = new Byte[256];

            String data = null;

            client = server.AcceptTcpClient();
            while (true)
            {
                data = null;
                NetworkStream stream = client.GetStream();
                int i;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    SetPosition.instance.Deserialize(data);
                }
            }
        }
        catch (SocketException)
        {
        }
        finally
        {
            server.Stop();
        }
    }
}