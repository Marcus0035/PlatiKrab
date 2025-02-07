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

            foreach (var player in players)
            {
                pRatios.Add(player, (float)pTrainings[player] / (pPayments[player] + 1));
            }

            return pRatios.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }
    }
}
