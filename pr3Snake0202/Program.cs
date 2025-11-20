using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Commonn;

namespace pr3Snake0202
{
    internal class Program
    {
        public static List<Leaders> Leaders = new List<Leaders>();
        public static List<ViewModelUserSettings> remoteIPAddress = new List<ViewModelUserSettings>();
        public static List<ViewModelGames> viewModelGames = new List<ViewModelGames>();
        private static int localPort = 5001;
        public static int MaxSpeed = 15;
        private static void Send()
        {
            foreach (ViewModelUserSettings User in remoteIPAddress)
            {
                UdpClient sender = new UdpClient();
                IPEndPoint endPoint = new IPEndPoint(
                    IPAddress.Parse(User.IPAddress),
                    int.Parse(User.Port));
                try
                {
                    var playerData = viewModelGames.Find(x => x.IdSnake == User.IdSnake);
                    var otherPlayersData = viewModelGames.FindAll(x => x.IdSnake != User.IdSnake);
                    var gameData = new GameData
                    {
                        PlayerData = playerData,
                        OtherPlayersData = otherPlayersData
                    };
                    byte[] gameDataBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(gameData));
                    sender.Send(gameDataBytes, gameDataBytes.Length, endPoint);
                    Console.WriteLine($"Отправил данные игроку {User.IPAddress}:{User.Port}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Возникло исключение" + ex.ToString() + "\n" + ex.Message);
                }
                finally
                {
                    sender.Close();
                }
            }
        }
        static void Main(string[] args)
        {
        }
    }
}
