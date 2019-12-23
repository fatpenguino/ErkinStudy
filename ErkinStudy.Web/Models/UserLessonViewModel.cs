using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ErkinStudy.Web.Models
{
    public class UserLessonViewModel
    {
        public long UserId { get; set; }
        public List<SelectListItem> SelectListItems { get; set; }

    }
}
