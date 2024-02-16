using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Controllers.Models
{
    public class Cart
    {
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; }


    }
}
