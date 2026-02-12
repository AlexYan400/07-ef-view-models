using Microsoft.AspNetCore.Mvc;
using LuckySpin.Models;
using LuckySpin.ViewModels;
using LuckySpin.Services;

namespace LuckySpin.Controllers
{
    public class SpinnerController : Controller
    {
        //DIJ Repository and DbContext objects
        private LuckySpinContext _dbContext; // the database context provides access to the database records for Players and Games
        private Repository _repository; // the repository provides access to the Games Spins
        //Constructor with DIJ Repository object
        public SpinnerController(LuckySpinContext dbContext, Repository repository)
        {
            // (3) saves the DIJ Repository objects into your instance variables
            _dbContext = dbContext;
            _repository = repository;
        }
        /***
         * Index Action (GET and POST)
         **/
        [HttpGet]
        public IActionResult Index()
        {
                return View();
        }
        [HttpPost]
        public IActionResult Index(Player player)
        {
            if(!ModelState.IsValid) { return View(); }

            //Stores the player in the repository
            _dbContext.Players.Add(player);
            var game = new Game(){ 
                Player = player
            };
            _dbContext.Games.Add(game);
            _dbContext.SaveChanges(); //Only after saving changes to the database will the Game have an Id, which is needed to link the Game to the Spins in the next Action
    
            return RedirectToAction("Spin", new{gameId = game.Id}); //Redirect to the Spin Action, passing the Game as a parameter
        }

        /***
         * Spin Action (GET only, no data from the View)
         **/       
        public IActionResult Spin(int gameId)
        {
            Game Game = _repository.getGame(gameId); // Get the Game from the repository to ensure you have the most up to date version of the Game with all the Spins
            //Spin away!
            Spin spin = new Spin() { Game = Game, GameId = Game.Id }; //Creates a new Spin, setting the Game and GameId properties to link it to the current Game
            Game.PlayTurn(spin);
            Game.Spins.Add(spin);
            _dbContext.SaveChanges();

            //Checks to see if the game is done, if not keep spinning
            //  if so, redirect to the LuckList Action to show the list of spins
            if (Game.Status != GameStatus.GameOver)
            {
                return View("Spin", Game); //Keep Playing
            }
            
            return RedirectToAction("LuckList", new {gameId = gameId}); 
        }

        /***
         * ListSpins Action (Get only, no data from the View)
         **/
        [HttpGet]
        public IActionResult LuckList(int gameId)
        {
             // Passes the repository to the View to display the game results
            return View(_repository.getGame(gameId));
        }

            /***
            * PlayersChoice Action (GET and POST)
            **/
        [HttpGet]
        public IActionResult PlayersChoice()
        {
            PlayersChoice playersChoice = new PlayersChoice()
            {
                //TODO: Pull data from the database for the properties of the view model.

            };
            return View(playersChoice);
       
        }
        [HttpPost]
        public IActionResult PlayersChoice(int SelectedPlayerId)
        {
            Player? player = _dbContext.Players.Find(SelectedPlayerId);
            //TODO: Use ModelState validation instead of the null check below
            if (player == null) { return RedirectToAction("PlayersChoice"); } 
            //Gift Balance for returning Players
            if (player.Balance == 0) { player.Balance = 5.0m; }

            //TODO: Create a new Game for the selected Player, save it to the database, and redirect to the Spin Action to start playing with the Game ID
            return RedirectToAction("PlayersChoice");
        }

    }
}

