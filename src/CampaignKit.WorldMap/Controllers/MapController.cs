﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CampaignKit.WorldMap.Entities;

namespace CampaignKit.WorldMap.Controllers
{
    public class HomeController : Controller
    {
        private readonly MappingContext _context;

        public HomeController(MappingContext context)
        {
            _context = context;
        }

        // GET: Map
        public async Task<IActionResult> Index()
        {
            return View(await _context.Maps.ToListAsync());
        }

        // GET: Map/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var map = await _context.Maps
                .FirstOrDefaultAsync(m => m.MapId == id);
            if (map == null)
            {
                return NotFound();
            }

            return View(map);
        }

        // GET: Map/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Map/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MapId,AdjustedSize,ContentType,Copyright,CreationTimestamp,FileExtension,MaxZoomLevel,Name,RepeatMapInX,Secret,ThumbnailPath")] Map map)
        {
            if (ModelState.IsValid)
            {
                _context.Add(map);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(map);
        }

        // GET: Map/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var map = await _context.Maps.FindAsync(id);
            if (map == null)
            {
                return NotFound();
            }
            return View(map);
        }

        // POST: Map/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MapId,AdjustedSize,ContentType,Copyright,CreationTimestamp,FileExtension,MaxZoomLevel,Name,RepeatMapInX,Secret,ThumbnailPath")] Map map)
        {
            if (id != map.MapId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(map);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MapExists(map.MapId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(map);
        }

        // GET: Map/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var map = await _context.Maps
                .FirstOrDefaultAsync(m => m.MapId == id);
            if (map == null)
            {
                return NotFound();
            }

            return View(map);
        }

        // POST: Map/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var map = await _context.Maps.FindAsync(id);
            _context.Maps.Remove(map);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MapExists(int id)
        {
            return _context.Maps.Any(e => e.MapId == id);
        }
    }
}
