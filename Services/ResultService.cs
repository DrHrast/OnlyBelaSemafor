using OnlyBelaSemafor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlyBelaSemafor.Services
{
    internal class ResultService
    {
        //DID_IT 3.1: Sum team score with team call, bela
        //DID_IT 3.2: Check if it's štilja and act accordingly
        //DID_IT: 3.2.1: If štilja add 90 points and all cals from bouth teams (including bela)
        //DID_IT: 3.2.2: If not štilja calculate as usually

        private List<int>? results;
        private int totalTeamOneResult;
        private int totalTeamTwoResult;

        public List<int> SumResults(ResultModel result)
        {
            CheckForStilja(result); //Here add method calls for all calculations

            results = new List<int>() { totalTeamOneResult, totalTeamTwoResult };
            return results;
        }

        public void CheckForStilja(ResultModel result)
        {
            if (result.team1Score == 0)
            {
                if (result.team1Bela != 0 || result.team1Call != 0)
                {
                    totalTeamTwoResult = 162 + result.team1Bela
                        + result.team1Call
                        + result.team2Bela
                        + result.team2Call;
                    totalTeamOneResult = 0;
                }
            }
            else if (result.team2Score == 0)
            {
                totalTeamOneResult = 162 + result.team1Bela
                        + result.team1Call
                        + result.team2Bela
                        + result.team2Call;
                totalTeamTwoResult = 0;
            }
            else
            {
                totalTeamOneResult = result.team1Score + result.team1Bela + result.team1Call;
                totalTeamTwoResult = result.team2Score + result.team2Bela + result.team2Call;
            }
        }
    }
}
