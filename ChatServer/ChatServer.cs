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

		public ChatServer()
		{
			while (true) {
				TcpListener listener = new TcpListener(IPAddress.Any, PORT);
			}
		}
	}
}

