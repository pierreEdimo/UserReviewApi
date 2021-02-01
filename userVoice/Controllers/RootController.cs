using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace userVoice.Controllers
{
    [ApiController]
    [Route("/")]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var response = new
            {
                href = Url.Link(nameof(GetRoot), null), 

                items = new
                {
                    href= Url.Link(nameof(ItemsController.Getitems), null)
                }, 
                categories = new
                {
                    href=Url.Link(nameof(CategoryController.GetAllCategories), null )
                }, 
             
                itemFromCategory = new
                {
                    href = Url.Link(nameof(ItemsController.GetitemFromCategory),null)
                }, 
        
               allUsers = new
               {
                  href = Url.Link(nameof(UserController.GetAllUsers ), null)
               }, 
               searchWords = new
               {
                   href = Url.Link(nameof(SearchWordsController.GetSearchWords), null )
               }, 
               reviews = new
               {
                   href = Url.Link(nameof(ReviewsController.Getreviews),null )
               }, 
               reviewFromAuthor = new
               {
                   href = Url.Link(nameof(ReviewsController.GetreviewFromAuthor), null )
               }
               

            };

            return Ok(response); 
        }
        
    }
}
