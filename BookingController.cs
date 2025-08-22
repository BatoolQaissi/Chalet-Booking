using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingApp.Data;
using BookingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BookingApp.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly AppContextDB _context;
        private readonly UserManager<User> _userManager;

        public BookingController(AppContextDB context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(User);

            if (user == null) { return NotFound(); }

            ViewBag.ReturnAction = "Index";

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return View(await _context.Booking
                    .Include(b => b.Offer).Include(b => b.User).Include(b => b.Offer.Accommodation).ToListAsync());
            }
            else
            {
                return View(await _context.Booking
                    .Where(b => b.UserId == user.Id)
                    .Include(b => b.Offer).Include(b => b.User).Include(b => b.Offer.Accommodation).ToListAsync());
            }
        }

        // GET: Booking/HostIndex
        [Authorize(Roles = "Host, Admin")]
        public async Task<IActionResult> HostIndex()
        {
            User user = await _userManager.GetUserAsync(User);

            if (user == null) { return NotFound(); }

            ViewBag.ReturnAction = "HostIndex";

            return View("Index", await _context.Booking
                .Include(b => b.Offer).Include(b => b.User).Include(b => b.Offer.Accommodation)
                .Where(b => b.Offer.Accommodation.UserId == user.Id).ToListAsync());
        }


        // POST: Booking/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(Guid id, string returnAction)
        {
            var booking = await _context.Booking
                .Include(b => b.Offer)
                .Include(b => b.User)
                .Include(b => b.Offer.Accommodation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            ViewBag.ReturnAction = returnAction;

            return View(booking);
        }

        // POST: Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("OfferId, ArrivalDate, ArrivalTime, DepartureDate, DepartureTime, NbPerson")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // التحقق من وجود حجز متداخل لنفس الشقة
                bool isOverlapping = await _context.Booking.AnyAsync(b =>
                    b.OfferId == booking.OfferId &&
                    (
                        (booking.ArrivalDate < b.DepartureDate && booking.DepartureDate > b.ArrivalDate)
                    )
                );
                if (isOverlapping)
                {
                    TempData["AlertType"] = "danger";
                    TempData["AlertMsg"] = "الشقة محجوزة بالفعل في الفترة المطلوبة. يرجى اختيار تواريخ أخرى.";
                    return RedirectToAction("View", "Offer", new { id = booking.OfferId });
                }

                int nbNight = (booking.DepartureDate - booking.ArrivalDate).Days;
                double pricePerNight = await _context.Offers.Where(o => o.Id == booking.OfferId).Select(o => o.PricePerNight).SingleOrDefaultAsync();
                double cleaningFee = await _context.Offers.Where(o => o.Id == booking.OfferId).Select(o => o.CleaningFee).SingleOrDefaultAsync();

                User senderUser = await _userManager.GetUserAsync(User);
                User receiverUser = await _context.Offers.Where(o => o.Id == booking.OfferId).Select(o => o.Accommodation.User).SingleOrDefaultAsync();

                // حساب السعر الكلي
                double totalPrice = pricePerNight * (double)nbNight + cleaningFee;

                // تخطي التحقق من الرصيد وتنفيذ الحجز مباشرة
                booking.TotalPrice = totalPrice;
                booking.UserId = (await _userManager.GetUserAsync(User)).Id;

                _context.Add(booking);
                await _context.SaveChangesAsync();
                
                // إضافة رسالة نجاح
                TempData["AlertType"] = "success";
                TempData["AlertMsg"] = "تم إتمام الحجز بنجاح!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
