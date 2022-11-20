using System.Collections.Concurrent;
using System.Data;
using System.Numerics;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;

string[] listOfOpponents = { "Belgija", "Kanada", "Maroko", "Belgija", "Kanada", "Maroko","Belgija", "Kanada", "Maroko" };

var goalScorersD = new Dictionary<string, int>();

var croatiaScores = new List<string>();

var otherScores = new List<string>();

var listOfPlayers = new Dictionary<string, (string position, int rating)>()
{
    {"Luka Modric", ("MF", 88) },
    {"Marcelo Brozovic", ("MF", 86) },
    {"Mateo Kovacic", ("MF", 84) },
    {"Ivan Perisic", ("FW", 84) },
    {"Andrej Kramaric", ("FW", 82) },
    {"Ivan Rakitic", ("MF", 82) },
    {"Josko Gvardiol", ("DF", 81) },
    {"Mario Pasalic", ("MF", 81) },
    {"Lovro Majer", ("MF", 80) },
    {"Dominik Livakovic", ("GK", 80) },
    {"Ante Rebic", ("FW", 80) },
    {"Josip Brekalo", ("MF", 79) },
    {"Borna Sosa", ("DF", 78) },
    {"Nikola Vlasic", ("MF", 78) },
    {"Duje Caleta-Car", ("DF", 78) },
    {"Dejan Lovren", ("DF", 78) },
    {"Mislav Orsic", ("FW", 77) },
    {"Marko Livaja", ("FW", 99) },
    {"Domagoj Vida", ("DF", 76) },
    {"Ante Budimir", ("FW", 76) },
    {"Kristijan Jakic", ("MF", 76) },
    {"Marko Pjaca", ("MF", 75) },
    {"Luka Ivanusec", ("MF", 75) },
    {"Toma Basic", ("MF", 75) },
    {"Josip Sutalo", ("DF", 75) },
    {"Josip Juranovic", ("DF", 75) },
    {"Bruno Petkovic", ("FW", 75) },
    {"Martic Erlic", ("DF", 75) },
    {"Filip Krovinovic", ("MF", 80) },
    {"Stipe Biuk", ("FW", 80) }

};

var opponents = new Dictionary < string,(int points, int gd) > ()
{
    {"Hrvatska",(0,0)},
    {"Maroko", (0,0)},
    {"Kanada", (0, 0)},
    {"Belgija", (0,0)}
};

var playersInSquad = new Dictionary<string, (string position, int rating)>();
var gamesPlayed = 0;

for (int i = 0; i < 20; i++)
{
    playersInSquad.Add(listOfPlayers.ElementAt(i).Key, listOfPlayers.ElementAt(i).Value);
}

double GetRandomNumberInRange(Random random, double minNumber, double maxNumber)
{
    return random.NextDouble() * (maxNumber - minNumber) + minNumber;
}

void training(Dictionary<string, (string position, int rating)> players)
{
    Console.WriteLine("\n");
    foreach (var item in players.Keys.ToList())
    {
        Random r = new Random();
        double num1 = GetRandomNumberInRange(r, -0.05, 0.05);
        double newRating = players[item].rating;
        newRating = newRating + (newRating * num1);
        Console.WriteLine($"{item} - Stari rating: {players[item].rating} Novi rating: {(int)newRating}");
        players[item] = (players[item].position, (int)newRating);
    }
    Console.WriteLine("\n");
}



