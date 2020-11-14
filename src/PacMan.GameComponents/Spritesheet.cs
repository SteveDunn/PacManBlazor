using System;
using System.Drawing;
using Microsoft.AspNetCore.Components;

namespace PacMan.GameComponents
{
    public static class Spritesheet
    {
        static ElementReference? _reference;

        public static ElementReference Reference => _reference ?? throw new InvalidOperationException("Nothing set yet!");

        public static void SetReference(ElementReference reference)
        {
            _reference = reference;
        }

        public static Size Size => new(225, 248);
    }
}