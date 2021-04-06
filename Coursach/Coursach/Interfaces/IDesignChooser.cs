using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Coursach.Interfaces
{
    public interface IDesignChooser
    {
        public string Standart
        { get; }
        public string Current { get; set; }
        public string TakeTheme
        {
            get;
        }
        public SelectList Themes { get; } 
    }
}