Dictionary<string, (string position, int rating)> First11(Dictionary<string, (string position, int rating)> players)
{

    var startingEleven = new Dictionary<string, (string position, int rating)>();

    int maxRat = 0;
    int j = 0;
    for (int i = 0; i < players.Count; i++)
    {
        if (players.ElementAt(i).Value.position == "GK" && players.ElementAt(i).Value.rating > maxRat)
        {
            maxRat = players.ElementAt(i).Value.rating;
            j = i;
        }
    }
    startingEleven.Add(players.ElementAt(j).Key, (players.ElementAt(j).Value.position, players.ElementAt(j).Value.rating));

    var a = 0;
    maxRat = 0;
    for (int i = 0; i < players.Count; i++)
    {
        if (players.ElementAt(i).Value.position == "DF" && players.ElementAt(i).Value.rating > maxRat)
        {
            maxRat = players.ElementAt(i).Value.rating;
            a = i;
        }
    }
    startingEleven.Add(players.ElementAt(a).Key, (players.ElementAt(a).Value.position, players.ElementAt(a).Value.rating));

    var b = 0;
    maxRat = 0;
    for (int i = 0; i < players.Count; i++)
    {
        if (players.ElementAt(i).Value.position == "DF" && players.ElementAt(i).Value.rating > maxRat && players.ElementAt(i).Key != startingEleven.ElementAt(1).Key)
        {
            maxRat = players.ElementAt(i).Value.rating;
            b = i;
        }
    }
    startingEleven.Add(players.ElementAt(b).Key, (players.ElementAt(b).Value.position, players.ElementAt(b).Value.rating));

    var c = 0;
    maxRat = 0;
    for (int i = 0; i < players.Count; i++)
    {
        if (players.ElementAt(i).Value.position == "DF" && players.ElementAt(i).Value.rating > maxRat && (players.ElementAt(i).Key != players.ElementAt(a).Key && players.ElementAt(i).Key != players.ElementAt(b).Key))
        {
            maxRat = players.ElementAt(i).Value.rating;
            c = i;
        }
    }
    startingEleven.Add(players.ElementAt(c).Key, (players.ElementAt(c).Value.position, players.ElementAt(c).Value.rating));

    var d = 0;
    maxRat = 0;
    for (int i = 0; i < players.Count; i++)
    {
        if (players.ElementAt(i).Value.position == "DF" && players.ElementAt(i).Value.rating > maxRat && (players.ElementAt(i).Key != players.ElementAt(a).Key && players.ElementAt(i).Key != players.ElementAt(b).Key && players.ElementAt(i).Key != players.ElementAt(c).Key))
        {
            maxRat = players.ElementAt(i).Value.rating;
            d = i;
        }
    }
    startingEleven.Add(players.ElementAt(d).Key, (players.ElementAt(d).Value.position, players.ElementAt(d).Value.rating));

    a = 0;
    maxRat = 0;
    for (int i = 0; i < players.Count; i++)
    {
        if (players.ElementAt(i).Value.position == "MF" && players.ElementAt(i).Value.rating > maxRat)
        {
            maxRat = players.ElementAt(i).Value.rating;
            a = i;
        }
    }
    startingEleven.Add(players.ElementAt(a).Key, (players.ElementAt(a).Value.position, players.ElementAt(a).Value.rating));

    b = 0;
    maxRat = 0;
    for (int i = 0; i < players.Count; i++)
    {
        if (players.ElementAt(i).Value.position == "MF" && players.ElementAt(i).Value.rating > maxRat && players.ElementAt(i).Key != players.ElementAt(a).Key)
        {
            maxRat = players.ElementAt(i).Value.rating;
            b = i;
        }
    }
    startingEleven.Add(players.ElementAt(b).Key, (players.ElementAt(b).Value.position, players.ElementAt(b).Value.rating));

    c = 0;
    maxRat = 0;
    for (int i = 0; i < players.Count; i++)
    {
        if (players.ElementAt(i).Value.position == "MF" && players.ElementAt(i).Value.rating > maxRat && (players.ElementAt(i).Key != players.ElementAt(a).Key && players.ElementAt(i).Key != players.ElementAt(b).Key))
        {
            maxRat = players.ElementAt(i).Value.rating;
            c = i;
        }
    }
    startingEleven.Add(players.ElementAt(c).Key, (players.ElementAt(c).Value.position, players.ElementAt(c).Value.rating));

    a = 0;
    maxRat = 0;
    for (int i = 0; i < players.Count; i++)
    {
        if (players.ElementAt(i).Value.position == "FW" && players.ElementAt(i).Value.rating > maxRat)
        {
            maxRat = players.ElementAt(i).Value.rating;
            a = i;
        }
    }
    startingEleven.Add(players.ElementAt(a).Key, (players.ElementAt(a).Value.position, players.ElementAt(a).Value.rating));

    b = 0;
    maxRat = 0;
    for (int i = 0; i < players.Count; i++)
    {
        if (players.ElementAt(i).Value.position == "FW" && players.ElementAt(i).Value.rating > maxRat && players.ElementAt(i).Key != players.ElementAt(a).Key)
        {
            maxRat = players.ElementAt(i).Value.rating;
            b = i;
        }
    }
    startingEleven.Add(players.ElementAt(b).Key, (players.ElementAt(b).Value.position, players.ElementAt(b).Value.rating));

    c = 0;
    maxRat = 0;
    for (int i = 0; i < players.Count; i++)
    {
        if (players.ElementAt(i).Value.position == "FW" && players.ElementAt(i).Value.rating > maxRat && (players.ElementAt(i).Key != players.ElementAt(a).Key && players.ElementAt(i).Key != players.ElementAt(b).Key))
        {
            maxRat = players.ElementAt(i).Value.rating;
            c = i;
        }
    }
    startingEleven.Add(players.ElementAt(c).Key, (players.ElementAt(c).Value.position, players.ElementAt(c).Value.rating));

    Console.WriteLine("\nPrvih 11:");

    foreach (var item in startingEleven)
        Console.WriteLine($"{item.Key} Pozicija: {item.Value.position} Rating {item.Value.rating}");

    Console.WriteLine("\n");

    return startingEleven;
}

