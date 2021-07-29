using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using RedisDemoMVC.Extensions;
using RedisDemoMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RedisDemoMVC.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        private RedisCache _cache = new RedisCache(new RedisCacheOptions()
        {
            Configuration = ConfigurationManager.ConnectionStrings["Redis"].ToString(),
                InstanceName = "RedisDemo_"
        });

        // GET: Posts
        public async Task<ActionResult> Index()
        {
            string recordKey = "Posts_" + DateTime.Now.ToString("yyyyMMdd_hhmm");
            
            var posts = await _cache.GetReacordAsync<List<Post>>(recordKey);

            if (posts == null)
            {
                posts = _context.Posts.ToList();
                TempData["msg"] = $"Loaded from the DB at { DateTime.Now }";

                await _cache.SetRecordAsync(recordKey, posts);
            }
            else
            {
                TempData["msg"] = $"Loaded from the cache at { DateTime.Now}";
            }

            return View(posts);
        }
    }
}