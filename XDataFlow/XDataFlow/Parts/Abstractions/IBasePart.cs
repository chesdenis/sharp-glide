namespace XDataFlow.Parts.Abstractions
{
    public interface IBasePart
    {
        string Name { get; set; }
        void PrintStatus(string status);
    }
}