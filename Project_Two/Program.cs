using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using System.Text;


namespace Project_Two
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Program should read the file from where the project folder exist.
            string filePath = Directory.GetCurrentDirectory();
            string stepBackOne = Directory.GetParent(filePath).ToString();
            string stepBackTwo = Directory.GetParent(stepBackOne).ToString();
            string stepBackThree = Directory.GetParent(stepBackTwo).ToString();
            string adjustedFilePath = $@"{stepBackThree}\Super_Bowl_Project.csv";

            Console.WriteLine("Hello there! This console application will create a text file with interesting Super Bowl facts up to the 2017 season.\n" +
                              "It even allows you to give a title to the text file.  ");
            Console.WriteLine("Enter a filename [EXTENSION is NOT NEEDED]: ");

            //end-user establishes the file name
            string userInput = Console.ReadLine();
            //FileException(userInput);  -- Unfortunately I can not figure out how to implement exception handling

            //filePath2 variable establishes where the text file will be saved
            string filePath2 = $@"{stepBackThree}\{userInput}.txt";
                        
            ReadWriteFiles(adjustedFilePath, filePath2);

        }

        //Method that writes to filePath2(includes all queries)
        static void ReadWriteFiles(string adjustedFilePath, string filePath2)
        {
            //Create list of SuperBowl objects
            List<SuperBowl> values = File.ReadAllLines(adjustedFilePath)
                                             .Skip(1)
                                             .Select(v => SuperBowl.FromCsv(v))
                                             .ToList();
            //Enable writing to the text file            
            StreamWriter sw = new StreamWriter(filePath2);
            
            //Generate a list of all super bowl winners
            sw.WriteLine("                                                        SUPER BOWL WINNERS                                                              ");
            sw.WriteLine("----Winning Team------------Year-----Winning Quarterback--------------Winning Coach-------Super Bowl MVP------------------Point Spread--");
            var superBowlWinners =
                from superBowl in values
                where superBowl.Date != null
                select superBowl;
            foreach (SuperBowl superBowl in superBowlWinners)
            {
                string output = String.Format("    {0,-23} {1,-8} {2,-32} {3,-19} {4,-31} {5,-17}",
                    superBowl.winningTeam, superBowl.Date.Year, superBowl.winningQB,
                    superBowl.winningCoach, superBowl.MVP, superBowl.pointDifference);
                sw.WriteLine(output);
                

            }

            //Generate a list of the top five attended super bowl’s
            sw.WriteLine("");
            sw.WriteLine("                                      TOP FIVE ATTENDED SUPER BOWLS                                            ");
            sw.WriteLine("----Year-----Winning Team------------Losing Team-------------Host City--------Host State------Host Stadium-----");
            var topFiveAttended =
               (from superBowl in values
                orderby superBowl.Attendance descending
                select superBowl).Take(5);
            foreach (SuperBowl superBowl in topFiveAttended)
            {
                string output = String.Format("    {0,-8} {1,-23} {2,-23} {3,-16} {4,-15} {5,-17}", 
                    superBowl.Date.Year, superBowl.winningTeam, superBowl.losingTeam, 
                    superBowl.hostCity, superBowl.hostState, superBowl.hostStadium);
                sw.WriteLine(output);
                
            }

            //Output the state that hosted the  most super bowls
            sw.WriteLine("");
            sw.WriteLine("        THE STATE THAT HOSTED THE MOST SUPER BOWLS       ");
            sw.WriteLine("------Host City--------Host State-------Host Stadium-------");

            var stateMostHosted = values.GroupBy(superBowl => superBowl.hostState)
                                            .Select(group => new { hostState = group.Key, Count = group.Count() })
                                            .OrderByDescending(superBowl => superBowl.Count);
            var item = stateMostHosted.First();

            var mostFrequent = item.hostState;
            var mostFrequentCount = item.Count;

            foreach (SuperBowl superBowl in values)
            {
                if (superBowl.hostState.Equals(mostFrequent))
                {
                    string output = String.Format("      {0,-16} {1,-16} {2,-17}",
                    superBowl.hostCity, superBowl.hostState, superBowl.hostStadium);
                    sw.WriteLine(output);
                }
            }

            //Generate a list of players who won MVP more than once
            sw.WriteLine("");
            sw.WriteLine("        PLAYERS WHO WON THE SUPER BOWL MVP TITLE MORE THAN ONCE          ");
            sw.WriteLine("------Super Bowl MVP--------Winning Team--------------Losing Team--------");

            var moreThanOnceMVP = from superBowl in values
                                  group superBowl by superBowl.MVP into MVPGroups
                                  orderby MVPGroups.Count() descending
                                  select MVPGroups;
            foreach (var groupMVP in moreThanOnceMVP)
            {
                if (groupMVP.Count() > 1)
                {
                    foreach (var superBowl in groupMVP)
                    {
                        string output = String.Format("      {0,-21} {1,-25} {2,-25}",
                        superBowl.MVP, superBowl.winningTeam, superBowl.losingTeam);
                        sw.WriteLine(output);
                    }
                }
                
            }

            //Which coach lost the most super bowls?
            sw.WriteLine("");
            sw.WriteLine("THE COACH THAT LOST THE MOST SUPER BOWLS");

            var coachLostMost = values.GroupBy(superBowl => superBowl.losingCoach)
                                      .Select(group => new { losingCoach = group.Key, Count = group.Count() })
                                      .OrderByDescending(superBowl => superBowl.Count);
            var clm = coachLostMost.First();
            var mostLosses = clm.losingCoach;
            var mostFrequentLosses = clm.Count;

            foreach (SuperBowl superBowl in values)
            {
                if (superBowl.losingCoach.Equals(mostLosses))
                {
                    sw.WriteLine("  " + superBowl.losingCoach + "  ");
                    break;
                }
            }

            //Which coach won the most super bowls?
            sw.WriteLine("");
            sw.WriteLine("THE COACH THAT WON THE MOST SUPER BOWLS");

            var coachWonMost = values.GroupBy(superBowl => superBowl.winningCoach)
                                     .Select(group => new { winningCoach = group.Key, Count = group.Count() })
                                     .OrderByDescending(superBowl => superBowl.Count);
            var cwm = coachWonMost.First();
            var mostWins = cwm.winningCoach;
            var mostFrequentWins = cwm.Count;

            foreach (SuperBowl superBowl in values)
            {
                if (superBowl.winningCoach.Equals(mostWins))
                {
                    sw.WriteLine("  " + superBowl.winningCoach + "  ");
                    break;
                }
            }

            //Which team(s) won the most super bowls?
            sw.WriteLine("");
            sw.WriteLine("THE TEAM(s) THAT WON THE MOST SUPER BOWLS");

            var teamWonMost = values.GroupBy(superBowl => superBowl.winningTeam)
                                    .Select(group => new { winningTeam = group.Key, Count = group.Count() })
                                    .OrderByDescending(superBowl => superBowl.Count);
            var twm = teamWonMost.FirstOrDefault();
            var tMostWins = twm.winningTeam;
            var tMostFrequentWins = twm.Count;

            foreach (SuperBowl superBowl in values)
            {
                if (superBowl.winningTeam.Equals(tMostWins))
                {
                    sw.WriteLine("  " + superBowl.winningTeam + "  ");
                    break;
                }
            }

            //Which team(s) lost the most super bowls?
            sw.WriteLine("");
            sw.WriteLine("THE TEAM(s) THAT LOST THE MOST SUPER BOWLS");

            var teamLostMost = values.GroupBy(superBowl => superBowl.losingTeam)
                                     .Select(group => new { losingTeam = group.Key, Count = group.Count() })
                                     .OrderByDescending(superBowl => superBowl.Count);
            var tlm = teamLostMost.FirstOrDefault();
            var tMostLosses = tlm.losingTeam;
            var tMostFrequentLosses = tlm.Count;

            foreach (SuperBowl superBowl in values)
            {
                if (superBowl.losingTeam.Equals(tMostLosses))
                {
                    sw.WriteLine("  " + superBowl.losingTeam + "  ");
                    break;
                }
            }

            //Which Super bowl had the greatest point difference?
            sw.WriteLine("");
            sw.WriteLine("THE SUPER BOWL THAT HAD THE GREATEST POINT DIFFERENCE");

            var greatestPointDifference = values.GroupBy(superBowl => superBowl.pointDifference)
                                                .Select(group => new { pointDifference = group.Key, Count = group.Count() })
                                                .OrderBy(superBowl => superBowl.pointDifference);
            var gpd = greatestPointDifference.Last();
            var greatestDifference = gpd.pointDifference;
            var pointDifferenceCount = gpd.Count;

            foreach (SuperBowl superBowl in values)
            {
                if (superBowl.pointDifference.Equals(greatestDifference))
                {
                    sw.WriteLine("Super Bowl " + superBowl.romanNumeral + " had the greatest point differnce of " + superBowl.pointDifference + " in " + superBowl.Date.Year + ".");
                }
            }

            //What is the average attendance of all super bowls?
            sw.WriteLine("");
            sw.WriteLine("AVERAGE ATTENDANCE OF ALL SUPER BOWLS 1967-2017");
            double AvgAttendance = values.Average(superBowl => superBowl.Attendance);
            sw.WriteLine("The average attendance at Super Bowls between 1967 to 2017 is " + AvgAttendance.ToString("n0") + " attendees.");
                


            sw.Close();


        }

        /*static void FileException(string userInput)
        {
            while (true)
            {
                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("Enter a filename [EXTENSION is NOT NEEDED]: ");
                    
                }
                else if (userInput.Contains(".txt") || userInput.Contains(".html") || userInput.Contains(".doc")
                                || userInput.Contains(".docx") || userInput.Contains(".odt") || userInput.Contains(".pdf")
                                || userInput.Contains(".rtf") || userInput.Contains(".tex") || userInput.Contains(".wks")
                                || userInput.Contains(".wps") || userInput.Contains(".wpd"))
                {
                    Console.WriteLine("Please remember to leave out the extension.  Please try again: ");
                    
                }
                
                else break;
            }
            
            

        }*/
    }
}
