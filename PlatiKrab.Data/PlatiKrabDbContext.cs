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
        public DbSet<Payment> Payments { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }


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

        public async Task<List<Player>> GetPlayersAsync()
        {
            using (var context = new PlatiKrabDbContext())
            {
                return await context.Players.Include(x => x.Trainings).Include(x => x.Payments).ToListAsync();
            }
        }
        public async Task<List<Player>> GetActivePlayersAsync()
        {
            using (var context = new PlatiKrabDbContext())
            {
                return await context.Players.Include(x => x.Trainings).Include(x => x.Payments).Where(x => x.Active).ToListAsync();
            }
        }
        public async Task AddPlayerAsync(Player player)
        {
            using (var context = new PlatiKrabDbContext())
            {
                context.Players.Add(player);
                await context.SaveChangesAsync();
            }
        }
        public async Task UpdatePlayerAsync(Player player)
        {
            using (var context = new PlatiKrabDbContext())
            {
                context.Players.Update(player);
                await context.SaveChangesAsync();
            }
        }
        public async Task DeletePlayerAsync(Player player)
        {
            using (var context = new PlatiKrabDbContext())
            {
                context.Players.First(x => x.PlayerId == player.PlayerId).Active = false;
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<Training>> GetTrainingsAsync()
        {
            using (var context = new PlatiKrabDbContext())
            {
                return await context.Trainings.Include(a => a.Payment).Include(a => a.Players).OrderByDescending(a => a.Date).ToListAsync();
            }
        }
        public async Task<List<Training>> GetUnPaidTrainingsAsync()
        {
            using (var context = new PlatiKrabDbContext())
            {
                return await context.Trainings.Include(a => a.Payment).Include(a => a.Players).Where(a => a.Payment == null || !a.Payment.Paid).OrderByDescending(a => a.Date).ToListAsync();
            }
        }
        public async Task AddTrainingAsync(Training training)
        {
            using (var context = new PlatiKrabDbContext())
            {
                context.Trainings.Add(training);
                await context.SaveChangesAsync();
            }
        }
        public async Task UpdateTrainingAsync(Training training)
        {
            using (var context = new PlatiKrabDbContext())
            {
                context.Trainings.Update(training);
                await context.SaveChangesAsync();
            }
        }
        public async Task AddOrUpdateTraining(Training training)
        {
            if (training.TrainingId == 0)
            {
                await AddTrainingAsync(training);
            }
            else
            {
                await UpdateTrainingAsync(training);
            }
        }
        public async Task DeleteTrainingAsync(Training training)
        {
            using (var context = new PlatiKrabDbContext())
            {
                context.Trainings.Remove(training);
                await context.SaveChangesAsync();
            }
        }
        public async Task DeleteTrainingByIdAsync(int trainingId)
        {
            using (var context = new PlatiKrabDbContext())
            {
                var training = context.Trainings.First(x => x.TrainingId == trainingId);
                context.Trainings.Remove(training);
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<Player>> GetPlayersFromTrainingAsync(int trainingId)
        {
            using var db = new PlatiKrabDbContext();
            return await db.Trainings
                     .Where(t => t.TrainingId == trainingId)
                     .Include(t => t.Players)
                     .SelectMany(t => t.Players)
                     .ToListAsync();
        }
        public async Task UpdatePlayersOnTrainingAsync(int trainingId, List<Player> players)
        {
            using (var context = new PlatiKrabDbContext())
            {
                var training = context.Trainings
                                      .Include(t => t.Players)
                                      .FirstOrDefault(t => t.TrainingId == trainingId);

                if (training == null)
                {
                    throw new ArgumentException($"Training with ID {trainingId} does not exist.");
                }

                training.Players.Clear();

                foreach (var player in players)
                {
                    var existingPlayer = context.Players.Find(player.PlayerId);

                    if (existingPlayer != null)
                    {
                        training.Players.Add(existingPlayer);
                    }
                    else
                    {
                        context.Players.Add(player);
                        training.Players.Add(player);
                    }
                }
                await context.SaveChangesAsync();
            }
        }
        public async Task<bool> IsTrainingPaidAsync(int trainingId)
        {
            using (var context = new PlatiKrabDbContext())
            {
                return await context.Payments.AnyAsync(p => p.TrainingId == trainingId && p.Paid);
            }
        }
        public async Task<bool> DoesTrainingHavePayment(int trainingId)
        {
            using (var context = new PlatiKrabDbContext())
            {
                return await context.Payments.AnyAsync(p => p.TrainingId == trainingId);
            }
        }
        public async Task<List<Payment>> GetPaymentsAsync()
        {
            using (var context = new PlatiKrabDbContext())
            {
                return await context.Payments.Include(a => a.Player).Include(a => a.Training).ToListAsync();
            }
        }
        public async Task AddPayment(Payment payment)
        {
            using (var context = new PlatiKrabDbContext())
            {
                var existingPayment = context.Payments.Find(payment.PaymentId);
                if (existingPayment == null)
                {
                    context.Entry(payment.Player).State = EntityState.Unchanged;
                    context.Entry(payment.Training).State = EntityState.Unchanged;
                    context.Payments.Add(payment);
                }
                else
                {
                    context.Entry(existingPayment).CurrentValues.SetValues(payment);
                }
                await context.SaveChangesAsync();
            }
        }
        public async Task UpdatePaymentAsync(Payment payment)
        {
            using (var context = new PlatiKrabDbContext())
            {
                context.Payments.Update(payment);
                await context.SaveChangesAsync();
            }
        }
        public async Task DeletePaymentAsync(Payment payment)
        {
            using (var context = new PlatiKrabDbContext())
            {
                context.Payments.Remove(payment);
                await context.SaveChangesAsync();
            }
        }
        public async Task ChangePaymentStatusAsync(Training training)
        {
            using (var context = new PlatiKrabDbContext())
            {
                var payment = context.Payments.First(p => p.TrainingId == training.TrainingId);
                payment.Paid = !payment.Paid;
                context.Payments.Update(payment);
                await context.SaveChangesAsync();
            }
        }

        public static  UserSettings GetUserSettings()
        {
            using (var context = new PlatiKrabDbContext())
            {
                return context.UserSettings.First();
            }
        }

        public static void UpdateUserSettings(UserSettings userSettings)
        {
            using (var context = new PlatiKrabDbContext())
            {
                context.UserSettings.Update(userSettings);
                context.SaveChanges();
            }
        }
    }
}
