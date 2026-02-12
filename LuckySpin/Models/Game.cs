using System;
using System.Collections.Generic;
using System.Linq;
using LuckySpin.Services;

namespace LuckySpin.Models
{
    /**
    * Game Model holds the game logic and in-memory list of Spins as well as the current Player
    * Notice the use of the GameStatus enum to track the current state of the Game
    * this will help control the game play flow in the Controller and Views
    **/
    public class Game
    {
        //Instance Variabless
        private decimal PlayCost { //Read only - the cost to play a spin
            get { return 0.50m; }
        }

        //Game Model Properties
        public int Id { get; set; }
        public GameStatus Status { get; set; } = GameStatus.Idle;
        //Game Navigation properties
        public int PlayerId { get; set; } //Foreign Key to the Player who is playing this Game
        public Player Player { get; set; } //Navigation property to the Player who is playing this Game
        public ICollection<Spin> Spins { get; set; } = new List<Spin>(); //The list of Spins for this Game

        //Methods
        // Implement the PlayTurn Method as shown in Figure 1. Be sure to set Game Status appropriately
        //      Run Unit Tests to check
        public void PlayTurn(Spin spin){
            Status = GameStatus.Spinning;
            if (Player.Balance >= PlayCost)
            {
                Player.Balance -= PlayCost;

                if (spin.isWinning(Player))
                {
                    Player.Balance += 1.0m;
                    Status = GameStatus.Won;
                }
                else
                {
                    Status = GameStatus.Spinning;
                }
                spin.RunningBalance = Player.Balance;
                if (Player.Balance < PlayCost)
                {
                    Status = GameStatus.GameOver;
                }

            }
            else
            {
                Status = GameStatus.GameOver;
            } 
        }
        //Game helper methods
        public void Start()
        {
            Reset();
            Status = GameStatus.Spinning;
        }
        public void Reset()
        {
            Status = GameStatus.Idle;
        }
    }

    public enum GameStatus
    {
        Idle,
        Spinning,
        Won,
        GameOver
    }
}