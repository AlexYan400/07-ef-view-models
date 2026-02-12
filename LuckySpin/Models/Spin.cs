using System;
using System.Linq;

namespace LuckySpin.Models
{
    public class Spin
    {
        // Instance Variables
        Random random = new Random();
        private int[] numbers;

        // Constructor
        public Spin()
        {
            numbers = new int[] 
            { 
                random.Next(10), 
                random.Next(10), 
                random.Next(10) 
            };
        }

        // Model Properties
        public int Id { get; set; }
        public decimal RunningBalance { get; set; }

        public int[] Numbers
        {
            get { return numbers; }
            set { numbers = value; }   // ✅ THIS was the missing piece
        }

        // Navigation Properties
        public int GameId { get; set; }
        public Game Game { get; set; }

        // Spin Method
        public bool isWinning(Player player)
        {
            return (player == null) 
                ? false 
                : numbers.Contains(player.Luck);
        }
    }
}

