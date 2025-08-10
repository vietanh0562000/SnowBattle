using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


namespace Framework.Extensions
{
	///  ObjectExtensions
	/// Description
	/// By Jorge L. Chávez Herrera.
	public static class ObjectExtensions 
	{
		#region Class implementation
		/// <summary>
		/// Convert an object to a byte array
		/// </summary>
		/// <returns>The to byte array.</returns>
		/// <param name="obj">Object.</param>
		static public byte[] ObjectToByteArray(this Object obj)
		{
			if(obj == null)
				return null;
			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			bf.Serialize(ms, obj);

			return ms.ToArray();
		}

		/// <summary>
		/// Convert a byte array to an Object
		/// </summary>
		/// <returns>The array to object.</returns>
		/// <param name="arrBytes">Arr bytes.</param>
		static public Object ByteArrayToObject(byte[] arrBytes)
		{
			MemoryStream memStream = new MemoryStream();
			BinaryFormatter binForm = new BinaryFormatter();
			memStream.Write(arrBytes, 0, arrBytes.Length);
			memStream.Seek(0, SeekOrigin.Begin);
			Object obj = (Object) binForm.Deserialize(memStream);

			return obj;
		}
		#endregion
	}
}
