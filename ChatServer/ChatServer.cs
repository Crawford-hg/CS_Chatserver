using System;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
namespace ChatServer
{
	public class ChatServer
	{

		int PORT = 1998;
		//String HOST = "localhost";
		public  HashSet<Client> clients = new HashSet<Client>();
        public static dataHandler dh = new dataHandler();

		public ChatServer()
		{


            TcpListener listener = new TcpListener(IPAddress.Any, PORT);
			listener.Start();
            Console.WriteLine("Server Listening for Connections");

            while (true) {
				TcpClient client = listener.AcceptTcpClient();
				Client cl = new Client(client);
				clients.Add(cl);
				cl.send_message("Welcome to the server");

				ClientHandler ch = new ClientHandler(cl);

				Thread thread = new Thread(()=>ch.handle_client());
				thread.Start();

			}
		}

        public static void Main(string[] Args)
        {
            Console.WriteLine("Starting up Server");
            ChatServer cs = new ChatServer();
			
        }
    }


	
}

