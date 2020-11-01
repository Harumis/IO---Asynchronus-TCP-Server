using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ServerLib
{
    internal class LoginSystem
    {
        public static int _bufferSize = 2000;
        public static byte[] buffer = new byte[_bufferSize];
        public static string enterLogin = "Enter login: \n \r";
        public static string waitForUser = "Someone is trying to log in. You have to open app again! \n \r";
        public static string newAccount = "You created new account: \n \r";
        public static string userExists = "This user, already exists: \n \r";
        public static string enterNewLogin = "Enter new login: \n \r";
        public static string enterNewPassword = "Enter new password: \n \r";
        public static string enterPassword = "Enter password: \n \r";
        public static string adminMessage = "Hello Admin.\n \r Here you can add new user. \n \r";
        public static string error = "Wrong login or password.\n \r Try again. \n\r";
        public static string login;
        public static string password;
        public static int receivedDataLenght;
        public static int userTryToLogin = 0;
        public static int userAlreadyLog = 0;

        public static void loginSystem(NetworkStream stream)
        {
            userAlreadyLog = 0;
            if (userTryToLogin == 0)
            {
                while (userTryToLogin == 0 && userAlreadyLog == 0)
                {
                    stream.Write(Encoding.ASCII.GetBytes(enterLogin), 0, enterLogin.Length);
                    do
                    {
                        receivedDataLenght = stream.Read(buffer, 0, _bufferSize);
                    } while ((login = Encoding.ASCII.GetString(buffer, 0, receivedDataLenght)) == "\r\n");

                    userTryToLogin = 1;

                    stream.Write(Encoding.ASCII.GetBytes(enterPassword), 0, enterPassword.Length);
                    do
                    {
                        receivedDataLenght = stream.Read(buffer, 0, _bufferSize);
                    } while ((password = Encoding.ASCII.GetString(buffer, 0, receivedDataLenght)) == "\r\n");
                    if (password[password.Length - 1] == '\n') password = password.Substring(0, password.Length - 2);
                    if (login[login.Length - 1] == '\n') login = login.Substring(0, login.Length - 2);
                    checkAccount(login, password, stream);
                    userTryToLogin = 0;
                }
            }
            else stream.Write(Encoding.ASCII.GetBytes(waitForUser), 0, waitForUser.Length);
        }

        public static void adminSystem(NetworkStream stream)
        {
            stream.Write(Encoding.ASCII.GetBytes(adminMessage), 0, adminMessage.Length);

            stream.Write(Encoding.ASCII.GetBytes(enterNewLogin), 0, enterNewLogin.Length);
            do
            {
                receivedDataLenght = stream.Read(buffer, 0, _bufferSize);
            } while ((login = Encoding.ASCII.GetString(buffer, 0, receivedDataLenght)) == "\r\n");

            stream.Write(Encoding.ASCII.GetBytes(enterNewPassword), 0, enterNewPassword.Length);

            do
            {
                receivedDataLenght = stream.Read(buffer, 0, _bufferSize);
            } while ((password = Encoding.ASCII.GetString(buffer, 0, receivedDataLenght)) == "\r\n");

            string user = login + ":" + password;
            stream.Write(Encoding.ASCII.GetBytes(newAccount), 0, newAccount.Length);
            StreamWriter file = File.AppendText(@"C:\Users\R2R2\source\repos\AsyncTcp\users.txt");
            file.WriteLine(user);
            file.Close();
            adminSystem(stream);
        }

        public static void checkAccount(string login, string password, NetworkStream stream)
        {
            int flag = 0;
            string[] users = System.IO.File.ReadAllLines(@"C:\Users\R2R2\source\repos\AsyncTcp\users.txt");
            string user = login + ":" + password;
            if (login == "admin" & password == "admin")
            {
                Console.WriteLine("User : " + login + " connected to server! ");
                adminSystem(stream);
                userAlreadyLog = 1;
            }
            else
            {
                foreach (string userCheck in users)
                {
                    if (userCheck == user)
                    {
                        string logIn = "Hello: " + login + " in server \n \r";
                        stream.Write(Encoding.ASCII.GetBytes(logIn), 0, logIn.Length);
                        flag = 1;
                        Console.WriteLine("User : " + login + " connected to server! ");
                        userAlreadyLog = 1;
                        break;
                    }
                }
                if (flag == 0)
                {
                    stream.Write(Encoding.ASCII.GetBytes(error), 0, error.Length);
                    userAlreadyLog = 0;
                    loginSystem(stream);
                }
            }
        }
    }
}