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

/* S-CODE ATTRIBUTION
TITLE: Building relational database tables using EF Core Code-First approach
AUTHOR: C# Corner Developer Network
DATE: 3 June 2026
VERSION: No version specified
AVAILABLE: https://www.c-sharpcorner.com/article/code-first-approach-in-entity-framework-core/
*/


namespace Event_Ease2.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Venue
    {
        [Key]
        public int VenueID { get; set; }
        [Required]
        public string VenueName { get; set; }
        public string VenueLocation { get; set; }
        public int VenueCapacity { get; set; }
        public string ImageURL { get; set; }  
    }
}