void GameTime(Dictionary<string, (string position, int rating)> players)
{
    if (gamesPlayed == 6)
    {
        Console.WriteLine("\nZao mi je sve ukamice su odigrane!\n");
        return;
    }
    if (players.Count() < 11)
    {
        Console.WriteLine("Broj igraca ne smije biti manji od 11!!!");
        Console.WriteLine($"Vas broj igraca je: {players.Count()}");
    }
    var startingEleven = First11(players);

    var r = new Random();
    var r2 = new Random();

    var score1 = (int)GetRandomNumberInRange(r, 0, 5);
    var score2 = (int)GetRandomNumberInRange(r2, 0, 5);

    Console.WriteLine($"\nRezutat utakmice Hrvatska({score1}:{score2}){listOfOpponents[gamesPlayed]}");
    croatiaScores.Add($"Hrvatska({score1}:{score2}){listOfOpponents[gamesPlayed]}");

    var goalScorers = new List<string>();

    for (int i = 0; i < score1; i++)
    {
        var goal = GetRandomNumberInRange(r, 0, 10);
        goalScorers.Add(startingEleven.ElementAt((int)goal).Key);
    }

    if (score1 > score2)
    {

        Console.WriteLine("\nCestitam na pobjedi!\nIgracima je skocio moral a s njim i rating:");
        foreach (var player in startingEleven.Keys)
        {
            var randomNumber = GetRandomNumberInRange(r, 0, 0.02);
            var oldRating = startingEleven[player].rating;
            var newRating = startingEleven[player].rating + startingEleven[player].rating * randomNumber;
            startingEleven[player] = (startingEleven[player].position, (int)newRating);
            Console.WriteLine($"{player} {oldRating}->{startingEleven[player].rating}");
        }
        opponents["Hrvatska"] = (opponents["Hrvatska"].points + 3, opponents["Hrvatska"].gd + (score1 - score2));
        opponents[listOfOpponents[gamesPlayed]] = (opponents[listOfOpponents[gamesPlayed]].points, opponents[listOfOpponents[gamesPlayed]].gd - (score1 - score2));
    }
    else if(score2 > score1)
    {
        Console.WriteLine("\nNazalost rezultat je los.\nIgracima je pao moral a s njim i rating:");
        foreach (var player in startingEleven.Keys)
        {
            var randomNumber = GetRandomNumberInRange(r, -0.02, 0);
            var oldRating = startingEleven[player].rating;
            var newRating = startingEleven[player].rating + startingEleven[player].rating * randomNumber;
            startingEleven[player] = (startingEleven[player].position, (int)newRating);
            Console.WriteLine($"{player} {oldRating}->{startingEleven[player].rating}");
        }
        opponents["Hrvatska"] = (opponents["Hrvatska"].points, opponents["Hrvatska"].gd - (score2 - score1));
        opponents[listOfOpponents[gamesPlayed]] = (opponents[listOfOpponents[gamesPlayed]].points+3, opponents[listOfOpponents[gamesPlayed]].gd + (score2 - score1));
    }
    else
    {
        Console.WriteLine("\nUtakmica je zavrsila nerjesenim rezultatom:");
        foreach (var player in startingEleven.Keys)
        opponents["Hrvatska"] = (opponents["Hrvatska"].points+1, opponents["Hrvatska"].gd);
        opponents[listOfOpponents[gamesPlayed]] = (opponents[listOfOpponents[gamesPlayed]].points + 1, opponents[listOfOpponents[gamesPlayed]].gd);
    }

    if (score1 > 0)
    {
        Console.WriteLine("\nStrijelci na utakmici su: ");

        foreach (var goal in goalScorers)
        {
            if (goalScorersD.ContainsKey(goal))
            {
                goalScorersD[goal] = goalScorersD[goal] += 1;
            }
            else
            {
                goalScorersD.Add(goal, 1);
            }
            var randomNumber = GetRandomNumberInRange(r, 0, 0.05);
            var oldRating = startingEleven[goal].rating;
            var newRating = startingEleven[goal].rating + startingEleven[goal].rating * randomNumber;
            startingEleven[goal] = (startingEleven[goal].position, (int)newRating);
            Console.WriteLine($"{goal} {oldRating}->{startingEleven[goal].rating}, Postignuti golovi: {goalScorersD[goal]}");
        }
    }

    var score3 = (int)GetRandomNumberInRange(r, 0, 5);
    var score4 = (int)GetRandomNumberInRange(r2, 0, 5);

    Console.WriteLine($"\n{listOfOpponents[gamesPlayed + 1]}({score3}:{score4}){listOfOpponents[gamesPlayed+2]}\n");
    otherScores.Add($"{listOfOpponents[gamesPlayed + 1]}({score3}:{score4}){listOfOpponents[gamesPlayed + 2]}");

    if (score3 > score4)
    {
        opponents[listOfOpponents[gamesPlayed + 1]] = (opponents[listOfOpponents[gamesPlayed + 1]].points + 3, opponents[listOfOpponents[gamesPlayed + 1]].gd + (score3 - score4));
        opponents[listOfOpponents[gamesPlayed + 2]] = (opponents[listOfOpponents[gamesPlayed + 2]].points, opponents[listOfOpponents[gamesPlayed + 2]].gd + (score4 - score3));
    }
    else if(score3 < score4)
    {
        opponents[listOfOpponents[gamesPlayed + 1]] = (opponents[listOfOpponents[gamesPlayed + 1]].points, opponents[listOfOpponents[gamesPlayed + 1]].gd + (score3 - score4));
        opponents[listOfOpponents[gamesPlayed + 2]] = (opponents[listOfOpponents[gamesPlayed + 2]].points+3, opponents[listOfOpponents[gamesPlayed + 2]].gd + (score4 - score3));
    }
    else
    {
        opponents[listOfOpponents[gamesPlayed + 1]] = (opponents[listOfOpponents[gamesPlayed + 1]].points+1, opponents[listOfOpponents[gamesPlayed + 1]].gd);
        opponents[listOfOpponents[gamesPlayed + 2]] = (opponents[listOfOpponents[gamesPlayed + 2]].points+1, opponents[listOfOpponents[gamesPlayed + 2]].gd);
    }
    gamesPlayed++;
}

