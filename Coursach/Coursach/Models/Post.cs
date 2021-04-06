using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Coursach.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public uint Likes { get; set; }
    }
}
