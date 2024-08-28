using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



namespace CardFool
{
    internal class MPlayer1
    {
        public List<SCard> UsedCards = new List<SCard>();
        public List<SCardPair> CardsOnTable = new List<SCardPair>();
        public bool defendStatus = false; // этап игры (защита/атака)
        private string name = "Plahotnikov.Vladimir";
        private List<SCard> hand = new List<SCard>();       // карты на руке

        // Возвращает имя игрока
        public string GetName()
        {
            return name;
        }

        // количество карт на руке
        public int GetCount()
        {
            return hand.Count;

        }
        // Добавляет новую карту в руку
        public void AddToHand(SCard card)
        {
            hand.Add(card);
        }

        // Сделать ход (первый)
        public List<SCard> LayCards()
        {
            if (defendStatus)
            {
                UpdateDefendStatus();
            }
            Sort();

            List<SCard> forAttack = [];
            int index = 0;
            for (; index < hand.Count; ++index)
            {
                if (hand[index].Suit != MTable.GetTrump().Suit)
                    break;
            }
            if (index == hand.Count)
                forAttack.Add(hand[0]);
            else
            {
                for (int i = 0; i < hand.Count; ++i)
                {
                    if (hand[i].Rank == hand[index].Rank && hand[i].Suit != MTable.GetTrump().Suit)
                    {
                        forAttack.Add(hand[i]);
                    }
                }
                int RemainingCards = 36 - UsedCards.Count - hand.Count;
                while (RemainingCards < forAttack.Count)
                {
                    forAttack.RemoveAt(forAttack.Count - 1);
                }
            }
            foreach(var card in forAttack)
            {
                hand.Remove(card);
            }
            return forAttack;
        }

        // Отбиться.
        // На вход подается набор карт на столе, часть из них могут быть уже покрыты
        public bool Defend(List<SCardPair> table)
        {
            if (!defendStatus)
            {
                UpdateDefendStatus();
            }
            Sort();
            if (hand.Count > 0)
            {
                for (int i = 0; i < table.Count; i++)
                { // Смотрим по тем картаам, что лежать на столе
                    if (!table[i].Beaten)
                    {
                        //ищем первую карту той же масти минимального ранга для отбития
                        int index = 0;
                        for (; index < hand.Count; ++index)
                        {
                            if (hand[index].Suit == table[i].Down.Suit && hand[index].Rank > table[i].Down.Rank)
                                break;
                        }

                        // если мы не нашли карту той же масти для отбития и атакующая карта не козырная, то ищем минимальный козырь 
                        if (index == hand.Count && table[i].Down.Suit != MTable.GetTrump().Suit)
                        {
                            for (index = 0; index < hand.Count; ++index)
                            {
                                if (hand[index].Suit == MTable.GetTrump().Suit)
                                    break;
                            }
                        }

                        if (index == hand.Count)
                            return false;

                        var BeatenPair = new SCardPair(table[i].Down);
                        BeatenPair.SetUp(hand[index], MTable.GetTrump().Suit);
                        table[i] = BeatenPair;
                        hand.RemoveAt(index);

                        if (hand.Count == 0) // если у нас кончились карты
                        {
                            CardsOnTable = table.ToList();
                            return true;
                        }
                    }
                }
            }
            CardsOnTable = table.ToList();
            return true;
        }

        // Подбросить карты
        // На вход подаются карты на столе
        public bool AddCards(List<SCardPair> table)
        {
            int CardCount = UsedCards.Count() + hand.Count() + table.Count * 2;

            if (table.Count == 6 || CardCount >= 36)
            {
                CardsOnTable = table.ToList();
                return false;
            }
            Sort();
            bool AllBeaten = true;
            foreach(var pair in table)
            {
                AllBeaten &= pair.Beaten;
            }
            for (int i = 0; i < table.Count; i++)
            {
                for (int j = 0; j < hand.Count; j++)
                {
                    if (hand[j].Rank == table[i].Up.Rank || hand[j].Rank == table[i].Down.Rank)
                    {
                        if (hand[j].Suit != MTable.GetTrump().Suit) // Подкидываем не козырь
                        {
                            table.Add(new SCardPair(hand[j]));
                            hand.RemoveAt(j);
                            return true;
                        }
                        // если игра подходит к концу, реализованных карт не меньше 25 и соперник отбился от всего, то можем подкидывать и козыри
                        else if (AllBeaten && CardCount >= 25 && hand[j].Suit == MTable.GetTrump().Suit) 
                        {
                            table.Add(new SCardPair(hand[j]));
                            hand.RemoveAt(j);
                            return true;
                        }
                    }
                }
            }
            CardsOnTable = table.ToList();
            return false;
        }
        private void UpdateDefendStatus()
        {
            defendStatus = !defendStatus;
            foreach (var pair in CardsOnTable)
            {
                UsedCards.Add(pair.Up);
                UsedCards.Add(pair.Down);
            }
            CardsOnTable.Clear();
        }
        private void Sort() // сортировка по рангу
        {
            hand.Sort((x, y) => x.Rank.CompareTo(y.Rank));
        }
        // Вывести в консоль карты на руке
        public void ShowHand()
        {
            Console.WriteLine("Hand " + name);
            foreach (SCard card in hand)
            {
                MTable.ShowCard(card);
                Console.Write(MTable.Separator);
            }
            Console.WriteLine();
        }
    }
}
