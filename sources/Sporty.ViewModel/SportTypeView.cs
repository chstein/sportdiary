using System;

namespace Sporty.ViewModel
{
    public class SportTypeView
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Disciplines Discipline { get; set; }
    }
}