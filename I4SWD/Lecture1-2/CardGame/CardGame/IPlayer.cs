﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    public interface IPlayer
    {
        string Name { get; }

        void ShowHand();

        uint TotalValue();

        void DealCard(ICard card);
    }
}
