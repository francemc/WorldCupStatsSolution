using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCupStats.Data.Models;

namespace WorldCupStats.WpfApp
{
    public interface IDialogService
    {
        void ShowTeamDetails(Team team);
        void ShowPlayerDetails(Player player);

        bool ShowStartupDialog(); 
    }
}
