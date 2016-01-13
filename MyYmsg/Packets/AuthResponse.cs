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
	/// Description of AuthResponse.
	/// </summary>
	public class AuthResponse : Packet
	{
		public AuthResponse(string username,string Y_CookiePart,string T_CookiePart,string B_CookiePart,string LoginHash)
		{
			this.Version = 17;
			this.VendorID = 0;
			this.Service = PacketService.AuthenticationResponse;
			this.Status = PacketStatus.Default;
			this.SessionID = 0;
			this.Data.Add(1, Encoding.UTF8.GetBytes(username));
			this.Data.Add(0, Encoding.UTF8.GetBytes(username));
			this.Data.Add(277, Encoding.UTF8.GetBytes(Y_CookiePart));
			this.Data.Add(278, Encoding.UTF8.GetBytes(T_CookiePart));
			this.Data.Add(307, Encoding.UTF8.GetBytes(LoginHash));
			this.Data.Add(244,Encoding.UTF8.GetBytes("4194239"));
			this.Data.Add(2, Encoding.UTF8.GetBytes(username));
			this.Data.Add(59,Encoding.UTF8.GetBytes(B_CookiePart));
			this.Data.Add(98, Encoding.UTF8.GetBytes("us"));
			this.Data.Add(135, Encoding.UTF8.GetBytes("9.0.0.2034"));
		}
	}
}
