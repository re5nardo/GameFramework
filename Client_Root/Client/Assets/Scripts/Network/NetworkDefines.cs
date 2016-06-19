using System.Text;
using System.Net.Sockets;

// State object for receiving data from remote device.
public class StateObject
{
	// Client socket.
	public Socket 		WorkSocket = null;
	// Size of receive buffer.
	public const int 	BufferSize = 256;
	// Receive buffer.
	public byte[] 		Buffer = new byte[BufferSize];
	// Received data string.
	public byte[] 		CurMessage;
	public ushort		CurMessageID;
	public ushort		CurPos;
	public ushort		TotalSize;
}

public enum Messages
{
	TEST_MESSAGE_ID = 0,

    //  ToS
    Ready_For_Start_ToS,
    Game_Event_Move_ToS,

    //  ToC
    Game_Start_ToC,
    Game_Event_Move_ToC,
}

class NetworkDefines
{
	public const int MESSAGE_HEADER_SIZE = 4;	//	msg id(2) + msg length info(2)
}
