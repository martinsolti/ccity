namespace CCity.Model;

public class PlaceableTransaction : ITransaction
{
    #region Properties

    /// <summary>
    /// The placeable to which the transaction refers
    /// </summary>
    public Placeable Placeable { get; set; } = null!;

    /// <summary>
    /// Addition or subtraction
    /// </summary>
    public bool Add { get; set; }

    /// <summary>
    /// The amount of the transaction
    /// </summary>
    public uint Amount { get; set; }

    /// <summary>
    /// The type of the transaction
    /// </summary>
    public PlaceableTransactionType TransactionType { get; set; }
    #endregion
}