void AllPlayers()
{
    Console.WriteLine("\nPopis svih igraca:");
    foreach(var player in playersInSquad)
    {
        Console.WriteLine($"\n{player.Key} Pozicija:{player.Value.position} Rating:{player.Value.rating}");
    }
    Console.WriteLine("\n");
}

void RatingsAscending()
{
    Console.WriteLine("\nIgraci sortirani po ratingu uzlazno:");
    var players_sorted = playersInSquad.OrderBy(x => x.Value.rating);
    foreach (var player in players_sorted)
    {
        Console.WriteLine($"{player.Key} Pozicija:{player.Value.position} Rating:{player.Value.rating}");
    }
    Console.WriteLine("\n");
}

void RatingsDescending()
{
    Console.WriteLine("\nIgraci sortirani po ratingu silazno:");
    var players_sorted = playersInSquad.OrderByDescending(x => x.Value.rating);
    foreach (var player in players_sorted)
    {
        Console.WriteLine($"{player.Key} Pozicija:{player.Value.position} Rating:{player.Value.rating}");
    }
    Console.WriteLine("\n");
}

void PrintByName()
{
    Console.WriteLine("\nUpisi ime i prezime igraca:\n(Pr. Luka Modric)");
    string nameSurname = Console.ReadLine();
    if (playersInSquad.ContainsKey(nameSurname))
    {
        Console.WriteLine($"Pozicija-{playersInSquad[nameSurname].position} Rating-{playersInSquad[nameSurname].rating}\n");
    }
    else
    {
        Console.WriteLine("\nIgrac cije ste ime unjeli nije na raprezentativnom popisu!");
    }
}

void PrintByRating()
{
    Console.WriteLine("\nUnesi neki rating:");
    var rating = int.Parse(Console.ReadLine());
    foreach(var player in playersInSquad)
    {
        if(player.Value.rating == rating)
        {
            Console.WriteLine($"\n{player.Key} Pozicija:{player.Value.position} Rating:{player.Value.rating}");
        }
    }
    Console.WriteLine("\n");
}

