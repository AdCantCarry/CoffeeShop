using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Models;

namespace CoffeeShop.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}