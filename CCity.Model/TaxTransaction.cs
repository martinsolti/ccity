namespace CCity.Model;

public class TaxTransaction : ITransaction  
{
    #region Properties

    /// <summary>
    /// Addition or subtraction
    /// </summary>
    public bool Add { get; set; } = true;

    /// <summary>
    /// Type of the tax
    /// </summary>
    public TaxType TaxType { get; set; }

    /// <summary>
    /// The amount of the transaction
    /// </summary>
    public uint Amount { get; set; }

    #endregion
}