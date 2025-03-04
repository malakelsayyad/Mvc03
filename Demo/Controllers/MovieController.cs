using Demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    //MVC Controller
    public class MovieController: Controller 
    {
        //Actions: public non-static method
        //Actions: have special return types
        //ActionResult

        //public string GetMovie(int id)
        //{
        //    return $"Get Movie with id : {id}";
        //}
        //public string AddMovie(int id)
        //{
        //    return $"Get Movie with id : {id}";
        //} 

        //public ContentResult GetMovie(int id)
        //{
        //    var content = new ContentResult() 
        //    { 
        //      Content= $"Get Movie with id : {id}",
        //      ContentType="object/pdf",
        //      StatusCode=200
        //    };
        //    return content;
        //}

        //public RedirectResult GetMovie(int id)
        //{
        //    var redirect = new RedirectResult("https://www.google.com/");
        //    return redirect;
        //}

        //public RedirectToActionResult GetMovie(int id)
        //{
        //    var redirect = new RedirectToActionResult("AddMovie","Movie",new {id=133 });
        //    return redirect;
        //}

        //[ActionName("GetMovie")]
        //[HttpPost]
        //public IActionResult GetMovieByIdWithCategoryDramaAndPoster(int id)
        //{
        //    if (id == 1)
        //    {
        //        //return new ContentResult() { Content = $"Get Movie with id : {id}", ContentType = "text/html" };
        //        return Content($"Get Movie with id : {id}", "text/html");
        //    }
        //    else if (id == 2) 
        //    { 
        //        //return new ContentResult() { Content = $"Get Movie with id : {id}", ContentType = "object/pdf" }; 
        //        return Content($"Get Movie with id : {id}", "object/pdf");

        //    }
        //    else if (id == 3)
        //    {
        //        //return new RedirectResult("https://www.google.com/");
        //        return Redirect("https://www.google.com/");
        //    }
        //    else
        //    {
        //        //return new RedirectToActionResult("AddMovie", "Movie", new { id = 133 }); 
        //        return RedirectToAction("AddMovie", "Movie", new { id = 133 });
        //    }
        //}
        //public string AddMovie(int id)
        //{
        //    return $"Get Movie with id : {id}";
        //}

        //Actions Parameters Binding
        //1.Form
        //2.Segment
        //3.Query Params
        //4.Files

        public IActionResult GetMovie(Movie movie)
        {
            return Content($"Get Movie with id : {movie.Id} Name: {movie.Name}", "text/html");
        }
    }
}
