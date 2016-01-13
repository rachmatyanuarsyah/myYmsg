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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using MyYmsg.Packets;

namespace MyYmsg
{
	public delegate void YMSGEvent(ymsgNet yahoo);
	
	/// <summary>
	/// Yahoo! Messenger Protocol Client.
	/// </summary>
	public partial class ymsgNet
	{
		const int BUFFER_SIZE = 100000;
		const int TIMEOUT = 10000;	// 10s

		#region Fields
		
		private InitOptions Options;
		private TcpClient Tcp;
		private NetworkStream Stream;
		
		private string Seed, Token, CookieT, CookieY, CookieB,YCrumb,response_data,crypted;
		
		public delegate bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors);
		public bool ValidateServerCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors ssl)
{
	return true;
}
		#endregion

		#region Events

		/// <summary>
		/// Occurs when the connection status was changed.
		/// </summary>
		public event YMSGEvent ConnectionStatusChanged;

		/// <summary>
		/// Occurs when the loging-in status was changed.
		/// </summary>
		public event YMSGEvent LoginStatusChanged;

		#endregion

		#region Properties
		public string username{ get; set; }
		public string password{ get; set; }
		public string target{ get; set; }
		private ConnectionStatus __connectionStatus = ConnectionStatus.NotConnected;
		private LoginStatus __loginStatus = LoginStatus.NotAuthenticated;
				
		/// <summary>
		/// Gets the <see cref="MyYmsg.ConnectionStatus"/> enumeration that is the status of connection.
		/// </summary>
		public ConnectionStatus ConnectionStatus
		{
			get { return this.__connectionStatus; }
			set
			{
				this.__connectionStatus = value;
				if (this.ConnectionStatusChanged != null) this.ConnectionStatusChanged(this);
			}
		}

		/// <summary>
		/// Gets the <see cref="MyYmsg.LoginStatus"/> enumeration that is the status of loging-in.
		/// </summary>
		public LoginStatus LoginStatus
		{
			get { return this.__loginStatus; }
			set
			{
				this.__loginStatus = value;
				if (this.LoginStatusChanged != null) this.LoginStatusChanged(this);
			}
		}
	
		/// <summary>
		/// Gets the bool value whether is using SSL.
		/// </summary>
		public bool UseSSL { get; private set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of <see cref="MyYmsg.ymsgNet"/> class.
		/// </summary>
		public ymsgNet()
		{
			this.Options = new InitOptions();
		}

		/// <summary>
		/// Initializes a new instance of <see cref="MyYmsg.ymsgNet"/> class with the initializing options.
		/// </summary>
		/// <param name="options">The initializing options.</param>
		public ymsgNet(InitOptions options)
		{
			this.Options = options;
		}

		#endregion

		#region Sends and receives data

		/// <summary>
		/// Makes ready to receive data.
		/// </summary>
		public void ReadyToReceive()
		{
			var buf = new byte[BUFFER_SIZE];
			this.Stream.BeginRead(buf, 0, BUFFER_SIZE, new AsyncCallback(this.OnDataReceived), new ReadAsyncData() { Buffer = buf, Stream = this.Stream });
		}

		/// <summary>
		/// Handles the received data.
		/// </summary>
		private void OnDataReceived(IAsyncResult r)
		{
			var info = (ReadAsyncData)r.AsyncState;
			int len = info.Stream.EndRead(r);
			this.ReadyToReceive();
//			if(len>650){
//				return;
//			}
			//
			// Reads the packet.
			var packet = new Packet();
			packet.Read(info.Buffer, 0, len);
			this.ProcessPacket(packet);
		}

		/// <summary>
		/// Sends and receives packet synchronously.
		/// </summary>
		/// <param name="packet">The request packet.</param>
		/// <returns>The response packet.</returns>
		private Packet SendAndReceivePacket(Packet packet)
		{
			//
			// Sends
			var buf = new byte[BUFFER_SIZE];
			int len = packet.Write(buf, 0);
			
			this.Stream.Write(buf, 0, len);
			
			//
			// Receives
			len = this.Stream.Read(buf, 0, BUFFER_SIZE);
			var resp = new Packet();
			resp.Read(buf, 0, len);
			return resp;
		}
		
		/// <summary>
		/// Sends a YMSG packet.
		/// </summary>
		/// <param name="packet">The YMSG packet.</param>
		private IAsyncResult SendPacket(Packet packet)
		{
			var buf = new byte[BUFFER_SIZE];
			int len = packet.Write(buf, 0);
			return this.Stream.BeginWrite(buf, 0, len, new AsyncCallback(this.OnDataSent), null);
		}

		/// <summary>
		/// Handles the sent data.
		/// </summary>
		/// <param name="r"></param>
		private void OnDataSent(IAsyncResult r)
		{
			this.Stream.EndWrite(r);
		}

		#endregion

		#region Connects to server

		/// <summary>
		/// Connects to the server.
		/// </summary>
		public void Connect()
		{
			//
			// Connects to the server with hostname and port.
			if (this.Tcp != null) this.Tcp.Close();
			try { this.Tcp = new TcpClient(this.Options.Host, this.Options.Port); }
			catch
			{
				this.ConnectionStatus = ConnectionStatus.NotConnected;
				return;
			}
			if (!this.Tcp.Connected)
			{
				this.ConnectionStatus = ConnectionStatus.NotConnected;
				return;
			}

			//
			// Creates the network stream.
			this.Stream = this.Tcp.GetStream();
			
			//
			// Verifies server.
			var packet = this.SendAndReceivePacket(new VerifyRequest());
			if (packet.Service == PacketService.Verify && packet.Status == PacketStatus.ServerAck) this.ConnectionStatus = ConnectionStatus.Success;
			else this.ConnectionStatus = ConnectionStatus.NotVerified;
		}

		#endregion

		#region Processes the received data

		/// <summary>
		/// Processes the received packet.
		/// </summary>
		/// <param name="packet">The YMSG packet.</param>
		private void ProcessPacket(Packet packet)
		{
			switch (packet.Service)
			{
				case PacketService.YAHOO_SERVICE_Y8_LIST:
					break;
				case PacketService.YAHOO_SERVICE_LIST:
					this.LoginStatus = LoginStatus.Success;
					break;
				case PacketService.YAHOO_SERVICE_NOTIFY:
					break;
				case PacketService.YAHOO_SERVICE_MESSAGE:
					Console.WriteLine(ByteArray.ToString(packet.Data[14]));
					break;
			}
			
		}

		#endregion

		#region Login

		/// <summary>
		/// Login with the username and password.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		public void Login()
		{
			if (this.ConnectionStatus != ConnectionStatus.Success) throw new Exception("[YMSG] Server haven\'t been connected !");
			if (string.IsNullOrEmpty(username)) throw new NullReferenceException("[YMSG] Username can\'t be null or empty !");
			if (string.IsNullOrEmpty(password)) throw new NullReferenceException("[YMSG] Password can\'t be null or empty !");
			
			this.Authentication();
			this.AuthUser();
			this.AuthConnection();
			this.Authrespons();
		}

		/// <summary>
		/// Authenticates the user with username.
		/// </summary>
		/// <param name="username">The username.</param>
		private void Authentication()
		{
			var packet = this.SendAndReceivePacket(new AuthenticationRequest(this.username));

			if (packet.Service == PacketService.AuthenticationRequest && packet.Status == PacketStatus.ServerAck)
			{
				

				this.Seed = ByteArray.ToString(packet.Data[94]);
				this.UseSSL = (ByteArray.ParseUInt(packet.Data[13]) == 2);
			}
			else this.LoginStatus = LoginStatus.AuthenticationFailure;
		}
		/// <summary>
		/// Authenticates the user with username and password.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <param name="seed">Seed code.</param>
		private void AuthUser()
		{
			HttpWebResponse response = null;

			//
			// Authenticates by HTTPS
			try
			{
				ServicePointManager.ServerCertificateValidationCallback= new RemoteCertificateValidationCallback(ValidateServerCertificate);
				var req =  (HttpWebRequest)WebRequest.Create(string.Concat("https://login.yahoo.com/config/pwtoken_get?src=ymsgr&ts=&login="+this.username+"&passwd="+this.password+"&chal=", Uri.EscapeUriString(this.Seed)));
                req.Headers.Add("Accept-Language", "en-us");
	            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
	            req.ProtocolVersion = HttpVersion.Version10;
                response = (HttpWebResponse)req.GetResponse();
			}
			catch
			{
				this.LoginStatus = LoginStatus.ConnectionError;
				return;
			}

			//
			// Reads the response
			var encode = Encoding.GetEncoding(950);
			using (var r = new StreamReader(response.GetResponseStream(), encode))
			{
				//
				// Gets the error code.
				string s = r.ReadLine();
				try { this.LoginStatus = (LoginStatus)int.Parse(s); }
				catch { this.LoginStatus = LoginStatus.UnknownError; }

				//
				// Gets the token
				if (this.LoginStatus == LoginStatus.Success)
				{
					s = r.ReadLine();
					if (s.StartsWith("ymsgr=")) this.Token = s.Substring(6);
					else this.LoginStatus = LoginStatus.UnknownError;
				}
			}

			response.Close();
		}

		/// <summary>
		/// Authenticates the connection.
		/// </summary>
		private void AuthConnection()
		{
			HttpWebResponse receiveStream = null;

			//
			// Authenticates by HTTPS.
			try
			{
			
				var req = (HttpWebRequest)WebRequest.Create(string.Concat("https://login.yahoo.com/config/pwtoken_login?src=ymsgr&ts=&token=", Uri.EscapeUriString(this.Token)));
				req.Headers.Add("Accept-Language", "en-us");
	            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
	            req.ProtocolVersion = HttpVersion.Version11;
				receiveStream = (HttpWebResponse)req.GetResponse();
			}
			catch
			{
				this.LoginStatus = LoginStatus.ConnectionError;
				return;
			}

			//
			// Reads the response.
			Stream response = receiveStream.GetResponseStream();
			var encode = Encoding.GetEncoding(950);
        	var readStream = new StreamReader (response, encode);
        	this.response_data=readStream.ReadToEnd();
        	
			//
			// Gets the Y - cookie.http
			int y_start = this.response_data.IndexOf("Y=")+2;
			int y_end = this.response_data.IndexOf(';', y_start);
			this.CookieY =this.response_data.Substring(y_start, y_end - y_start)+"; path=/; domain=.yahoo.com";
			
			//
			// Gets the T - cookie.http
			int t_start = this.response_data.IndexOf("T=") + 2;
			int t_end =this.response_data.IndexOf(';', t_start);
			this.CookieT =this.response_data.Substring(t_start, t_end - t_start)+"; path=/; domain=.yahoo.com";
			//
			// Gets the B - cookie.
			this.CookieB = (char)9 +this.response_data.Substring(t_start, t_end - t_start);
			int c_start = this.response_data.IndexOf("crumb=")+6;
			int c_end =this.response_data.IndexOf(Environment.NewLine, c_start);
            this.YCrumb =this.response_data.Substring(c_start, c_end - c_start);
			this.crypted = ProcessAuth16(YCrumb, Seed);
			receiveStream.Close();
		}
		private string ProcessAuth16(string Crumb, string Challenge){
			string Crypt = string.Join(string.Empty, new string[] {Crumb,Challenge});
			byte[] Hash = HashAlgorithm.Create("MD5").ComputeHash(Encoding.Default.GetBytes(Crypt));
			string Auth = Convert.ToBase64String(Hash).Replace("+", ".").Replace("/", "_").Replace("=", "-");
			return Auth;
		}
		private void Authrespons()
		{
			var packet =this.SendAndReceivePacket(new AuthResponse(this.username,this.CookieY, this.CookieT, this.CookieB, this.crypted));
			if (packet.Service == PacketService.YAHOO_SERVICE_LIST) this.LoginStatus = LoginStatus.Success;
			else this.LoginStatus = LoginStatus.AuthenticationFailure;
		}
		#endregion
	
		#region Send Messege
		public void SendPM(string text)
		{
			this.SendPacket(new SendPm(this.username,this.target, text));
		}
		#endregion
	}
}
