/*
 * Library:         QuiddlerLibrary.dll
 * Module:          IDeck.cs
 * Author:          Ruben Dario  Mejia Cardona
 * Date:            February 11, 2022
 * Description:     IDeck Interface that include methods and properties
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiddlerLibrary
{
    public interface IDeck : IDisposable
    {
        public string About { get; }

        public int CardCount { get; }

        public int CardsPerPlayer { set; get; }

        public string TopDiscard { get; }

        public IPlayer NewPlayer();

        public string ToString();
    }
}
