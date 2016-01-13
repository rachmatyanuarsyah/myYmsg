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

namespace MyYmsg
{
	/// <summary>
	/// Description of Packet.
	/// </summary>
	public class Packet
	{
		readonly byte[] SIGNATURE = ByteArray.GetByteArray("YMSG");

		#region Attributes

		/// <summary>
		/// Gets or sets the ushort value that is the protocol version.
		/// </summary>
		public ushort Version { get; set; }

		/// <summary>
		/// Gets or sets the ushort value that is the vendor ID.
		/// </summary>
		public ushort VendorID { get; set; }

		/// <summary>
		/// Gets or sets the ushort value that is the length of the data.
		/// </summary>
		public ushort Length { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="vIT.Net.YMSG.Enumerations.PacketService"/> enumeration that is the service that the packet request or response.
		/// </summary>
		public PacketService Service { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="vIT.Net.YMSG.Enumerations.PacketStatus"/> enumeration that is the status of the packet.
		/// </summary>
		public PacketStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="vIT.Net.YMSG.PacketData"/> object that contains the data in key-value format.
		/// </summary>
		public PacketData Data { get; set; }

		/// <summary>
		/// Gets or sets the uint value that is the session ID.
		/// </summary>
		public uint SessionID { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of <see cref="vIT.Net.YMSG.Packet"/> class that contains the YMSG packet.
		/// </summary>
		public Packet()
		{
			this.Data = new PacketData();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Reads the packet from the byte array with the starting index and count of bytes. Returns the number of bytes read.
		/// </summary>
		/// <param name="data">The byte array that contains the data of the packet.</param>
		/// <param name="index">The starting index in the array.</param>
		/// <param name="count">The count of bytes that can be read.</param>
		/// <returns>The number of bytes read.</returns>
		public int Read(byte[] data, int index, int count)
		{
			//
			// Reads the header of the packet.
			if (count < 20) return 0;
			if (ByteArray.Compare(data, 0, SIGNATURE, 0, 4) != 0) return 0;

			this.Version = (ushort)((data[4] << 8) | data[5]);
			this.VendorID = (ushort)((data[6] << 8) | data[7]);
			this.Length = (ushort)((data[8] << 8) | data[9]);
			this.Service = (PacketService)((data[10] << 8) | data[11]);
			this.Status = (PacketStatus)((data[12] << 24) | (data[13] << 16) | (data[14] << 8) | data[15]);
			this.SessionID = (uint)((data[16] << 24) | (data[17] << 16) | (data[18] << 8) | data[19]);
		
			//
			// Reads the packet data.
			return this.Data.Read(data, index + 20, count - 20) + 20;
		}

		/// <summary>
		/// Writes the packet into the byte array with the starting index in the array. Returns the number of bytes that were written.
		/// </summary>
		/// <param name="data">The byte array that will be write into.</param>
		/// <param name="index">The starting index in the array.</param>
		/// <returns>Returns the number of bytes that were written.</returns>
		public int Write(byte[] data, int index)
		{
			//
			// Writes the packet data.
			this.Length = (ushort)this.Data.Write(data, index + 20);

			//
			// Writes the header of the packet.
			Array.Copy(SIGNATURE, 0, data, index, 4);
			Array.Copy(
				new byte[]
				{
					(byte)(this.Version >> 8), (byte)(this.Version & 0xFF),
					(byte)(this.VendorID >> 8), (byte)(this.VendorID & 0xFF),
					(byte)(this.Length >> 8), (byte)(this.Length & 0xFF),
					(byte)((ushort)this.Service >> 8), (byte)((ushort)this.Service & 0xFF),
					(byte)((uint)this.Status >> 24), (byte)(((uint)this.Status >> 16) & 0xFF), (byte)(((uint)this.Status >> 8) & 0xFF), (byte)((uint)this.Status & 0xFF),
					(byte)(this.SessionID >> 24), (byte)((this.SessionID >> 16) & 0xFF), (byte)((this.SessionID >> 8) & 0xFF), (byte)(this.SessionID & 0xFF)
				},
				0, data, 4, 16);

			return this.Length + 20;
		}

		#endregion
	}
}
