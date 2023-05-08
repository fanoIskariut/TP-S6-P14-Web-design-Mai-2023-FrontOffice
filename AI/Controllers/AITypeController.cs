using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AI.Context;
using AI.Models;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Memory;

namespace AI.Controllers
{
  

    public class AITypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        private readonly IMemoryCache _cache;

       /* public AITypeController(ApplicationDbContext context)
        {
            _context = context;
        }
        

        public AITypeController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }*/
        public AITypeController(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        // GET: AIType
       

        public IActionResult Index(int? idr ,int? page)
        {
            int pageSize = 4; // Nombre d'éléments à afficher par page
            int pageNumber = (page ?? 1); // Numéro de la page actuelle

            var IA = _context.setAI.OrderBy(e => e.id);
            
            if (idr != null)
            {
                IA = (IOrderedQueryable<AIType>)IA.Where(e => e.id == idr);
            }

            int totalIA = IA.Count();

            var pagedIA = IA.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalIA / pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(pagedIA);
        }
        

        // GET: AIType/Details/5
        /*[HttpGet("AIType/Details/{id}")]
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.setAI == null)
            {
                return NotFound();
            }

            var aIType = await _context.setAI
                .FirstOrDefaultAsync(m => m.id == id);
            if (aIType == null)
            {
                return NotFound();
            }

            return View(aIType);
        }*/
        [HttpGet("AIType/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.setAI == null)
            {
                return NotFound();
            }

            // Vérifiez si la valeur est dans le cache
            var cacheKey = $"AIType_Details_{id}";
            if (_cache.TryGetValue(cacheKey, out IActionResult cachedResult))
            {
                return cachedResult;
            }

            var aIType = await _context.setAI
                .FirstOrDefaultAsync(m => m.id == id);
            if (aIType == null)
            {
                return NotFound();
            }

            // Ajoutez la valeur au cache pour les prochaines requêtes
            cachedResult = View(aIType);
            _cache.Set(cacheKey, cachedResult, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            return cachedResult;
        }
    }
}
