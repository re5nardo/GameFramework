

public delegate void DefaultHandler();
public delegate void BoolHandler(bool bValue);
public delegate void MessageHandler(IMessage msg);

public enum Messages
{
	TEST_MESSAGE_ID = 0,
}