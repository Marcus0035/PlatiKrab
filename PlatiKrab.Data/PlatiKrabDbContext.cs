using KeyChordFinder.Data;
using Microsoft.EntityFrameworkCore;
using PlatiKrab.Data.Models;
using System.Collections;

namespace PlatiKrab.Data
{
    public class PlatiKrabDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<PlayerTraining> PlayerTrainings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        //public DbSet<PaymentPlayer> PaymentPlayers { get; set; }

        private static readonly string ConnectionString = GetConnectionString("PlatiKrab.db");
        //private static readonly string ConnectionString = @"Data Source=C:\Users\marek\source\repos\PlatiKrab\PlatiKrab\Resources\Raw\PlatiKrab.db";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }


        private static string GetConnectionString(string dbName)
        {
            var dbPath = DbHelper.GetDbPath(dbName);
            return $"Data source={dbPath}";
        }

        public async Task<List<Player>> GetActivePlayersAsync()
        {
            using var context = new PlatiKrabDbContext();
            return await context.Players
                                    .Where(x => x.Active)
                                    .Include(x => x.PlayerTrainings).ThenInclude(x => x.Training)
                                    .Include(x => x.Payments).ToListAsync();
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            using var context = new PlatiKrabDbContext();
            return await context.Players
                                    .Include(x => x.PlayerTrainings).ThenInclude(x => x.Training)
                                    .Include(x => x.Payments).ToListAsync();
        }

        public async Task AddPlayerAsync(Player player)
        {
            using var context = new PlatiKrabDbContext();
            context.Players.Add(player);
            await context.SaveChangesAsync();
        }

        public async Task RemovePlayerAsync(Player player)
        {
            using var context = new PlatiKrabDbContext();
            var playerToChange = context.Players.First(x => x.PlayerId == player.PlayerId);
            playerToChange.Active = false;
            await context.SaveChangesAsync();
        }

        public async Task<List<Training>> GetTrainingsAsync()
        {
            using var context = new PlatiKrabDbContext();
            return await context.Trainings
                .Include(x => x.PlayerTrainings).ThenInclude(x => x.Player)
                .Include(x => x.Payment).ToListAsync();
        }

        public async Task<Training> GetTrainingByIdAsync(int trainingId)
        {
            using var context = new PlatiKrabDbContext();
            return await context.Trainings
                .Include(t => t.PlayerTrainings).ThenInclude(pt => pt.Player)
                .Include(t => t.Payment)
                .FirstAsync(t => t.TrainingId == trainingId);
        }



        public async Task<List<Training>> GetUnpaidTrainingAsync()
        {
            using var context = new PlatiKrabDbContext();
            return await context.Trainings.Include(x => x.PlayerTrainings).ThenInclude(x => x.Player)
                .Include(x => x.Payment).Where(x => x.Payment == null || !x.Payment.Paid).ToListAsync(); ;
        }

        public async Task<List<Player>> GetPlayersOnTrainingAsync(Training t)
        {
            using var context = new PlatiKrabDbContext();
            var training = await context.Trainings
                .Include(x => x.PlayerTrainings).ThenInclude(x => x.Player)
                .FirstAsync(x => x.TrainingId == t.TrainingId);


            return training.PlayerTrainings.Select(x => x.Player).ToList();
        }

        public async Task AddTrainingAsync(Training training)
        {
            using var context = new PlatiKrabDbContext();
            context.Trainings.Add(training);
            await context.SaveChangesAsync();
        }

        public async Task AddOrEditTraining(Training training, List<Player> players)
        {
            using var context = new PlatiKrabDbContext();

            if (training.TrainingId == 0)
            {
                await context.Trainings.AddAsync(training);
                await context.SaveChangesAsync();

                // Přidání nových PlayerTrainings
                foreach (var player in players)
                {
                    var playerTraining = new PlayerTraining
                    {
                        PlayerId = player.PlayerId,
                        TrainingId = training.TrainingId
                    };
                    context.PlayerTrainings.Add(playerTraining);
                }
            }
            else
            {
                var existingTraining = await context.Trainings
                    .Include(t => t.PlayerTrainings)
                    .FirstOrDefaultAsync(t => t.TrainingId == training.TrainingId);

                if (existingTraining != null)
                {
                    // Aktualizace hlavních hodnot tréninku
                    context.Entry(existingTraining).CurrentValues.SetValues(training);

                    // Smazání všech existujících PlayerTrainings
                    context.PlayerTrainings.RemoveRange(existingTraining.PlayerTrainings);

                    // Přidání nových PlayerTrainings
                    foreach (var player in players)
                    {
                        var playerTraining = new PlayerTraining
                        {
                            PlayerId = player.PlayerId,
                            TrainingId = training.TrainingId
                        };
                        context.PlayerTrainings.Add(playerTraining);
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task<Dictionary<Player, int>> GetPlayersCountTrainings(List<Player> players)
        {
            using var context = new PlatiKrabDbContext();
            var playersCountTrainings = new Dictionary<Player, int>();
            foreach (var player in players)
            {
                var count = await context.PlayerTrainings.CountAsync(x => x.PlayerId == player.PlayerId);
                playersCountTrainings.Add(player, count);
            }
            return playersCountTrainings;
        }

        public async Task<Dictionary<Player, int>> GetPlayersCountPayments(List<Player> players)
        {
            using var context = new PlatiKrabDbContext();
            var playersCountPayments = new Dictionary<Player, int>();
            foreach (var player in players)
            {
                var count = await context.Payments.CountAsync(x => x.PlayerWhoPays.PlayerId == player.PlayerId);
            }
            return playersCountPayments;
        }


        public async Task AddPaymentAsync(Payment payment)
        {
            using var context = new PlatiKrabDbContext();
            context.Payments.Add(payment);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            using var context = new PlatiKrabDbContext();
            context.Payments.Update(payment);
            await context.SaveChangesAsync();
        }
    }
}
