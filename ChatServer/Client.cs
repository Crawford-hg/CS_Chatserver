using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
namespace ChatServer
{
	public class Client
	{
		TcpClient client;
		public Client(TcpClient client)
		{
			this.client = client;
		}
	}
}

