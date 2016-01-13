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
using System.Text;
namespace MyYmsg.Packets
{
	/// <summary>
	/// Description of AuthenticationRequest.
	/// </summary>
	public class AuthenticationRequest : Packet
	{
		public AuthenticationRequest(string username)
		{
			this.Version = 17;
			this.VendorID = 0;
			this.Service = PacketService.AuthenticationRequest;
			this.Status = PacketStatus.Default;
			this.SessionID = 0;
			this.Data.Add(1, Encoding.UTF8.GetBytes(username));
		}
	}
}
