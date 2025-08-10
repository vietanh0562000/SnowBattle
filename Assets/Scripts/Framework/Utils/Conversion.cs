using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework.Utils
{
    public static class Conversion 
    {
        /// <summary>
        /// Packs a Color32List in to a byte array.
        /// </summary>
        /// <returns>The byte array.</returns>
        /// <param name="color32List">Color32 list.</param>
        public static byte[] Color32ListToByteArray(List<Color32> color32List)
        {
            byte[] ret = new byte[color32List.Count * 4];

            int byteIndex = 0;

            for (int i = 0; i < color32List.Count; i++)
            {
                ret[byteIndex] = color32List[i].r;
                ret[byteIndex+1] = color32List[i].g;
                ret[byteIndex+2] = color32List[i].b;
                ret[byteIndex+3] = color32List[i].a;
                byteIndex += 4;
            }

            return ret;
        }

        /// <summary>
        /// Unpacks a byte array into a Color32 list.
        /// </summary>
        /// <returns>The byte array.</returns>
        /// <param name="byteArray">Byte array.</param>
        public static List<Color32> Color32ListFromByteArray(byte[] byteArray)
        {
            List<Color32> ret = new List<Color32>();

            for (int i = 0; i < byteArray.Length; i += 4)
            {
                Color32 color32 = new Color32(byteArray[i], byteArray[i + 1], byteArray[i + 2], byteArray[i + 3]);
                ret.Add(color32);
            }

            return ret;
        }
    }
}
