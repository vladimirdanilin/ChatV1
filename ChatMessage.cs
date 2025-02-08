namespace ChatV1
{
    public class ChatMessage
    {
        public ChatMessage()
        {
            
        }

        public ChatMessage(string senderId, string senderName, string message)
        {
            SenderId = senderId;
            SenderName = senderName;
            Message = message;
        }

        public string SenderId { get; set; }

        public string SenderName { get; set; }

        public string Message { get; set; }
    }
}
