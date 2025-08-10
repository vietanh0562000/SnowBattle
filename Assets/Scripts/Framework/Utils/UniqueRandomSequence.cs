/// <summary>
/// Unique random sequence generator
/// Generate sequence of random numbers, each number will be generated just once until the sequence is complete.
/// Implements avoiding last number in the current sequence and fisrt in number in next sequence to be equal.
/// 
/// Create by Jorge L. Chavez Herrera.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework.Utils
{
	public class UniqueRandomSequence
	{
		List<int> sequence = new List<int>();
		int length = 0;
		int previousLastRandom = -1;
		
		static private Dictionary<string, UniqueRandomSequence> staticDict = new Dictionary<string, UniqueRandomSequence>();
		
		/// <summary>
		/// Initializes a new instance of the <see cref="FiveRonin.Framework.Utils.UniqueRandomSequence"/> class.
		/// </summary>
		/// <param name="length">Length.</param>
		public UniqueRandomSequence (int length)
		{
			this.length = length;
			GenerateNewSequence ();
		}
		
		/// <summary>
		/// Gets the next random value in the sequence.
		/// </summary>
		/// <value>The next value.</value>
		public int nextValue
		{
			get 
			{
				int ret = sequence[0];

				sequence.RemoveAt(0);
				
				if  (sequence.Count == 0)
				{
					previousLastRandom = ret;
					GenerateNewSequence ();
				}
				
				return ret;
			}
		}
		
		private void GenerateNewSequence ()
		{
			sequence.Clear ();
			
			int newRandom = Random.Range (0, length);
			
			for (int i = 0; i < length; i++)
			{
				while (sequence.Contains (newRandom) || newRandom == previousLastRandom)
				{
					newRandom = Random.Range (0, length);
					previousLastRandom = -1;
				}
				
				sequence.Add (newRandom);
			}
		}
		
		/// <summary>
		/// Gets the a unique random number for a sequence stored in the static dictionary.
		/// This might be useful to share sequences among mltiple instances from the same class.
		/// </summary>
		/// <returns>The unique random.</returns>
		/// <param name="key">Key.</param>
		/// <param name="length">Length.</param>
		static public int GetRandomForKey (string key, int length)
		{
			if (staticDict.ContainsKey (key) == false)
			{
				staticDict.Add (key, new UniqueRandomSequence (length)); 
			}

			return staticDict[key].nextValue;
		}
	}
}
