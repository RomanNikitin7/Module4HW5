using System.Collections.Generic;
using System.Threading.Tasks;
using ALevelSample.Models;
using ALevelSample.Services.Abstractions;

namespace ALevelSample;

public class App
{
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;

    public App(
        IUserService userService,
        IOrderService orderService,
        IProductService productService)
    {
        _userService = userService;
        _orderService = orderService;
        _productService = productService;
    }

    public async Task Start()
    {
        var firstName = "first name";
        var lastName = "last name";

        var userId = await _userService.AddUser(firstName, lastName);
        await _userService.GetUser(userId);

        await _userService.UpdateUser(userId, "New first Name", "New last Name");

        var product1 = await _productService.AddProductAsync("product1", 4);
        var product2 = await _productService.AddProductAsync("product2", 7);

        await _productService.UpdateProductAsync(product2, "product3", 19);

        var order1 = await _orderService.AddOrderAsync(userId, new List<OrderItem>()
        {
            new OrderItem()
            {
                Count = 3,
                ProductId = product1
            },

            new OrderItem()
            {
                Count = 6,
                ProductId = product2
            },
        });
        await _orderService.DeleteOrderAsync(order1);
        var order2 = await _orderService.AddOrderAsync(userId, new List<OrderItem>()
        {
            new OrderItem()
            {
                Count = 1,
                ProductId = product1
            },

            new OrderItem()
            {
                Count = 9,
                ProductId = product2
            },
        });

        await _orderService.UpdateOrderAsync(order2, userId, new List<OrderItem>()
        {
            new OrderItem()
            {
                Count = 35,
                ProductId = product1
            },

            new OrderItem()
            {
                Count = 100,
                ProductId = product2
            },
        });

        var userOrder = await _orderService.GetOrderByUserIdAsync(userId);

        await _userService.DeleteUser(userId);

        await _productService.DeleteProductAsync(product1);
    }
}