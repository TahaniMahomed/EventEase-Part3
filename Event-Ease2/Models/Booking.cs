/* S!--CODE ATTRIBUTION-->
<!--TITLE: (CLDV6211 POE Document)-->
<!--AUTHOR: (The Independent Institute of Education / Varsity College)-->
<!--DATE: (13 April 2026)-->
<!--VERSION: (POE Assignment Document)-->
<!--AVAILABLE: (https://advtechonline.sharepoint.com/:w:/r/sites/TertiaryStudents/_layouts/15/Doc.aspx?sourcedoc=%7B50C308BE-32AB-485D-83BA-81B7AF8B57CB%7D&file=CLDV6211POE.docx&action=default&mobileredirect=true)-->
*/
/* S!--CODE ATTRIBUTION-->
<!--TITLE: (MVC Music Store Part 6: Using Data Annotations for Model Validation)-->
<!--AUTHOR: (Jon Galloway – Microsoft Learn)-->
<!--DATE: (13 April 2026)-->
<!--VERSION: (ASP.NET MVC 3 – Music Store Tutorial Series)-->
<!--AVAILABLE: (https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/mvc-music-store/mvc-music-store-part-6)-->
*/
/* S!--CODE ATTRIBUTION
TITLE: EF Core Relationships
AUTHOR: Microsoft EF Core Team
DATE: 7 May 2026
VERSION: EF Core 8.0
AVAILABLE: https://learn.microsoft.com/en-us/ef/core/modeling/relationships
*/

/* S!--CODE ATTRIBUTION
TITLE: Data Annotations for Validation
AUTHOR: Microsoft .NET Team
DATE: 7 May 2026
VERSION: .NET Framework / .NET 8.0
AVAILABLE: https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations
*/

using System;
using System.ComponentModel.DataAnnotations;

namespace Event_Ease2.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        [Display(Name = "Start Date & Time")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date & Time")]
        public DateTime EndDate { get; set; }

        // We keep this but hide it from the form
        [Display(Name = "System Timestamp")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        public int VenueID { get; set; }
        public Venue? Venue { get; set; }

        public int EventID { get; set; }
        public Event? Event { get; set; }
    }
}