using System;
using System.Collections.Generic;
using System.Linq;

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
    private static Random randomiser = new Random();

    private List<Card> playingDeck;

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

    /// <summary>
    /// Draw a number of cards from the pack
    /// </summary>
    /// <param name="howmany"></param>
    /// <returns>List of cards being dealt, remember to handle null.</returns>
    public virtual List<Card> Draw(int howmany)
    {
        return Pick(howmany);
    }

    /// <summary>
    /// Sort the pacvk and then draw number of cards from the beginning.
    /// </summary>
    /// <param name="howmany"></param>
    /// <returns>List of cards being dealt, remember to handle null.</returns>
    public virtual List<Card> DrawSorted(int howmany)
    {
        OrderBySuitThenRank();

        return Pick(howmany);
    }

    private void OrderBySuitThenRank()
    {
        playingDeck = playingDeck.OrderBy(c => (byte)c.Suit).ThenBy(c => c.Rank).ToList();
    }

    private List<Card> Pick(int i)
    {
        if (playingDeck.Count == 0)
        {
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

