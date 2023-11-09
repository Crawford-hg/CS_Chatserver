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
		StreamWriter sw;
		String name;
		public Client(TcpClient client)
		{
			this.client = client;
			this.sw = new StreamWriter(client.GetStream(),Encoding.ASCII);
			Console.WriteLine("New connection added");
		}



		public void send_message(String msg) {
			sw.Write(msg);
			sw.Flush();
		}


		public NetworkStream get_Stream() {
			return client.GetStream();
		}

		public void set_name(string name) {
			this.name = name;
		}
	}

}

