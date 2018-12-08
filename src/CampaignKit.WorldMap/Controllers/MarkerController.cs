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
    public class MarkerController : Controller
    {
        private readonly MappingContext _context;

        public MarkerController(MappingContext context)
        {
            _context = context;
        }

        // GET: Marker
        public async Task<IActionResult> Index()
        {
            return View(await _context.Markers.ToListAsync());
        }

        // GET: Marker/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marker = await _context.Markers
                .FirstOrDefaultAsync(m => m.MarkerId == id);
            if (marker == null)
            {
                return NotFound();
            }

            return View(marker);
        }

        // GET: Marker/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Marker/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MarkerId,x,y,Title,MarkerData")] Marker marker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(marker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(marker);
        }

        // GET: Marker/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marker = await _context.Markers.FindAsync(id);
            if (marker == null)
            {
                return NotFound();
            }
            return View(marker);
        }

        // POST: Marker/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MarkerId,x,y,Title,MarkerData")] Marker marker)
        {
            if (id != marker.MarkerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarkerExists(marker.MarkerId))
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
            return View(marker);
        }

        // GET: Marker/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marker = await _context.Markers
                .FirstOrDefaultAsync(m => m.MarkerId == id);
            if (marker == null)
            {
                return NotFound();
            }

            return View(marker);
        }

        // POST: Marker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var marker = await _context.Markers.FindAsync(id);
            _context.Markers.Remove(marker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarkerExists(int id)
        {
            return _context.Markers.Any(e => e.MarkerId == id);
        }
    }
}
