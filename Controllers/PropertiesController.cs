using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rentals.API.Models; 

namespace Rentals.API.Controllers
{
    [ApiController] // tells ASP.NET this class handles API requests and anables automatic model validation
    [Route("api/[controller]")] // maps this controller to /api/properties
    public class PropertiesController : ControllerBase
    {
        // Temporary in-memory list - replaced with real DB in phase 2 
        private static List<Property> _properties = [ 
            new Property{
                Id = 1, 
                Title = "Furnished Flat in DHA",
                Area = "DHA",
                PricePerMonth = 450000, 
                Bedrooms = 2, 
                IsFurnished = true, 
                IsVerified = true,
                ContactNumber = "0300-123456789"
            }, 
            new Property{
                Id = 2,
                Title = "Single Room in Gulshan-e-Iqbal",
                Area = "Gulshan",
                PricePerMonth = 15000,
                Bedrooms = 1,
                IsFurnished = false,
                IsVerified = true,
                ContactNumber = "0321-7654321"
            },
            new Property{
                Id = 3,
                Title = "Portion in North Nazimabad",
                Area = "North Nazimabad",
                PricePerMonth = 25000,
                Bedrooms = 3,
                IsFurnished = false,
                IsVerified = false, // pending admin approval
                ContactNumber = "0333-9876543"
            },
        ];

        // GET api/properties
        [HttpGet]
        // ActionResult<T> lets you return either data Ok(data) OR HTTP status code (NotFound(),NoContent()) 
        //                 this is the coreect return type for every api method 

        public ActionResult<IEnumerable<Property>> GetAll()
        {
            var varified = _properties.Where(p => p.IsVerified).ToList();
            return Ok(_properties);
        }

        // GET api/properties/1
        [HttpGet("{id}")]
        public ActionResult<Property> GetById(int id)
        {
            var property = _properties.Where(p => p.Id == id);
            if (property == null)
                return NotFound();
            return Ok(property);
        }

        // GET api/properties/search?area=DHA&maxPrice=50000
        [HttpGet("search")]
        public ActionResult<IEnumerable<Property>> Search (
            [FromQuery] string ? area ,
            [FromQuery] decimal ? maxPrice,
            [FromQuery] int ? bedrooms,
            [FromQuery] bool ? isFurnished)
        {

            var results = _properties.Where(p => p.IsVerified).AsQueryable(); 
            
            if (!string.IsNullOrEmpty(area)) 
                results = results.Where(p => p.Area.ToLower().Contains(area.ToLower()));

            if (maxPrice.HasValue)
                results = results.Where(p => p.PricePerMonth <= maxPrice.Value);

            if (bedrooms.HasValue)
                results = results.Where(p => p.Bedrooms == bedrooms.Value);

            if (isFurnished.HasValue)
                results = results.Where(p => p.IsFurnished == isFurnished.Value);

            return Ok(results.ToList()); 
        }

        // POST api/properties 
        [HttpPost]
        public ActionResult<Property> Create(Property property)
        {
            property.Id = _properties.Count + 1;
            property.CreatedAt = DateTime.UtcNow;
            property.IsVerified = false; // always starts unvarified 
            _properties.Add(property);

            return CreatedAtAction(nameof(GetById), new { id = property.Id }, property); 
        }

        // PUT api/properties/1 
        [HttpPut("{id}")]
        public ActionResult Update(int id, Property updated)
        {
            var property = _properties.FirstOrDefault(p => p.Id == id);
            if (property == null) return NotFound(); 

            property.Title = updated.Title;
            property.Description = updated.Description ?? property.Description;
            property.Area = updated.Area;
            property.Bedrooms = updated.Bedrooms;
            property.IsAvailable = updated.IsAvailable; 
            property.IsVerified = updated.IsVerified;
            property.IsFurnished = updated.IsFurnished;
            property.ContactNumber = updated.ContactNumber;
            property.PricePerMonth = updated.PricePerMonth; 
            
            return NoContent(); 
        }

        // DELETE api/properties/1
        [HttpDelete]
        public ActionResult Delete(int id) {
            var property = _properties.FirstOrDefault(p => p.Id == id);
            if (property == null) return NotFound(); 

            _properties.Remove(property);
            return NoContent(); 
        }
    }
}
