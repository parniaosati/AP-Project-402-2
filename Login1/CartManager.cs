using System.Collections.ObjectModel;

public class CartManager
{
    private static readonly CartManager _instance = new CartManager();
    public static CartManager Instance => _instance;

    private ObservableCollection<CartItem> _cartItems;

    private CartManager()
    {
        _cartItems = new ObservableCollection<CartItem>();
    }

    public void AddToCart(CartItem item)
    {
        _cartItems.Add(item);
    }

    public ObservableCollection<CartItem> GetCartItems()
    {
        return _cartItems;
    }

    public void ClearCart()
    {
        _cartItems.Clear();
    }
}

public class CartItem
{
    public int CustomerId { get; set; }
    public int RestaurantId { get; set; }
    public int MenuId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string RestaurantName { get; set; }
    public decimal Total => Price * Quantity;
}
