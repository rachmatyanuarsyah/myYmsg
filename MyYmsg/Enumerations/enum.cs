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
 
namespace MyYmsg
{	
	#region Conection Status
	/// <summary>
	/// The status of YMSGClient connection.
	/// </summary>
	public enum ConnectionStatus
	{
		/// <summary>
		/// Connected successfully with Yahoo! server.
		/// </summary>
		Success,

		/// <summary>
		/// Not connected.
		/// </summary>
		NotConnected,

		/// <summary>
		/// Connected, but not verified.
		/// </summary>
		NotVerified
	}
	#endregion
	
	#region Login Status
	/// <summary>
	/// Login status.
	/// </summary>
	public enum LoginStatus : int
	{
		Success = 0,
		NotAuthenticated,
		AuthenticationFailure,
		ConnectionError,
		
		IncorrectUsernameOrPassword = 1212,
		SecurityLockTooManyFailed = 1213,
		SecurityLock = 1214,
		UsernameNotFound = 1235,
		UnknownLock = 1216,
		UsernamePasswordBlank = 100,
		UnknownError
	}
	#endregion
	
	#region Packet Service
	/// <summary>
	/// Yahoo! Messenger services
	/// </summary>
	public enum PacketService : ushort
	{
		Logon							= 1,
		Logoff							= 2,
		IsAway							= 3,
		YAHOO_SERVICE_ISBACK,
		YAHOO_SERVICE_IDLE,	/* 5 (placemarker) */
		YAHOO_SERVICE_MESSAGE,
		YAHOO_SERVICE_IDACT,
		YAHOO_SERVICE_IDDEACT,
		YAHOO_SERVICE_MAILSTAT,
		YAHOO_SERVICE_USERSTAT,	/* 0xa */
		YAHOO_SERVICE_NEWMAIL,
		YAHOO_SERVICE_CHATINVITE,
		YAHOO_SERVICE_CALENDAR,
		YAHOO_SERVICE_NEWPERSONALMAIL,
		YAHOO_SERVICE_NEWCONTACT,
		YAHOO_SERVICE_ADDIDENT,	/* 0x10 */
		YAHOO_SERVICE_ADDIGNORE,
		YAHOO_SERVICE_PING,
		YAHOO_SERVICE_GOTGROUPRENAME,	/* < 1, 36(old), 37(new) */
		YAHOO_SERVICE_SYSMESSAGE = 0x14,
		YAHOO_SERVICE_SKINNAME = 0x15,
		YAHOO_SERVICE_PASSTHROUGH2 = 0x16,
		YAHOO_SERVICE_CONFINVITE = 0x18,
		YAHOO_SERVICE_CONFLOGON,
		YAHOO_SERVICE_CONFDECLINE,
		YAHOO_SERVICE_CONFLOGOFF,
		YAHOO_SERVICE_CONFADDINVITE,
		YAHOO_SERVICE_CONFMSG,
		YAHOO_SERVICE_CHATLOGON,
		YAHOO_SERVICE_CHATLOGOFF,
		YAHOO_SERVICE_CHATMSG = 0x20,
		YAHOO_SERVICE_GAMELOGON = 0x28,
		YAHOO_SERVICE_GAMELOGOFF,
		YAHOO_SERVICE_GAMEMSG = 0x2a,
		YAHOO_SERVICE_FILETRANSFER = 0x46,
		YAHOO_SERVICE_VOICECHAT = 0x4A,
		YAHOO_SERVICE_NOTIFY,
		Verify					= 0x4C,
		YAHOO_SERVICE_P2PFILEXFER,
		YAHOO_SERVICE_PEERTOPEER = 0x4F,	/* Checks if P2P possible */
		YAHOO_SERVICE_WEBCAM,
		AuthenticationResponse					= 0x54,
		YAHOO_SERVICE_LIST,
		AuthenticationRequest							= 0x57,
		YAHOO_SERVICE_AUTHBUDDY = 0x6d,
		YAHOO_SERVICE_ADDBUDDY = 0x83,
		YAHOO_SERVICE_REMBUDDY,
		YAHOO_SERVICE_IGNORECONTACT,	/* > 1, 7, 13 < 1, 66, 13, 0 */
		YAHOO_SERVICE_REJECTCONTACT,
		YAHOO_SERVICE_GROUPRENAME = 0x89,	/* > 1, 65(new), 66(0), 67(old) */
		YAHOO_SERVICE_Y7_PING = 0x8A,
		YAHOO_SERVICE_CHATONLINE = 0x96,	/* > 109(id), 1, 6(abcde) < 0,1 */
		YAHOO_SERVICE_CHATGOTO,
		YAHOO_SERVICE_CHATJOIN,	/* > 1 104-room 129-1600326591 62-2 */
		YAHOO_SERVICE_CHATLEAVE,
		YAHOO_SERVICE_CHATEXIT = 0x9b,
		YAHOO_SERVICE_CHATADDINVITE = 0x9d,
		YAHOO_SERVICE_CHATLOGOUT = 0xa0,
		YAHOO_SERVICE_CHATPING,
		YAHOO_SERVICE_COMMENT = 0xa8,
		YAHOO_SERVICE_GAME_INVITE = 0xb7,
		YAHOO_SERVICE_STEALTH_PERM = 0xb9,
		YAHOO_SERVICE_STEALTH_SESSION = 0xba,
		YAHOO_SERVICE_AVATAR = 0xbc,
		YAHOO_SERVICE_PICTURE_CHECKSUM = 0xbd,
		YAHOO_SERVICE_PICTURE = 0xbe,
		YAHOO_SERVICE_PICTURE_UPDATE = 0xc1,
		YAHOO_SERVICE_PICTURE_UPLOAD = 0xc2,
		YAHOO_SERVICE_YAB_UPDATE = 0xc4,
		YAHOO_SERVICE_Y6_VISIBLE_TOGGLE = 0xc5,	/* YMSG13, key 13: 2 = invisible, 1 = visible */
		YAHOO_SERVICE_Y6_STATUS_UPDATE = 0xc6,	/* YMSG13 */
		YAHOO_SERVICE_PICTURE_STATUS = 0xc7,	/* YMSG13, key 213: 0 = none, 1 = avatar, 2 = picture */
		YAHOO_SERVICE_VERIFY_ID_EXISTS = 0xc8,
		YAHOO_SERVICE_AUDIBLE = 0xd0,
		YAHOO_SERVICE_Y7_PHOTO_SHARING = 0xd2,
		YAHOO_SERVICE_Y7_CONTACT_DETAILS = 0xd3,	/* YMSG13 */
		YAHOO_SERVICE_Y7_CHAT_SESSION = 0xd4,
		YAHOO_SERVICE_Y7_AUTHORIZATION = 0xd6,	/* YMSG13 */
		YAHOO_SERVICE_Y7_FILETRANSFER = 0xdc,	/* YMSG13 */
		YAHOO_SERVICE_Y7_FILETRANSFERINFO,	/* YMSG13 */
		YAHOO_SERVICE_Y7_FILETRANSFERACCEPT,	/* YMSG13 */
		YAHOO_SERVICE_Y7_MINGLE = 0xe1,	/* YMSG13 */
		YAHOO_SERVICE_Y7_CHANGE_GROUP = 0xe7,	/* YMSG13 */
		YAHOO_SERVICE_MYSTERY = 0xef,	/* Don't know what this is for */
		YAHOO_SERVICE_Y8_STATUS = 0xf0,	/* YMSG15 */
		YAHOO_SERVICE_Y8_LIST = 0Xf1,	/* YMSG15 */
		YAHOO_SERVICE_MESSAGE_CONFIRM = 0xfb,
		YAHOO_SERVICE_WEBLOGIN = 0x0226,
		YAHOO_SERVICE_SMS_MSG = 0x02ea
	}
	#endregion
	
	#region Packet Status
	public enum PacketStatus : int
	{
		Disconnected				= -1,
		Default						= 0,
		ServerAck					= 1,
		Game						= 0x2,
		Away						= 0x4,
		Continued					= 0x5,
		Invisible					= 12,
		Notify						= 0x16,
		WebLogin					= 0x5a55aa55,
		Offline						= 0x5a55aa56
	}
	#endregion
	
}