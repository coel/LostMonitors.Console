using System;
using System.Collections.Generic;
using System.Linq;
using LostMonitors.Core;
using LostMonitors.Core.Engine;
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

            var engine = new Engine();
            var player1Instance = player1.GetInstance();
            var player2Instance = player2.GetInstance();
            var globalState = engine.Init(player1Instance, player2Instance);

            System.Console.WriteLine("Player 1 delt: {0}", string.Join(", ", globalState.Player1Cards.Select(x => x.ToConcise())));
            System.Console.WriteLine("Player 2 delt: {0}", string.Join(", ", globalState.Player1Cards.Select(x => x.ToConcise())));

            var player1Turn = true;
            GlobalBoardTurn turn;
            while ((turn = engine.Play()) != null)
            {
                if (player1Turn)
                {
                    System.Console.Write("Player 1: ");
                }
                else
                {
                    System.Console.Write("Player 2: ");
                }

                if (turn.PlayIsDiscard)
                {
                    System.Console.Write("Discarded {0}, ", turn.PlayCard.ToConcise());
                }
                else
                {
                    System.Console.Write("Played {0}, ", turn.PlayCard.ToConcise());
                }

                if (turn.DrawLocation.HasValue)
                {
                    System.Console.Write("Took {0}, ", turn.DrawCard.ToConcise());
                }
                else
                {
                    System.Console.Write("Drew {0}, ", turn.DrawCard.ToConcise());
                }

                System.Console.WriteLine();

                player1Turn = !player1Turn;
            }

            var scores = engine.Score();
            System.Console.WriteLine("Player 1 Score: {0}", scores.Item1);
            System.Console.WriteLine("Player 2 Score: {0}", scores.Item2);
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
