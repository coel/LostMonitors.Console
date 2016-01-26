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
            if (!args.Any())
            {
                System.Console.WriteLine("Usage: {0} <player 1 name/index> <player 2 name/index>", AppDomain.CurrentDomain.FriendlyName);
                var players = PlayerService.GetPlayers().OrderBy(x => x.PlayerType.ToString()).ToList();

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
            }
        }
    }
}
