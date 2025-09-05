using System;

namespace AnimalID
{
    public class AnimalID
    {
        public string species { get; set; }
        public int age { get; set; }
        public string sex { get; set; }

        public AnimalID()
        {
            species = "";
            age = 0;
            sex = "";
        }
    }
}