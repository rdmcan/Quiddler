﻿/*
 * Library:         QuiddlerLibrary.dll
 * Module:          IPlayer.cs
 * Author:          Ruben Dario  Mejia Cardona
 * Date:            February 11, 2022
 * Description:     IPlayer Interface that include methods and properties
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiddlerLibrary
{
    public interface IPlayer
    {
        public int CardCount { get; }

        public int TotalPoints { get; }

        public string DrawCard();

        public bool Discard(string discardCard);

        public string PickupTopDiscard();

        public int PlayWord(string candidate);

        public int TestWord(string candidate);

        public string ToString();
    }
}
