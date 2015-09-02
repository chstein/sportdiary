using System;
using System.ComponentModel.DataAnnotations;

namespace Sporty.ViewModel
{
    public class GoalView
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please insert a date.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please insert a name.")]
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsNew { get; set; }
    }
}