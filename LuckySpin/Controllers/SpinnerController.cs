using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuckySpin.Models;
using LuckySpin.ViewModels;
using LuckySpin.Services;

namespace LuckySpin.Controllers
{
    public class SpinnerController : Controller
    {
        // DIJ Repository and DbContext objects
        private readonly LuckySpinContext _dbContext;   // database access for Players/Games/Spins
        private readonly Repository _repository;        // helper access for getting a Game with Spins

        public SpinnerController(LuckySpinContext dbContext, Repository repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        /***
         * Index Action (GET and POST)
         ***/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Player player)
        {
            if (!ModelState.IsValid)
            {
                return View(player);
            }

            // Save new player + create a new game
            _dbContext.Players.Add(player);

            var game = new Game()
            {
                Player = player
            };

            _dbContext.Games.Add(game);
            _dbContext.SaveChanges(); // game.Id exists after this

            return RedirectToAction("Spin", new { gameId = game.Id });
        }

        /***
         * Spin Action (GET only)
         ***/
        [HttpGet]
        public IActionResult Spin(int gameId)
        {
            Game game = _repository.getGame(gameId); // should include Player + Spins

            // Create spin linked to game
            Spin spin = new Spin() { GameId = game.Id };

            // Apply game logic and store the spin
            game.PlayTurn(spin);
            game.Spins.Add(spin);

            // ✅ IMPORTANT: ensure the spin is tracked/inserted
            _dbContext.Set<Spin>().Add(spin);
            _dbContext.SaveChanges();

            // Keep playing until game over
            if (game.Status != GameStatus.GameOver)
            {
                return View("Spin", game);
            }

            return RedirectToAction("LuckList", new { gameId = gameId });
        }

        /***
         * LuckList Action (GET only)
         ***/
        [HttpGet]
        public IActionResult LuckList(int gameId)
        {
            return View(_repository.getGame(gameId));
        }

        /***
         * PlayersChoice Action (GET and POST)
         ***/
        [HttpGet]
        public IActionResult PlayersChoice()
        {
            PlayersChoice vm = new PlayersChoice()
            {
                Players = _dbContext.Players
                    .OrderBy(p => p.FirstName)
                    .ThenBy(p => p.Luck)
                    .ToList(),

                Games = _dbContext.Games
                    .Include(g => g.Player)
                    .Include(g => g.Spins)
                    .OrderByDescending(g => g.Spins.Count)
                    .ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult PlayersChoice(int SelectedPlayerId)
        {
            if (SelectedPlayerId <= 0)
            {
                return RedirectToAction("PlayersChoice");
            }

            Player? player = _dbContext.Players.Find(SelectedPlayerId);

            // TODO in assignment: use ModelState validation instead of null checks
            if (player == null)
            {
                return RedirectToAction("PlayersChoice");
            }

            // Start a new game with selected player and $5 balance
            player.Balance = 5.0m;

            Game game = new Game()
            {
                PlayerId = player.Id,
                Player = player
            };

            _dbContext.Games.Add(game);
            _dbContext.SaveChanges();

            return RedirectToAction("Spin", new { gameId = game.Id });
        }
    }
}
