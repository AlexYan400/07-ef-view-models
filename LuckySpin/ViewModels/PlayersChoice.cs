using LuckySpin.Models;
using System.Collections.Generic;

namespace LuckySpin.ViewModels
{
    public class PlayersChoice
    {
        // Selected player from dropdown
        public int SelectedPlayerId { get; set; }

        // All players for dropdown list
        public List<Player> Players { get; set; } = new List<Player>();

        // All previous games ordered by most spins
        public List<Game> Games { get; set; } = new List<Game>();
    }
}
