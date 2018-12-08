using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CampaignKit.WorldMap.Entities;

namespace CampaignKit.WorldMap.Controllers
{
    public class TileController : Controller
    {
        private readonly MappingContext _context;

        public TileController(MappingContext context)
        {
            _context = context;
        }

        // GET: Tile
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tiles.ToListAsync());
        }

        // GET: Tile/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tile = await _context.Tiles
                .FirstOrDefaultAsync(m => m.TileId == id);
            if (tile == null)
            {
                return NotFound();
            }

            return View(tile);
        }

        // GET: Tile/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tile/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TileId,MapId,Path,CreationTimestamp,CompletionTimestamp,TileSize")] Tile tile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tile);
        }

        // GET: Tile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tile = await _context.Tiles.FindAsync(id);
            if (tile == null)
            {
                return NotFound();
            }
            return View(tile);
        }

        // POST: Tile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TileId,MapId,Path,CreationTimestamp,CompletionTimestamp,TileSize")] Tile tile)
        {
            if (id != tile.TileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TileExists(tile.TileId))
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
            return View(tile);
        }

        // GET: Tile/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tile = await _context.Tiles
                .FirstOrDefaultAsync(m => m.TileId == id);
            if (tile == null)
            {
                return NotFound();
            }

            return View(tile);
        }

        // POST: Tile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tile = await _context.Tiles.FindAsync(id);
            _context.Tiles.Remove(tile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TileExists(int id)
        {
            return _context.Tiles.Any(e => e.TileId == id);
        }
    }
}
