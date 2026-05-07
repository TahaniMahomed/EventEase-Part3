/* S!--CODE ATTRIBUTION-->
<!--TITLE: (CLDV6211 POE Document)-->
<!--AUTHOR: (The Independent Institute of Education / Varsity College)-->
<!--DATE: (13 April 2026)-->
<!--VERSION: (POE Assignment Document)-->
<!--AVAILABLE: (https://advtechonline.sharepoint.com/:w:/r/sites/TertiaryStudents/_layouts/15/Doc.aspx?sourcedoc=%7B50C308BE-32AB-485D-83BA-81B7AF8B57CB%7D&file=CLDV6211POE.docx&action=default&mobileredirect=true)-->
*/
/* S!--CODE ATTRIBUTION-->
<!--TITLE: (ASP.NET Scaffolding in Visual Studio 2013)-->
<!--AUTHOR: (Tom FitzMacken – Microsoft Learn)-->
<!--DATE: (12 April 2026)-->
<!--VERSION: (Visual Studio 2013 – ASP.NET Scaffolding)-->
<!--AVAILABLE: (https://learn.microsoft.com/en-us/aspnet/visual-studio/overview/2013/aspnet-scaffolding-overview)-->
*/
/* S!--CODE ATTRIBUTION
TITLE: Entity Framework Core: Prevent deletion of related data
AUTHOR: Microsoft EF Core Team
DATE: 7 May 2026
VERSION: EF Core 8.0
AVAILABLE: https://learn.microsoft.com/en-us/ef/core/saving/cascade-delete
*/

/* S!--CODE ATTRIBUTION
TITLE: ASP.NET Core: Using TempData for UI Notifications
AUTHOR: Microsoft ASP.NET Core Team
DATE: 7 May 2026
VERSION: ASP.NET Core 8.0
AVAILABLE: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state#tempdata
*/
/* S!--CODE ATTRIBUTION
TITLE: Programming with Mosh: Entity Framework Relationships
AUTHOR: Programming with Mosh (YouTube Channel)
DATE: 7 May 2026
VERSION: Video Tutorial
AVAILABLE: https://www.youtube.com/watch?v=ti5An0vA0z8
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Event_Ease2.Data;
using Event_Ease2.Models;
using Microsoft.AspNetCore.Http;

namespace Event_Ease2.Controllers
{
    public class VenuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Event_Ease2.Services.BlobService _blobService;

        public VenuesController(ApplicationDbContext context, Event_Ease2.Services.BlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Venues
        public async Task<IActionResult> Index(string searchString)
        {
            var venues = from v in _context.Venues select v;

            if (!String.IsNullOrEmpty(searchString))
            {
                venues = venues.Where(s => s.VenueName!.Contains(searchString)
                                        || s.VenueLocation!.Contains(searchString));
            }

            return View(await venues.ToListAsync());
        }

        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues.FirstOrDefaultAsync(m => m.VenueID == id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueID,VenueName,VenueLocation,VenueCapacity")] Venue venue, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                string imageUrl = await _blobService.UploadFileAsync(imageFile, "venue-images");
                venue.ImageURL = imageUrl;
            }
            else
            {
                venue.ImageURL = "no-image-uploaded.png";
            }

            ModelState.Remove("imageFile");
            ModelState.Remove("ImageURL");

            if (ModelState.IsValid)
            {
                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return NotFound();
            return View(venue);
        }

        // POST: Venues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueID,VenueName,VenueLocation,VenueCapacity,ImageURL")] Venue venue)
        {
            if (id != venue.VenueID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues.FirstOrDefaultAsync(m => m.VenueID == id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        // POST: Venues/Delete/5 (UPDATED WITH ERROR HANDLING)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // 1. CHECK FOR ACTIVE BOOKINGS (Requirement: Restrict deletion)
            var hasBookings = await _context.Bookings.AnyAsync(b => b.VenueID == id);

            if (hasBookings)
            {
                // 2. ALERT USER (Requirement: Display alert on validation error)
                TempData["ErrorMessage"] = "Action Denied: This venue has active bookings and cannot be deleted.";
                return RedirectToAction(nameof(Index));
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue != null)
            {
                _context.Venues.Remove(venue);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Venue deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueID == id);
        }
    }
}