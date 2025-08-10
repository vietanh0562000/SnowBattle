using UnityEngine;
using Framework.Utils;

/// <summary>
/// Array Extensions
/// Extension functions for built in arrays
/// 
/// By Jorge L. CHavez Herrera.

public static class ArrayExtensions
{
    #region Class implementation
    public static T[] RandomizeArrayOrder<T>(T[] array)
    {
        if (array == null)
            return array;

        UniqueRandomSequence random = new UniqueRandomSequence(array.Length);

        T[] ret = new T[array.Length];

        for (int i = 0; i < ret.Length; i++)
            ret[i] = array[random.nextValue];

        return ret;
    }
    #endregion
}