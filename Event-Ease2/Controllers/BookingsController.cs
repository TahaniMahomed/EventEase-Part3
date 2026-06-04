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
<!--DATE: (12 Apri 2026)-->
<!--VERSION: (Visual Studio 2013 – ASP.NET Scaffolding)-->
<!--AVAILABLE: (https://learn.microsoft.com/en-us/aspnet/visual-studio/overview/2013/aspnet-scaffolding-overview)-->
*/
/* S!--CODE ATTRIBUTION
TITLE: Stack Overflow: Checking for overlapping date ranges
AUTHOR: Stack Overflow Contributors
DATE: 7 May 2026
VERSION: C# Algorithm Example
AVAILABLE: https://stackoverflow.com/questions/13513932/algorithm-to-detect-overlapping-periods
*/

/* S!--CODE ATTRIBUTION
TITLE: Microsoft Learn: Handle Concurrency Exceptions and Validation
AUTHOR: Microsoft ASP.NET Core Team
DATE: 7 May 2026
VERSION: ASP.NET Core 8.0
AVAILABLE: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation
*/

/* S!--CODE ATTRIBUTION
TITLE: TutorialsTeacher: ASP.NET Core MVC Validation
AUTHOR: TutorialsTeacher (YouTube Channel)
DATE: 7 May 2026
VERSION: Video Tutorial
AVAILABLE: https://www.youtube.com/watch?v=Lq10N_BfK0E
*/
/* S!--CODE ATTRIBUTION
TITLE: Microsoft Learn: Add Search to an ASP.NET Core MVC App
AUTHOR: Microsoft ASP.NET Core Team
DATE: 7 May 2026
VERSION: ASP.NET Core 8.0
AVAILABLE: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/search
*/

/* S!--CODE ATTRIBUTION
TITLE: Entity Framework Core: Loading Related Data (Eager Loading)
AUTHOR: Microsoft EF Core Team
DATE: 7 May 2026
VERSION: EF Core 8.0
AVAILABLE: https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
*/

/* S-CODE ATTRIBUTION
TITLE: Model attributes and relationships in Entity Framework Core
AUTHOR: Microsoft Corporation
DATE: 3 June 2023
VERSION: No version specified
AVAILABLE: https://learn.microsoft.com/en-us/ef/core/modeling/
*/

/* S-CODE ATTRIBUTION
TITLE: Perform text search and filter data in ASP.NET Core MVC
AUTHOR: Microsoft Corporation
DATE: 3 June 2026
VERSION: No version specified
AVAILABLE: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/search
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

namespace Event_Ease2.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings (UPDATED FOR PART 3 ADVANCED MULTI-CONDITIONAL FILTERING)
        public async Task<IActionResult> Index(string searchString, int? eventTypeId, DateTime? filterStartDate, DateTime? filterEndDate, int? venueId)
        {
            // Set up lookups for dropdown lists in our filtering view panel
            ViewData["EventTypeID"] = new SelectList(_context.EventTypes, "EventTypeID", "EventTypeName", eventTypeId);
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", venueId);

            // Retain search parameter text strings inside inputs across page updates
            ViewData["CurrentSearch"] = searchString;
            ViewData["FilterStartDate"] = filterStartDate?.ToString("yyyy-MM-ddTHH:mm");
            ViewData["FilterEndDate"] = filterEndDate?.ToString("yyyy-MM-ddTHH:mm");

            // Build our base relational tracking query pipeline
            var bookingsQuery = _context.Bookings
                .Include(b => b.Event)
                    .ThenInclude(e => e.EventType) // Multi-level join for structural category lookups
                .Include(b => b.Venue)
                .AsQueryable();

            // 1. FILTER CONDITION: Part 2 Keyword String Matching (Booking ID or Event Name)
            if (!String.IsNullOrEmpty(searchString))
            {
                bookingsQuery = bookingsQuery.Where(s => s.BookingID.ToString().Contains(searchString)
                                                      || s.Event.EventName.Contains(searchString));
            }

            // 2. FILTER CONDITION: Part 3 Advanced Event Type Selection lookup
            if (eventTypeId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.Event.EventTypeID == eventTypeId.Value);
            }

            // 3. FILTER CONDITION: Part 3 Specific Venue ID filter
            if (venueId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.VenueID == venueId.Value);
            }

            // 4. FILTER CONDITION: Part 3 Date Range Bounds Validation
            if (filterStartDate.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.StartDate >= filterStartDate.Value);
            }

            if (filterEndDate.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.EndDate <= filterEndDate.Value);
            }

            return View(await bookingsQuery.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                    .ThenInclude(e => e.EventType)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["EventID"] = new SelectList(_context.Events, "EventID", "EventName");
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingID,StartDate,EndDate,VenueID,EventID")] Booking booking)
        {
            // 1. VALIDATE END DATE POST-DATING START DATE
            if (booking.EndDate <= booking.StartDate)
            {
                ModelState.AddModelError("EndDate", "Validation Error: The scheduled End Date must fall after the selected Start Date.");
            }

            if (ModelState.IsValid)
            {
                // 2. DOUBLE BOOKING CHECK (CREATE)
                bool isDoubleBooked = await _context.Bookings.AnyAsync(b =>
                    b.VenueID == booking.VenueID &&
                    ((booking.StartDate >= b.StartDate && booking.StartDate < b.EndDate) ||
                     (booking.EndDate > b.StartDate && booking.EndDate <= b.EndDate) ||
                     (booking.StartDate <= b.StartDate && booking.EndDate >= b.EndDate)));

                if (isDoubleBooked)
                {
                    ModelState.AddModelError("", "Validation Error: This venue is already booked for these dates.");
                    ViewData["EventID"] = new SelectList(_context.Events, "EventID", "EventName", booking.EventID);
                    ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", booking.VenueID);
                    return View(booking);
                }

                booking.BookingDate = DateTime.Now;
                _context.Add(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Booking created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["EventID"] = new SelectList(_context.Events, "EventID", "EventName", booking.EventID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", booking.VenueID);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewData["EventID"] = new SelectList(_context.Events, "EventID", "EventName", booking.EventID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", booking.VenueID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingID,StartDate,EndDate,VenueID,EventID")] Booking booking)
        {
            if (id != booking.BookingID) return NotFound();

            // 1. VALIDATE END DATE POST-DATING START DATE
            if (booking.EndDate <= booking.StartDate)
            {
                ModelState.AddModelError("EndDate", "Validation Error: The scheduled End Date must fall after the selected Start Date.");
            }

            if (ModelState.IsValid)
            {
                // 2. DOUBLE BOOKING CHECK (EDIT - EXCLUDING CURRENT RECORD SELF-CONFLICT)
                bool isDoubleBookedOnEdit = await _context.Bookings.AnyAsync(b =>
                    b.BookingID != booking.BookingID &&
                    b.VenueID == booking.VenueID &&
                    ((booking.StartDate >= b.StartDate && booking.StartDate < b.EndDate) ||
                     (booking.EndDate > b.StartDate && booking.EndDate <= b.EndDate) ||
                     (booking.StartDate <= b.StartDate && booking.EndDate >= b.EndDate)));

                if (isDoubleBookedOnEdit)
                {
                    ModelState.AddModelError("", "Validation Error: Cannot update booking because it creates a schedule overlap with an existing reservation.");
                    ViewData["EventID"] = new SelectList(_context.Events, "EventID", "EventName", booking.EventID);
                    ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", booking.VenueID);
                    return View(booking);
                }

                try
                {
                    booking.BookingDate = DateTime.Now;
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Booking updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["EventID"] = new SelectList(_context.Events, "EventID", "EventName", booking.EventID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", booking.VenueID);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                    .ThenInclude(e => e.EventType)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Booking removed successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingID == id);
        }
    }
}