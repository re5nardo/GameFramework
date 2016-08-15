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
    ReadyForStartToS_ID = 0,
    GameEventMoveToS_ID,
    JoinLobbyToS_ID,
    SelectNormalGameToS_ID,

    //  ToC
    GameStartToC_ID = 30000,
    GameEventMoveToC_ID,
    JoinLobbyToC_ID,
    SelectNormalGameToC_ID,
}

class NetworkDefines
{
	public const int MESSAGE_HEADER_SIZE = 4;	//	msg id(2) + msg length info(2)
}
