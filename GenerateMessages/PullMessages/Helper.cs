namespace PullMessages
{
    using King.Azure.Data;
    using Models;
    public class Helper
    {
        public IQueued<Sample> Message;
        public Sample Data;
    }
}