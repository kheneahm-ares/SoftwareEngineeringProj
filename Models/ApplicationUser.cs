﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CodingBlogDemo2.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {

//        public virtual ICollection<Course> Courses { get; set; }

        public bool IsAdmin { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
