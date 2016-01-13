/*
 * 	Author : Rachmat Y
 *  Copyright (C) 2015
 * 	This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.

 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.

 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace MyYmsg
{
	/// <summary>
	/// Description of ByteArray.
	/// </summary>
	public static class ByteArray
	{
		public static byte[] GetByteArray(string s)
		{
			var bytes = new byte[s.Length];
			for (int i = 0; i < s.Length; ++i) bytes[i] = (byte)s[i];
			return bytes;
		}

		/// <summary>
		/// Returns the UTF-8 string of the byte-array data.
		/// </summary>
		/// <param name="data">The byte array that contains the data.</param>
		/// <returns>The UTF-8 string.</returns>
		public static string ToString(byte[] data)
		{ return Encoding.UTF8.GetString(data); }

		/// <summary>
		/// Compares 2 byte-arrays with the starting index of each arrays and the count of compared bytes. 
		/// Returns 1 if s1 is higher than s2, 0 if s1 is equal than s2 and -1 if s1 is less than s2.
		/// </summary>
		/// <param name="s1">The first byte-array.</param>
		/// <param name="index1">The starting index in s1 array.</param>
		/// <param name="s2">The second byte-array.</param>
		/// <param name="index2">The starting index in s2 array.</param>
		/// <param name="count">The count of compared bytes.</param>
		/// <returns>1 if s1 is higher than s2, 0 if s1 is equal than s2 and -1 if s1 is less than s2.</returns>
		public static int Compare(byte[] s1, int index1, byte[] s2, int index2, int count)
		{
			for (; count > 0; ++index1, ++index2, --count)
			{
				if (s1[index1] > s2[index2]) return 1;
				else if (s1[index1] < s2[index2]) return -1;
			}

			return 0;
		}

		/// <summary>
		/// Parses uint value from the byte-array (ASCII string) with the starting index in the array and count of bytes that will be parsed.
		/// </summary>
		/// <param name="data">The byte array that contains the ASCII string.</param>
		/// <param name="index">The starting index in the array.</param>
		/// <param name="count">The count of bytes that will be parsed.</param>
		/// <returns>The uint value.</returns>
		public static uint ParseUInt(byte[] data, int index, int count)
		{
			int t = 1;
			uint num = 0;

			for (int i = index + count - 1; i >= index; --i, t *= 10)
				num += (uint)(data[i] * t);

			return num;
		}

		/// <summary>
		/// Parses uint value from the byte-array (ASCII string).
		/// </summary>
		/// <param name="data">The byte array that contains the ASCII string.</param>
		/// <returns>The uint value.</returns>
		public static uint ParseUInt(byte[] data)
		{ return ParseUInt(data, 0, data.Length); }
	}
}
