using UnityEngine;

namespace Assets.Utilities.Definitions
{
    public static class CustomColours
    {
        public const float softColourMultiplier = 0.25f;

        public static readonly Color Red = new Color(1, 0, 0, 1);
        public static readonly Color SoftRed = new Color(1, 0, 0, softColourMultiplier);

        public static readonly Color Green = new Color(0, 1, 0, 1);
        public static readonly Color SoftGreen = new Color(0, 1, 0, softColourMultiplier);

        public static readonly Color Blue = new Color(0, 0, 1, 1);
        public static readonly Color SoftBlue = new Color(0, 0, 1, softColourMultiplier);

        public static readonly Color Yellow = new Color(1, 0.92f, 0.016f, 1);
        public static readonly Color SoftYellow = new Color(1, 0.92f, 0.016f, softColourMultiplier);

        public static readonly Color White = new Color(1, 1, 1, 1);
        public static readonly Color SoftWhite = new Color(1, 1, 1, softColourMultiplier);
    }
}
