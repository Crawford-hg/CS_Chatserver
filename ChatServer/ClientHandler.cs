using System.Net.Sockets;

namespace ChatServer
{
	public class ClientHandler
	{
		Client client;
        ChatServer cs;
        string name;
        bool running = true;
		public ClientHandler(Client client)
		{
            this.client = client;
        }

        public void handle_client()
        {
            bool logged_in = false;

            while (running == true)
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
                            client.set_name(name);
                        }

                        else if (msg.ToUpper().Equals("LOG IN"))
                        {
                            _login(client_reader);
                            logged_in = true;
                            client.set_name(name);
                        }
                        else
                        {
                            this.client.send_message("please log in or sign up to send messages \n");
                        }
                    }
                    else if (msg[0] == '/') {
                        _handle_command(msg);
                    }
                    else
                    {
                foreach (Client cl in cs.get_clients())
                        {
                            if (cl != client)
                                cl.send_message(this.name + ": " + msg + "\n");
                        }
                    }
                    Console.WriteLine("Message recieved " + msg +"\n");
                    if (running == false)
                    {
                        client.send_message("\n ---------------------------------- \n disconnecting from server \n ----------------------------------");
                        return;
                    }
                }
                if (running == false)
                {
                    client.send_message("\n ---------------------------------- \n disconnecting from server \n ----------------------------------");
                    return;
                }

            }
        }

        public void quit() {
            cs.remove_client(client);
            this.running = false;
            //this.cs = null;
            //this.client = null;
        }

        public void _handle_command(String msg){
            switch (msg) {
                case "/quit":
                    quit();
                    break;
            }

            if (msg.Contains("/direct")){
                string[] tokens = msg.Split(' ');
                string target = tokens[1];
                Client recipient = null;
                foreach (Client cl in cs.get_clients()) {
                    if (cl.get_name().Equals(target)) {
                        recipient = cl;
                        break;
                    }
                }
                string outMsg = string.Join(" ", tokens, 1, tokens.Length -1);
                if(recipient != null)
                    recipient.send_message("DIRECT " + name + ": "+outMsg + "\n");

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
                            this.name = user;
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
                            this.name = user;
                            Console.WriteLine("Login successfull");
                            return;
                        }
                        catch (Exception e) { this.client.send_message("An error occured on login, please try again"); }
                    }
                }
            }
        }


        public void set_chat_server(ChatServer cs) {
            this.cs = cs;
        }


    }


}

