namespace Ews.Essentials.Data.FixedStrings
{
    public interface IFixedString
    {
        int Capacity { get; }
        int Length { get; }
        char this[int index] { get; }
        string ToString();
    }
}