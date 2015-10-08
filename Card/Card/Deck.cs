using System;
using System.Collections.Generic;
using System.Linq;

namespace Card
{
    public interface IDeck
    {
        void Shuffle();

        List<Card> Draw(int howmany);

        List<Card> DrawSorted(int howmany);
    }

    public enum Suit : byte
    {
        Clubs = 0,

        Diamonds = 1,

        Hearts = 2,

        Spades = 4
    };

    /// <summary>
    /// Deck implements IDeck, oop friendly.
    /// 
    /// Shuffle, Draw and DrawSorted methods are virtual, this means they can be
    /// overridden using class inheritance in case a different implementation of 
    /// the method is required.
    /// </summary>
    public class Deck : IDeck 
    {
        // Do diff not as a static member
        private static Random randomiser = new Random();

        private List<Card> playingDeck;

        public Func<string, string> Selector = str => str.ToUpper();

        public Func<string, string, int> LenghtOfTwoStrings = (s1, s2) => (s1.Length + s2.Length);

        private Func<int, List<Card>, List<Card>> Take = (i, lst) => lst.Take(i).ToList();

        private Func<List<Card>, List<Card>, List<Card>> Except = (lst, exl) => lst.Except(exl).ToList();

        private Func<Card, List<Card>, bool> Add = (card, deck) => { deck.Add(card); return true; };

        public Deck()
        {
            playingDeck = new List<Card>();

            DefaultDeckOfCards();
        }

        private void DefaultDeckOfCards()
        {
            var helper = new EnumHelper();

            var suites = helper.GetValues<Suit>();

            int[] ranks = Enumerable.Range(0, 13).ToArray();

            string[] faces = { "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };

            //IEnumerable<String> uppercaseFace = faces.Select(Selector);

            Console.WriteLine(faces.Select(Selector));


            //foreach (var ucf in uppercaseFace)
            //{
            //    Console.WriteLine(ucf);

            //    var x = LenghtOfTwoStrings("A", ucf);

            //    Console.WriteLine(ucf);


            //    Console.WriteLine(LenghtOfTwoStrings("A", ucf));
            //}

            foreach (var suit in suites)
            {
                for (var i = 0; i < ranks.Length; i++)
                {
                    var card = new Card() { Name = faces[i], Rank = ranks[i], Suit = (byte)suit };

                    playingDeck.Add(card);
                }
            }
        }

        //Fisher-Yates shuffle.
        private static void Shuffling<T>(List<T> list)
        {
            int n = list.Count;

            while (n > 1)
            {
                n--;

                int k = randomiser.Next(n + 1);

                T value = list[k];

                list[k] = list[n];

                list[n] = value;
            }
        }

        public virtual void Shuffle()
        {
            Shuffling(playingDeck);
        }

        public virtual List<Card> Draw(int howmany)
        {
            return Pick(howmany);
        }

        public virtual List<Card> DrawSorted(int howmany)
        {
            OrderBySuitThenRank();

            return Pick(howmany);
        }

        private void OrderBySuitThenRank()
        {
            // Lamda to sort deck
            playingDeck = playingDeck.OrderBy(c => (byte)c.Suit).ThenBy(c => c.Rank).ToList();
        }

        private List<Card> Pick(int i)
        {
            if (playingDeck.Count == 0)
            {
                // Do differently I would return playing Desck instead of null;
                return null;
            }
            //
            // I've made an assumption here to only return the number of cards
            // left in the deck where the number of cards requested is less than
            // available.
            //
            if (i > playingDeck.Count)
            {
                i = playingDeck.Count;
            }

            var cards = playingDeck.Take(i);

            playingDeck = playingDeck.Except(cards).ToList();

            return cards.ToList();
        }
    }

    public class Card
    {
        public string Name { get; set; }

        public int Rank { get; set; }

        public byte Suit { get; set; }
    }

    public class EnumHelper
    {
        public IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
