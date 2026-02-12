using LuckySpin.Models;
using Microsoft.EntityFrameworkCore;

namespace LuckySpin.Services
{
    public class Repository
    {   
        // This repository will eventually be the an interface between the Controller and the database context, but for now it just provides access to the database context
        private LuckySpinContext _dbContext;
        public Repository(LuckySpinContext dbContext)
        {
            _dbContext = dbContext;
        }
        public DbSet<Spin> Spins => _dbContext.Spins;
        public DbSet<Game> Games => _dbContext.Games;
        public DbSet<Player> Players => _dbContext.Players;

        //Methods to get Game, Spins and Player for a given GameId
        public Game getGame(int GameId) {
            return _dbContext.Games.FirstOrDefault(g => g.Id == GameId) ?? new Game();
        }

        public ICollection<Spin> getSpins(int GameId) {
            return _dbContext.Games.FirstOrDefault(g => g.Id == GameId)?.Spins ?? new List<Spin>();
        }   

        public Player getPlayer(int GameId) {
            return _dbContext.Games.FirstOrDefault(g => g.Id == GameId)?.Player ?? new Player();
        }



    }

}
