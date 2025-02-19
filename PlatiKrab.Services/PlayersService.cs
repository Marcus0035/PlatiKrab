using PlatiKrab.Data.Models;

namespace PlatiKrab.Services
{
    public class PlayersService
    {
        public Dictionary<Player, int> GetPlayersPaymentCount(List<Player> players)
        {
            return players.ToDictionary(player => player, player => player.Payments.Count());
        }

        public Dictionary<Player, int> GetPlayersTrainingCount(List<Player> players)
        {
            return players.ToDictionary(player => player, player => player.Trainings.Count);
        }

        public Player GetPlayerWithMostRatio(List<Player> players)
        {
            Dictionary<Player, int> pPayments = GetPlayersPaymentCount(players);
            Dictionary<Player, int> pTrainings = GetPlayersTrainingCount(players);
            Dictionary<Player, float> pRatios = new();

            List<Player> playerWithZeroPayments = players.Where(p => p.Payments.Count == 0).ToList();
            List<Player> validPlayers = players.Except(playerWithZeroPayments).ToList();

            if (playerWithZeroPayments.Count > 0)
            {
                int maxTrainings = playerWithZeroPayments.Max(p => p.Trainings.Count);
                return playerWithZeroPayments.First(p => p.Trainings.Count == maxTrainings);
            }

            foreach (var player in validPlayers)
            {
                pRatios.Add(player, (float)pTrainings[player] / pPayments[player]);
            }

            return pRatios.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }
    }
}