void PrintByPosition()
{
    Console.WriteLine("\nUnesi neku poziciju (GK,DF,MF,FW):");
    var position = Console.ReadLine();
    foreach (var player in playersInSquad)
    {
        if (player.Value.position == position)
        {
            Console.WriteLine($"\n{player.Key} Pozicija:{player.Value.position} Rating:{player.Value.rating}");
        }
    }
    Console.WriteLine("\n");
}

void PrintFirstEleven()
{
    var startingEleven = First11(playersInSquad);
    Console.WriteLine("Prvih 11 u postavi Hrvatske reprezentacije su:");
    foreach(var player in startingEleven)
    {
        Console.WriteLine($"\n{player.Value.position} {player.Key} {player.Value.rating}");
    }
    Console.WriteLine("\n");
}

void PrintGoalScorers()
{
    Console.WriteLine("\nLista strijelaca: ");
    foreach(var player in goalScorersD)
    {
        Console.WriteLine($"\n{player.Key} Broj golova:{player.Value}");
    }
    Console.WriteLine("\n");
}

void PrintCroatiaResults()
{
    if (croatiaScores.Count == 0)
    {
        Console.WriteLine("\nJos nije odigrano niti jedno kolo!\n");
        return;
    }
    var i = 1;
    Console.WriteLine("Rezlutati Vatrenih:");
    foreach(var result in croatiaScores)
    {
        Console.WriteLine($"\n{i}. kolo: {result}");
        i++;
    }
    Console.WriteLine("\n");
}

void PrintOtherResults()
{
    if (croatiaScores.Count == 0)
    {
        Console.WriteLine("\nJos nije odigrano niti jedno kolo!\n");
        return;
    }
    var i = 1;
    Console.WriteLine("\nRezultati drugih utakimca u skupini su:");
    foreach (var result in otherScores)
    {
        Console.WriteLine($"\n{i}. kolo: {result}");
        i++;
    }
    Console.WriteLine("\n");
}

void PrintTable()
{
    Console.WriteLine($"Tablica nase skupine nakon {gamesPlayed}. kola:");
    var table_sorted= opponents.OrderByDescending(x=>x.Value);
    var i = 1;
    Console.WriteLine("\t Gol razlika \t Bodovi");
    foreach (var table in table_sorted)
    {
        Console.WriteLine($"\n{i++}. {table.Key} \t{table.Value.gd} \t    {table.Value.points}");
    }
    Console.WriteLine("\n");
}

int userInput;

do
{

    Console.WriteLine("1 - Odradi trening");
    Console.WriteLine("2 - Odigraj utakmicu");
    Console.WriteLine("3 - Statistika");
    Console.WriteLine("4 - Kontrola igraca");
    Console.WriteLine("0 - Izadi iz aplikacije");

    userInput = int.Parse(Console.ReadLine());

    switch (userInput)
    {
        case 1:
            training(playersInSquad);
            break;
        case 2:
            GameTime(playersInSquad);
            break;
        case 3:
            Console.WriteLine("\n1 - Ispis svih igraca");
            Console.WriteLine("2 - Ispis po ratingu uzlazno");
            Console.WriteLine("3 - Ispis po ratingu silazno");
            Console.WriteLine("4 - Ispis igraca po imenu i prezimenu");
            Console.WriteLine("5 - Ispis igraca po ratingu");
            Console.WriteLine("6 - Ispis igraca po poziciji");
            Console.WriteLine("7 - Ispis prve postve");
            Console.WriteLine("8 - Ispis strijelaca i koliko golova imaju");
            Console.WriteLine("9 - Ispis svih rezultata Hrvatske");
            Console.WriteLine("10 - Ispis svih rezultata u skupini");
            Console.WriteLine("11 - Ispis tablice");

            var userInput2 = int.Parse(Console.ReadLine());

            switch (userInput2)
            {
                case 1:
                    AllPlayers();
                    break;
                case 2:
                    RatingsAscending();
                    break;
                case 3:
                    RatingsDescending();
                    break;
                case 4:
                    PrintByName();
                    break;
                case 5:
                    PrintByRating();
                    break;
                case 6:
                    PrintByPosition();
                    break;
                case 7:
                    PrintFirstEleven();
                    break;
                case 8:
                    PrintGoalScorers();
                    break;
                case 9:
                    PrintCroatiaResults();
                    break;
                case 10:
                    PrintOtherResults();
                    break;
                case 11:
                    PrintTable();
                    break;
            }
            break;
    }

} while (userInput is not 0);


