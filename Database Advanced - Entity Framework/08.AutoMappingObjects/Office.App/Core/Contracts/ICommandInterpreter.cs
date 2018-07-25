namespace Office.App.Core.Contracts
{
    public interface ICommandInterpreter
    {
        string Read(string[] inputTokens);
    }
}
