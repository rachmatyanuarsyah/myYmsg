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
using MyYmsg;
namespace test
{
	class Program
	{
		public static void Main(string[] args)
		{
			ymsgNet ymsg_dr = new ymsgNet();
			
			Console.Write("Input UserId :");
			string userId=Console.ReadLine();
			Console.Write("Input Password:");
			string Password=Console.ReadLine();
			
			ymsg_dr.username=userId;
			ymsg_dr.password=Password;
			
			ymsg_dr.Connect();
			
			Console.WriteLine("wait for conection. . . ");
			if (ymsg_dr.ConnectionStatus== ConnectionStatus.Success) {
				Console.WriteLine("Yahoo Authenticating... Please Wait!!");
				ymsg_dr.Login();
				if (ymsg_dr.LoginStatus==LoginStatus.Success){
					Console.WriteLine("Yahoo logged In!!");
				}else{
					Console.WriteLine("Bad Name!!");
				}
			}else{
				Console.WriteLine("Not Connecting!!");
			}
			//listening to recevie data
			ymsg_dr.ReadyToReceive();
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadLine();
			
			Console.Write("Input User to send:");
			string TargetId=Console.ReadLine();
			
			ymsg_dr.target=TargetId;
			while (true) // Loop indefinitely
			{
			    Console.Write("Input messege : ");
			   	string t =Console.ReadLine();
			    if (t == "exit") // Check string
			    {
					break;
			    }
			    ymsg_dr.SendPM(t);
			    Console.Write("Press any key to continue . . . ");
				Console.ReadLine();
			}
		}
	}
}