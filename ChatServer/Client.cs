
using System.Net.Sockets;
using System.Text;

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

		public void set_name(string n) {
			this.name = n;
		}

		public string get_name() {
			return name;
		}

	}

}

