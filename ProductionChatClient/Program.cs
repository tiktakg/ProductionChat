namespace ProductionChat.Client;

public class Program
{
    public static async Task Main(string[] args)
        => await new ChatClientApp().RunAsync();
}