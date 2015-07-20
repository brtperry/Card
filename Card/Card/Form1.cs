using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Card
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private class DeckController
        {
            private List<IDeck> decks;

            public DeckController(IDeck id)
            {
                decks = new List<IDeck>();


            }

            public void Subscribe(IDeck id)
            {
                decks.Add(id);
            }

            public void Draw(int i)
            {
                foreach (var id in decks)
                {
                    id.Draw(i);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var deck = new Deck();

            deck.Shuffle();

            ListCards(deck.Draw(4));

            ListCards(deck.DrawSorted(10));

            ListCards(deck.Draw(10));

            //deck.Shuffle();

            ListCards(deck.Draw(10));

            ListCards(deck.DrawSorted(10));

            ListCards(deck.DrawSorted(10));

            ListCards(deck.DrawSorted(10));
        }

        private void ListCards(List<Card> cards)
        {
            if (cards == null)
            {
                return;
            }

            foreach (var card in cards)
            {
                Console.WriteLine("{0} of {1}", card.Name, (Suit)card.Suit);
            }

            Console.WriteLine("-----------------------------------------------");
        }
    }
}
