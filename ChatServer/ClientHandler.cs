using System;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace ChatServer
{
	public class ClientHandler
	{
		Client client;
		public ClientHandler(Client client)
		{
            this.client = client;
        }

        public void handle_client()
        {
            bool logged_in = false;

            while (true)
            {
                NetworkStream client_stream = client.get_Stream();
                StreamReader client_reader = new StreamReader(client_stream);

                string msg;
                while ((msg = client_reader.ReadLine()) != null)
                {
                    if (!logged_in)
                    {
                        if (msg.ToUpper().Equals("SIGN UP"))
                        {
                            _sign_up(client_reader, this.client);
                            logged_in = true;
                        }

                        if (msg.ToUpper().Equals("LOG IN")) { _login(client_reader); }
                    }
                    Console.WriteLine("Message recieved " + msg);
                }
            }
        }

        public void _sign_up(StreamReader client_reader, Client client)
        {
            client.send_message("please enter a username \n");
            string user = "";
            string pass = "";

            while (true)
            {
                string msg;

                while ((msg = client_reader.ReadLine()) != null)
                {
                    if (user.Equals(""))
                    {
                        if (!string.IsNullOrEmpty(msg))
                        {
                            user = msg;
                            client.send_message("please enter a password \n");
                        }
                    }
                    else if (pass.Equals("") && !string.IsNullOrEmpty(msg))
                    {
                        pass = msg;
                    }

                    if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass))
                    {
                        try
                        {
                            ChatServer.dh.insert(user, pass);
                        }
                        catch(Exception e)
                        {
                            //client.send_message("user already exists, please pick a new name or sign in \n");
                            client.send_message("error " + e);
                            user = "";
                        }
                        if (!user.Equals(""))
                        {
                            Console.WriteLine("User and password set successfully \n");
                            return;
                        }
                    }
                }
            }
        }

        public void _login(StreamReader client_reader)
        {
            client.send_message("please enter your username \n");
            string user = "";
            string pass = "";

            while (true)
            {
                string msg;

                while ((msg = client_reader.ReadLine()) != null)
                {
                    if (user.Equals(""))
                    {
                        if (!string.IsNullOrEmpty(msg))
                        {
                            user = msg;
                            client.send_message("please enter your password \n");
                        }
                    }
                    else if (pass.Equals("") && !string.IsNullOrEmpty(msg))
                    {
                        pass = msg;
                    }

                    if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass))
                    {
                        try
                        {
                            ChatServer.dh.select(user, pass);
                        }
                        catch (Exception e) { this.client.send_message("An error occured on login, please try again"); }
                    }
                }
            }
        }


    }


}

