using System.Reflection.Metadata;

namespace HoneyPotTrapper.Models.ViewModels
{
    public interface IMessageViewModel
    {
        void SetMessage(string message);
        string AddMessage(string message);
        string GetMessage();
    }

	public class MessageViewModel : IMessageViewModel
    {
        private string Message { get; set; }
        public void SetMessage(string message)
        {
            Message = message;
        }
        public string AddMessage(string message)
        {
            if (string.IsNullOrEmpty(Message))
            {
                Message = message;
            }
            else
            {
                Message = message + "\n" + Message;
            }
            return Message;
        }
        public string GetMessage()
        {
            return Message;
        }

    }


}
