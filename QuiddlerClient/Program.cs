/*
 * Module:          QuiddlerClient.cs
 * Author:          Ruben Dario  Mejia Cardona
 * Date:            February 11, 2022
 * Description:     Console Quiddler Client 
 */

using System;
using System.Collections.Generic;
using QuiddlerLibrary;

namespace QuiddlerClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IDeck myDeck = new Deck())
            {
                try
                {
                    // Header Info
                    Console.WriteLine("Test Client for: Quiddler (TM) Library, © 2022  " + myDeck.About);
                    Console.WriteLine("\nDeck initialized with the following " + myDeck.CardCount + " cards...");
                    Console.WriteLine(myDeck.ToString());
                }
                catch (Exception ex)
                {
                    Console.Write("Error info:" + ex.Message);
                }
                Console.WriteLine();

                // Number of players
                int numPlayers = 0;
                do
                {
                    Console.Write("How many players are there? (1-8): ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    var number = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    if (int.TryParse(number, out numPlayers))
                        numPlayers = Int32.Parse(number);
                    if (numPlayers < 1 || numPlayers > 8)
                        Console.WriteLine("Please enter a valid number between 1 and 8");
                } while (numPlayers < 1 || numPlayers > 8);

                // Number of cards dealt to each player
                int cardsPlayers = 0;
                do
                {
                    Console.Write("How many cards will be dealt to each player? (3-10): ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    cardsPlayers = Int32.Parse(Console.ReadLine());
                    Console.ForegroundColor = ConsoleColor.White;

                    try
                    {
                        myDeck.CardsPerPlayer = cardsPlayers;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                } while (cardsPlayers < 3 || cardsPlayers > 10);

                // List of players
                List<IPlayer> players = new List<IPlayer>();

                for (int i = 0; i < numPlayers; i++)
                {
                    players.Add(myDeck.NewPlayer());
                }

                try
                {
                    Console.WriteLine($"\nCards were dealt to {numPlayers} player(s).");
                    Console.WriteLine($"The top card which was '{myDeck.TopDiscard}' was moved to the discard pile.");
                }
                catch (Exception ex)
                {
                    Console.Write("Error info:" + ex.Message);
                }

                ConsoleKey responsePlay = ConsoleKey.A;

                // Flags to keep the player in the game and discard a card into the discard pile
                bool keepPlayFlag = true;
                bool discardFlag = true;

                do
                {
                    // Iterate trough the number of players input by the user
                    for (int i = 0; i < numPlayers; i++)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\n---------------------------------------------------------------------------------------------");
                        try
                        {
                            Console.WriteLine($"Player {(i + 1)} ({players[i].TotalPoints} points)");
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Error info:" + ex.Message);
                        }
                        Console.WriteLine("---------------------------------------------------------------------------------------------");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();
                        try
                        {
                            Console.WriteLine($"The deck now contains the following {myDeck.CardCount} cards...");
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Error info:" + ex.Message);
                        }
                        Console.WriteLine(myDeck.ToString());
                        Console.WriteLine();
                        Console.WriteLine($"Your cards are [{players[i]}].");

                        // Add a card to the player hand from discard pile or deck
                        ConsoleKey responseDiscard;
                        do
                        {
                            try
                            {
                                Console.Write($"Do you want the top card in the discard pile which is '{myDeck.TopDiscard}'? (y/n): ");
                            }
                            catch (Exception ex)
                            {
                                Console.Write("Error info:" + ex.Message);
                            }

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            responseDiscard = Console.ReadKey(false).Key;
                            Console.ForegroundColor = ConsoleColor.White;
                            if (responseDiscard == ConsoleKey.Y)
                            {
                                try
                                {
                                    players[i].PickupTopDiscard();
                                }
                                catch (Exception ex)
                                {
                                    Console.Write("Error info:" + ex.Message);
                                }
                                Console.WriteLine();
                            }
                            else if (responseDiscard == ConsoleKey.N)
                            {
                                Console.WriteLine();
                                try
                                {
                                    Console.WriteLine($"The dealer dealt '{players[i].DrawCard()}' to you from the deck.");
                                    Console.WriteLine($"The deck contains {myDeck.CardCount} cards.");
                                }
                                catch (Exception ex)
                                {
                                    Console.Write("Error info:" + ex.Message);
                                }
                                break;
                            }
                            else if (responseDiscard != ConsoleKey.Y || responseDiscard != ConsoleKey.N)
                                Console.WriteLine("\nERROR: Your answer must be either 'y' or 'n'.");
                        } while (responseDiscard != ConsoleKey.Y);
                        Console.WriteLine($"Your cards are [{players[i]}].");

                        //------------- Test a word for its points value ------------------

                        string candidateStr = "";
                        int pointsTest = 0;
                        ConsoleKey responsePoints;
                        do
                        {
                            Console.Write("Test a word for its points value? (y/n): ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            responsePoints = Console.ReadKey(false).Key;
                            Console.ForegroundColor = ConsoleColor.White;
                            if (responsePoints == ConsoleKey.Y)
                            {
                                Console.Write($"\nEnter a word using [{players[i]}] leaving a space between cards: ");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                candidateStr = Console.ReadLine();
                                Console.ForegroundColor = ConsoleColor.White;

                                try
                                {
                                    pointsTest = players[i].TestWord(candidateStr);
                                    Console.WriteLine($"The word [{candidateStr}] is worth {pointsTest} points.");
                                }
                                catch (Exception ex)
                                {
                                    Console.Write("Error info:" + ex.Message);
                                }
                            }
                            else if (responsePoints == ConsoleKey.N)
                            {
                                string discardCard = "";
                                while (!players[i].Discard(discardCard))
                                {
                                    Console.Write("\nEnter a card from your hand to drop on the discard pile: ");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    discardCard = Console.ReadLine();
                                    Console.ForegroundColor = ConsoleColor.White;
                                };
                                Console.WriteLine("Your cards are [" + players[i] + "].");
                                break;
                            }
                            else
                                Console.WriteLine("\nERROR: Your answer must be either 'y' or 'n'.");
                        } while (responsePoints != ConsoleKey.Y || pointsTest == 0);

                        //----------- Play the candidate word to get points ----------------

                        if (responsePoints == ConsoleKey.Y)
                        {
                            ConsoleKey responsePlayWord;
                            do
                            {
                                Console.Write($"Do you want to play the word [{candidateStr}]? (y/n): ");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                responsePlayWord = Console.ReadKey(false).Key;
                                Console.ForegroundColor = ConsoleColor.White;

                                if (responsePlayWord == ConsoleKey.Y)
                                {
                                    try
                                    {
                                        players[i].PlayWord(candidateStr);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Write("Error info:" + ex.Message);
                                    }

                                    // If one card is left keepPlay Flag set to true 
                                    if (players[i].CardCount == 1)
                                    {
                                        Console.WriteLine($"\nDropping '{players[i]}' on the discard pile.");

                                        try
                                        {
                                            players[i].Discard(players[i].ToString());                                            
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.Write("Error info:" + ex.Message);
                                        }

                                        Console.WriteLine("");
                                        Console.WriteLine($"***** Player {(i + 1)} is out! *****");
                                        keepPlayFlag = false;
                                        discardFlag = false;
                                        responsePlay = ConsoleKey.N;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"\nYour cards are [{players[i]}] and you have {players[i].TotalPoints} points.");
                                        discardFlag = true;
                                    } // end if esle
                                }
                                else if (responsePlayWord == ConsoleKey.N)
                                {
                                    Console.WriteLine();
                                    //add a flag
                                    discardFlag = true;
                                    break;
                                }
                                else
                                    Console.WriteLine("\nERROR: Your answer must be either 'y' or 'n'.");
                            } while (responsePlayWord != ConsoleKey.Y);

                            // Drop a card on the Discard pile 
                            if (discardFlag)
                            {
                                string discardCrd = "";
                                while (!players[i].Discard(discardCrd))
                                {
                                    Console.Write("Enter a card from your hand to drop on the discard pile: ");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    discardCrd = Console.ReadLine();
                                    Console.ForegroundColor = ConsoleColor.White;
                                };
                                Console.WriteLine($"Your cards are [{players[i]}].");
                            }
                        } // end if
                    } // end for

                    // Check if players has still cards in their hand
                    if (keepPlayFlag)
                    {
                        bool playerOutFlag = true;
                        do
                        {
                            Console.Write("\nWould you like each player to take another turn? (y/n): ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            responsePlay = Console.ReadKey(false).Key;
                            Console.ForegroundColor = ConsoleColor.White;
                            if (responsePlay == ConsoleKey.Y || responsePlay == ConsoleKey.N)
                            {
                                playerOutFlag = false;
                                Console.WriteLine();
                            }
                            else
                                Console.WriteLine("\nERROR: Your answer must be either 'y' or 'n'.");
                        } while (playerOutFlag);
                    }
                } while (responsePlay == ConsoleKey.Y);

                //----------- Retiring the game and final scores ----------------

                Console.WriteLine("\nRetiring the game.");
                Console.WriteLine("\nThe final scores are...");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("---------------------------------------------------------------------------------------------");

                for (int i = 0; i < numPlayers; i++)
                {
                    Console.WriteLine($"Player {i + 1} {players[i].TotalPoints} points");
                } // end for
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}