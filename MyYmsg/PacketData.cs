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
	/// Description of PacketData.
	/// </summary>
	public class PacketData: Dictionary<int, byte[]>
	{
		#region Constructor

		public PacketData() : base() { }

		#endregion

		#region Methods

		/// <summary>
		/// Reads the packet data from the byte array with the offset and count of bytes. Returns the count of bytes that were read.
		/// </summary>
		/// <param name="data">The byte array that contains the data.</param>
		/// <param name="index">The starting index in the array.</param>
		/// <param name="count">The count of bytes to be read.</param>
		/// <returns>The count of bytes that were read.</returns>
		public int Read(byte[] data, int index, int count)
		{
			int oldIndex = index;

			while (count > 0)
			{
				
				//
				// Parses the key number.
				var key = ParseValue(data, index, count);
				if (key == null) break;
				index += key.Length + 2;										// + 2 bytes 0xC0 and 0x80
				count -= key.Length + 2;

				//
				// Parses the value.
				var value = ParseValue(data, index, count);
				if (value == null) break;
				index += value.Length + 2;
				count -= value.Length + 2;

				//
				// Converts the key to number.
				int keyInt = 0, j = 1;
				for (int i = key.Length - 1; i >= 0; --i, j *= 10)
					keyInt += (key[i] - '0') * j;
				//Check the key if already add
				if(!this.ContainsKey(keyInt)){
					this.Add(keyInt, value);
				}
			}

			return index - oldIndex;
		}

		/// <summary>
		/// Returns the byte array that was parsed from the data array.
		/// </summary>
		/// <param name="data">The byte array that contains the data.</param>
		/// <param name="index">The starting index in the array.</param>
		/// <param name="count">The count of bytes to be read.</param>
		private byte[] ParseValue(byte[] data, int index, int count)
		{
			int pos = index;

			for (; count > 1; ++pos, --count)
				if (data[pos] == 0xC0 && data[pos + 1] == 0x80)
				{
					var value = new byte[pos - index];
					Array.Copy(data, index, value, 0, pos - index);
					return value;
				}

			return null;
		}

		/// <summary>
		/// Writes the packet data into the data byte-array from the starting index. Returns the number of bytes that were write.
		/// </summary>
		/// <param name="data">The byte array that will be write into.</param>
		/// <param name="index">The starting index in the array.</param>
		/// <returns>The number of bytes that were write.</returns>
		public int Write(byte[] data, int index)
		{
			int pos = index;

			foreach (var item in this)
			{
				var key = Encoding.Default.GetBytes(Convert.ToString(item.Key));

				//
				// Writes the key number.
				Array.Copy(key, 0, data, pos, key.Length);
				pos += key.Length + 2;
				data[pos - 2] = 0xC0;
				data[pos - 1] = 0x80;

				//
				// Writes the value.
				Array.Copy(item.Value, 0, data, pos, item.Value.Length);
				pos += item.Value.Length + 2;
				data[pos - 2] = 0xC0;
				data[pos - 1] = 0x80;
			}

			return pos - index;
		}

		#endregion
	}
}
