using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Two
{
    class SuperBowl
    {
        public DateTime Date { get; set; }
        public string romanNumeral { get; set; }
        public int Attendance { get; set; }
        public string winningQB { get; set; }
        public string winningCoach { get; set; }
        public string winningTeam { get; set; }
        public int winningPts { get; set; }
        public string losingQB { get; set; }
        public string losingCoach { get; set; }
        public string losingTeam { get; set; }
        public int losingPts { get; set; }
        public string MVP { get; set; }
        public int pointDifference { get; set; }
        public string hostStadium { get; set; }
        public string hostCity { get; set; }
        public string hostState { get; set; }

        public static SuperBowl FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            SuperBowl superBowl = new SuperBowl();
            superBowl.Date = Convert.ToDateTime(values[0]);
            superBowl.romanNumeral = values[1];
            superBowl.Attendance = Convert.ToInt32(values[2]);
            superBowl.winningQB = values[3];
            superBowl.winningCoach = values[4];
            superBowl.winningTeam = values[5];
            superBowl.winningPts = Convert.ToInt32(values[6]);
            superBowl.losingQB = values[7];
            superBowl.losingCoach = values[8];
            superBowl.losingTeam = values[9];
            superBowl.losingPts = Convert.ToInt32(values[10]);
            superBowl.MVP = values[11];
            superBowl.pointDifference = Convert.ToInt32(values[6]) - Convert.ToInt32(values[10]);
            superBowl.hostStadium = values[12];
            superBowl.hostCity = values[13];
            superBowl.hostState = values[14];
            return superBowl;


        }
    }
}