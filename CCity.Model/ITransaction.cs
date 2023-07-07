namespace CCity.Model;

public interface ITransaction
{
    #region Properties

    /// <summary>
    /// Addition or subtraction
    /// </summary>
    public bool Add { get; }

    /// <summary>
    /// The amount of transaction
    /// </summary>
    public uint Amount { get; }

    #endregion
}