using System;

namespace AnimalID
{
    public class AnimalID
    {
        public string species { get; set; }
        public string age { get; set; }
        public string sex { get; set; }

        public AnimalID()
        {
            species = "";
            age = ""; //i.e. juvenille or adult
            sex = "";
        }
    }
}