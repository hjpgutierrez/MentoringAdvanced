namespace Carting.BLL.Interfaces
{
    public interface IMessageProcessor
    {
        Task ProcessMessageAsync(string message);
    }
}
