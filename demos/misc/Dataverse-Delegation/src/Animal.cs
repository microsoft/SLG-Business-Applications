using System;

namespace AnimalFarm
{
    public class Animal
    {
        public string Name {get; set;}
        public string Species {get; set;}
        public bool Sex {get; set;}
        public int Weight {get; set;}

        public Animal()
        {
            Name = "";
            Species = "";
            Sex = false;
            Weight = 0;
        }

        public static Animal Random()
        {
            string[] FirstNames = new string[]{"Luna","Max","Bella","Milo","Daisy","Rocky","Chloe","Oliver","Nala","Toby","Zoey","Charlie","Luna","Jasper","Willow","Leo","Ruby","Cody","Sophie","Duke","Mocha","Rosie","Teddy","Maya","Winston","Mia","Gizmo","Sadie","Oscar","Pepper","Zeus","Lily","Rocky","Bella","Dexter","Gracie","Murphy","Zoey","Toby","Stella","Cooper","Ruby","Oliver","Lucy","Simba","Mia","Milo","Luna","Charlie","Chloe","Dexter","Zoe","Jack","Lulu","Max","Olive","Rocky","Daisy","Leo","Cleo","Jasper","Penny","Loki","Sophie","Marley","Gus","Ruby","Finn","Coco","Zeus","Willow","Dexter","Lily","Max","Luna","Bailey","Leo","Ivy","Winston","Zoey","Oliver","Mia","Rocky","Hazel","Charlie","Nala","Teddy","Chloe","Max","Luna","Murphy","Mochi","Sadie","Jasper","Lucy","Milo","Bella","Dexter","Lily","Toby"};
            string[] LastNames = new string[]{"Smith","Johnson","Williams","Jones","Brown","Davis","Miller","Wilson","Moore","Taylor","Anderson","Thomas","Jackson","White","Harris","Martin","Thompson","Garcia","Martinez","Robinson","Clark","Rodriguez","Lewis","Lee","Walker","Hall","Allen","Young","Hernandez","King","Wright","Lopez","Hill","Scott","Green","Adams","Baker","Gonzalez","Nelson","Carter","Mitchell","Perez","Roberts","Turner","Phillips","Campbell","Parker","Evans","Edwards","Collins","Stewart","Sanchez","Morris","Rogers","Reed","Cook","Morgan","Bell","Murphy","Bailey","Rivera","Cooper","Richardson","Cox","Howard","Ward","Torres","Peterson","Gray","Ramirez","James","Watson","Brooks","Kelly","Sanders","Price","Bennett","Wood","Barnes","Ross","Henderson","Coleman","Jenkins","Perry","Powell","Long","Patterson","Hughes","Flores","Washington","Butler","Simmons","Foster","Gonzales","Bryant","Alexander"};
            string[] Species = new string[]{"Lion","Tiger","Elephant","Giraffe","Zebra","Kangaroo","Penguin","Cheetah","Gorilla","Hippopotamus","Koala","Panda","Kangaroo","Pangolin","Armadillo","Leopard","Coyote","Hedgehog","Chimpanzee","Jaguar","Ostrich","Raccoon","Lynx","Kookaburra","Albatross","Ocelot","Sloth","Dolphin","Platypus","Koala","Orangutan","Cheetah","Puma","Lemur","Eagle","Penguin","Gazelle","Polar Bear","Kangaroo","Meerkat","Hawk","Toucan","Bison","Octopus","Squirrel","Panda","Frog","Cobra","Giraffe","Raccoon","Chameleon","Lionfish","Armadillo","Dingo","Koala","Lynx","Ocelot","Tiger","Penguin","Jaguar","Dolphin","Leopard","Gorilla","Kangaroo","Elephant","Platypus","Hippopotamus","Zebra","Lemur","Cheetah","Albatross","Panda","Ostrich","Coyote","Kookaburra","Lion","Kangaroo","Tiger","Giraffe","Elephant","Leopard","Cheetah","Koala","Panda","Hippopotamus","Gorilla","Zebra","Penguin","Lynx","Armadillo","Ocelot","Sloth","Kangaroo","Toucan","Meerkat","Bison"};

            Animal ToReturn = new Animal();
            ToReturn.Name = RandomFromList(FirstNames) + " " + RandomFromList(LastNames);
            ToReturn.Species = RandomFromList(Species);

            //Set sex
            Random r = new Random();
            if (r.Next(0, 2) == 0)
            {
                ToReturn.Sex = false;
            }
            else
            {
                ToReturn.Sex = true;
            }

            //Set weight
            ToReturn.Weight = r.Next(5, 500);

            return ToReturn;
        }

        private static string RandomFromList(string[] list)
        {
            Random r = new Random();
            return list[r.Next(0, list.Length)];
        }
    }
}