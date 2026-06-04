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
/* S-CODE ATTRIBUTION
TITLE: Passing data from Controller to View via ViewData and SelectList in ASP.NET MVC
AUTHOR: TutorialsTeacher
DATE: 3 June 2026
VERSION: No version specified
AVAILABLE: https://www.tutorialsteacher.com/mvc/viewdata
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
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewBag.EventTypeID = new SelectList(
                _context.EventTypes,
                "EventTypeID",
                "EventTypeName");

            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventID,EventName,EventDescription,EventTypeID")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.EventTypeID = new SelectList(
                _context.EventTypes,
                "EventTypeID",
                "EventTypeName",
                @event.EventTypeID);

            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            // ⬇️ ADD THIS LINE RIGHT HERE TO POPULATE THE DROP LIST ⬇️
            ViewBag.EventTypeID = new SelectList(
                _context.EventTypes,
                "EventTypeID",
                "EventTypeName",
                @event.EventTypeID); // This tells it which category is currently selected!

            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventID,EventName,EventDescription,EventTypeID")] Event @event)
        {
            if (id != @event.EventID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventID)) return NotFound();
                    else throw;
                }
            }

            // ⬇️ ALSO ENSURE THIS GENERATION IS PRESENT HERE IN CASE THE FORM HAS ERRORS ⬇️
            ViewBag.EventTypeID = new SelectList(
                _context.EventTypes,
                "EventTypeID",
                "EventTypeName",
                @event.EventTypeID);

            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5 (UPDATED TO PREVENT BREAKING LIVE DEPENDENCIES)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // 1. CHECK FOR LIVE DEPENDENT BOOKINGS UPFRONT
            var hasActiveBookings = await _context.Bookings.AnyAsync(b => b.EventID == id);

            if (hasActiveBookings)
            {
                // 2. PREVENT DELETION AND SEND CLEAR ALERTS
                TempData["ErrorMessage"] = "Action Denied: This Event cannot be deleted because it is tied to an active venue reservation.";
                return RedirectToAction(nameof(Index));
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventID == id);
        }
    }
}