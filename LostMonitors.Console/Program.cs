using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostMonitors.Core.Services;

namespace LostMonitors.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var players = PlayerService.GetPlayers().OrderBy(x => x.PlayerType.ToString()).ToList();

            if (!args.Any())
            {
                System.Console.WriteLine("Usage: {0} <player 1 name/index> <player 2 name/index>", AppDomain.CurrentDomain.FriendlyName);

                if (players.Any())
                {
                    System.Console.WriteLine("\t Index\t Name");

                    for (var i = 0; i < players.Count(); i++)
                    {
                        System.Console.WriteLine("\t {0}\t {1} ({2})", i, players[i].Name, players[i].PlayerType);
                    }
                }
                else
                {
                    System.Console.WriteLine("Could not find any assemblies containing implementations of LostMonitors.Core.IPlayer, copy assemblies to current folder or set AppSetting \"LostMonitors.AssemblyFolder\".");
                }
                return;
            }

            var player1 = GetPlayerFromArgument(args[0], players);
            var player2 = GetPlayerFromArgument(args[1], players);

            if (player1 == null || player2 == null)
            {
                return;
            }
        }

        private static Player GetPlayerFromArgument(string arg, List<Player> players)
        {
            int player1Index;
            if (int.TryParse(arg, out player1Index))
            {
                if (player1Index >= players.Count)
                {
                    System.Console.Error.WriteLine("Only found {0} players, no player found at index: {1}", players.Count, player1Index);
                    return null;
                }
                return players[player1Index];
            }

            var player = players.FirstOrDefault(x => string.Equals(x.Name, arg, StringComparison.InvariantCultureIgnoreCase)) ?? players.FirstOrDefault(x => string.Equals(x.PlayerType.ToString(), arg, StringComparison.InvariantCultureIgnoreCase));

            if (player == null)
            {
                System.Console.Error.WriteLine("No player found with name or type: {0}", arg);
                return null;
            }

            return player;
        }
    }
}
