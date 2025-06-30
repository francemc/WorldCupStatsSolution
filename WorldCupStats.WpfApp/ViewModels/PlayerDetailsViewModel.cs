using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCupStats.Data.Models;

namespace WorldCupStats.WpfApp.ViewModels
{
    class PlayerDetailsViewModel
    {
        public Player Player { get; }
        public int Goals { get; }
        public int YellowCards { get; }

        public PlayerDetailsViewModel(Player player, int goals, int yellowCards)
        {
            Player = player;
            Goals = goals;
            YellowCards = yellowCards;
        }
    }
}
