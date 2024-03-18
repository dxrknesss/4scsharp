namespace practice2;

struct Product
{
  public string ProductName;
  public DateOnly AdditionDate;
  public double WeightKg;
  public decimal PricePer1KgUSD;
  public string SupplierName;
  public int MaxStoragePeriodDays;

  public Product(
      string ProductName, DateOnly AdditionDate,
      double WeightKg, decimal PricePer1KgUSD,
      string SupplierName, int MaxStoragePeriodDays
      )
  {
    this.ProductName = ProductName;
    this.AdditionDate = AdditionDate;
    this.WeightKg = WeightKg;
    this.PricePer1KgUSD = PricePer1KgUSD;
    this.SupplierName = SupplierName;
    this.MaxStoragePeriodDays = MaxStoragePeriodDays;
  }

  override public string ToString() // method that overrides object's ToString
  {
    return $"Product {this.ProductName}, added {this.AdditionDate}, " +
      $"weighs {this.WeightKg}kg and costs ${this.PricePer1KgUSD} per 1 kg, " +
      $"it was supplied by {this.SupplierName} and can be stored for {this.MaxStoragePeriodDays} days from supply date.";
  }
}
