using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFool
{
    internal class MPlayer1
    {
        private string name = "Vasya";
        private List<SCard> hand = new List<SCard>();       // карты на руке

        // Возвращает имя игрока
        public string GetName()
        { 
            return name;
        }

        // количество карт на руке
        public int GetCount()
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
        // Добавляет новую карту в руку
        public void AddToHand(SCard card)
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        // Сделать ход (первый)
        public List<SCard> LayCards()
        {
           //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        // Отбиться.
        // На вход подается набор карт на столе, часть из них могут быть уже покрыты
        public bool Defend(List<SCardPair> table)
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        // Подбросить карты
        // На вход подаются карты на столе
        public bool AddCards(List<SCardPair> table)
        {
           
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
