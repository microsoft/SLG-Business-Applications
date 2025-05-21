using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataversePerformanceTesting
{
    public class Animal
    {
        public string Name {get; set;}
        public AnimalSpecies Species {get; set;}
        public DateTime DateOfBirth {get; set;}
        public int WeightPounds {get; set;}
        public float DailyFeedIntakePounds {get; set;}

        public Animal()
        {
            Name = "";
        }

        public JObject ForDataverseUpload()
        {
            JObject ToReturn = new JObject();
            ToReturn.Add("timh_name", Name);
            ToReturn.Add("timh_species", (int)Species);
            ToReturn.Add("timh_dateofbirth", DateOfBirth.ToString());
            ToReturn.Add("timh_weightpounds", WeightPounds);
            ToReturn.Add("timh_dailyfeedintakepounds", DailyFeedIntakePounds);
            return ToReturn;
        }

        public static Animal Random()
        {
            Animal ToReturn = new Animal();

            //Random stuff
            string[] FirstNames = new string[]{"Biscuit","Wiggles","Muffin","Snickers","Pickles","Nibbles","Cupcake","Sprout","Bubbles","Peanut","Whiskers","Pumpkin","Fluffy","Puddles","Blinky","Doodles","Cheeks","Snuggle","Marshmallow","Chompers","Buttons","Moose","Giggles","Churro","Tater","Daisy","Waffles","Pip","Boop","Mochi","Puffin","Scooter","Chomp","Doodlebug","Snickers","Chilly","Mittens","Tootsie","Scooch","Whisk","Frito","Clover","Fuzzball","Rascal","Pancake","Munchkin","Popcorn","Booger","Flopsy","Squiggles","Noodle","Scamp","Goober","Twix","Pebbles","Frito","Muffins","Chunky","Spud","Snickers","Buttercup","Fizz","Bean","Snickerdoodle","Zippy","Bingo","Binky","Blossom","Ruffles","Cricket","Sniffy","Skittles","Ziggy","Pudding","Squash","Zuzu","Crumbs","Taco","Cheddar","Smudge","Pipsqueak","Oreo","Sugar","Twinkle","Honeybun","Whiffy","Puff","Fizzgig","Toffee","Mallow","Snookums","Scoot","Pumpkin","Nutmeg","Mumble","Tickles","Sparkles","Spunky","Maple","Scruffy","Gizmo","Grumbles","Toots","Sprinkle","Dot","Sparky","Fluff","Sizzle","Snappy","Beep","Blinky","Roo","Fluffernutter","Luna","Pookie","Socks","Furball","Smudge","Yoyo","Zipper","Hiccups","Bonkers","Pinky","Snooze","Skippy","Crumb","Nibs","Cheerio","Kibbles","Bugsy","Snugglebug","Tiddles","Cotton","Beanie","Fizzles","Noodle","Wiggle","Gumdrop","Puffball","Zaza","Dazzle","Scribbles","Hobnob","Scrambles","Perky","Pixie","Peaches","Skedaddle","Spritz","Muggle","Flick","Marbles","Flake","Stitch","Bumble","Twizzle","Snips","Squeaks","Rumbles","Sushi","Truffles","Twiddle","Mittens","Patches","Curly","Jellybean","Fizzwhizz","Muddle","Gummy","BamBam","Spritz","Slinky","Wobbles","Nuzzle","Cupcake","Buttons","Zigzag","Dotty","Scurry","Zapp","Butter","Snuggie","Rolo","Twitch","Pippin","Chill","Bumble","Yapper","Giggles","Pop","Fizzers","Hopper","Dash","Frodo","Smores","Nippy","Zuzu","Toot","Nuts","Corky","Fido","Shimmer","Wags","Gizmo","Snap","Doodle","Blossom","Pocky","Doofus","Daisy","Snuggle","Jumbles","Snip","Lolly","Goof","Raspberry","Boopsy","Milo","Flea","Crabby","Biscuit","Nippy","Scoop","Frizzle","Wiggly","Zazzle","Boop","Rolo","Momo","Piff","Chirpy","Dobby","Zizzle","Snickers","Flip","Gumdrop","Jabber","Hop","Glitter","Nana","Whippy","Kix","Jiggles","Tipsy","Nifty","Flick","Zibby","Bubbles","Dobby","Lolly","Cupcake","Scruffles","Doodle","Dazzle","Fumble","Chatter","Zazzy","Pickle","Muddles","Scraps","Sizzle","Fritter","Quirky","Buffy","Coco","Juju","Gidget","Muggles","Zowie","Spots","Dippy","Taz","Winkle","Riff","Chip","Froggie","Kiki","Zigzag","Hopscotch","Dizzy","Skitter","Tickles","Fluffy","Sprout","Pocky","Sunny","Fizzy","Dewy","Chipper","Fizz","Froggy","Tricksy","Doodles","Tango","Taco","Diddle","Smooch","Snuzzle","Tippy","Giggles","Peewee","Doodles","Twister","Perky","Whizzy","Snickers","Snook","Scrappy","Squish","Cuddlebug","Scooter","Biff","Nubs","Sprinkle","Zoom","Scrunch","Flapjack","Whiskers","Sprocket","Piddle","Sparrow","Glimmer","Glitzy","Nibble","Smidge","Tinker","Glitter","Zimmy","Quibble"};
            string[] LastNames = new string[]{"McFluff","Wiggleton","Barkley","Sniffles","Whiskerton","McNibbles","Pawson","Munchworthy","Sprinkleton","Chubbins","Waggles","Fluffington","Snuggleby","Furballer","Puddlejumper","Snoozington","Twinklebottom","Snickerdoodle","Fuzzwhisk","Whiskerpaws","McSnuggles","Muffinpaw","Bumblefluff","Peabody","Wobbleton","Nibblesworth","Sugarwhisk","Snugglepaws","Puffenstuff","McSniffs","Wigglebottom","Binkybuns","Sniffleby","Furrington","Puddlenose","Gigglepaws","Rumblefluff","Tiddlepuff","Ziggleton","McNibble","Scooterfluff","Snootington","Bibblebuns","Whiskersmith","Nibblewhisk","Rumpelwhisk","Tickletail","Flufferpuff","Sparklebuns","Scootsworth","Floppywhisk","Snuggleton","Peepaws","Snufflebums","Chomperton","Booplesnoot","Nibblekins","Wobblewhisk","Snifferly","Tumblewhisk","Purrington","McFluffers","Gigglewhisk","Snoodleton","Flickerpaws","Twitcherly","Purrfoot","Hoppington","Muddlepaws","Nibblefur","Scamperton","Fluffernut","Wigglewhisk","Muddlefoot","Snicklepaw","Wigglesniff","Fluttertail","Cuddlewhisk","Snuzzlebum","Chirpleton","Munchpaw","Snugglewhisk","Snorkleby","Whiskerly","Gigglefur","Snoopington","Snoutley","Boopkins","Snoofle","Snugglenose","Mufflebuns","Tiddlywinks","Bumblefoot","Flittertail","Fluffersniff","McSnuzzle","Wagglebum","Chipperton","Wiggletail","Ticklewhisk","Nuzzlepaws","Rumpus","Snickertail","Snicklepurr","Zippywhisk","Sprinklewhisk","Snickerdip","Nibblebottom","Snoutsworth","Tumblesniff","Twiddlefur","Scampersmith","Gigglesniff","Twinklewhisk","Rascalbottom","Flapwhisk","Scruffywags","Muffletail","Snickerdink","Chirpwhisk","McChomp","Bumblewhisk","Whiskerfluff","Nibblepaws","Furrytail","Puddingtail","Flickpaw","Nibblewhisk","Chirple","Scrufflekins","Chortlewhisk","Twinkletoes","Snickerpaws","Pipwhisk","Flopsalot","Snufflesnoot","Flutterwhisk","Pibblepaws","Cuddlesnuff","Nibblykins","Snooflepuff","Hufflewhisk","Snickersnuff","Puddenpaw","Hopperwhisk","McBumble","Snootykins","Snoozytail","Nibblewhiskers","Snickerbop","Gigglewhiskers","Cuddletoes","Snicklewhisk","Fuzzykins","Wagglenose","Chirplewhisk","Scrufflefoot","Nibblebottom","Snickersnoot","Snicklesnoot","Pibblewhisk","Rumblesniff","Tiddlywhisk","Snooflepaws","Fuzzywhisk","Booplesworth","Snickerbottom","Twiddlepaws","Snugglesnoot","Fuzzytail","Bumbletail","Snicklebuns","Wagglewhisk","Snickerwhisk","Snickerwhisk","Snickerpuff","Flufferkins","McSnugglepaws","Wigglesnort","Flitterwhisk","Snicklesniff","Snickertail","Snickernose","Muzzlewhisk","Cuddlefluff","Wagglywhisk","Nibblebuns","Snifflebop","Snickerwhiskers","Rufflesniff","Snufflebop","Nibblewhiskers","Wigglewhisk","Bumblewhiskers","Cuddleflop","Puddlewhisk","Gigglesnuffle","Snicklebottom","Boopwhisk","Snickerpaws","Snicklesnuff","Twinklewhisk","Boopkins","Snickerbottom","Whiskerkins","Booplesnoot","Fuzzyfluff","Snicklesnoot","Nibblewhisk","Fluffywhisk","Chortlepaws","Snugglepaws","Nibblebottom","Purrpaws","Nibblebottom","Twiddlewhisk","Bumblemitt","Ticklenose","Snickerpaws","Muddlewhisk","Nibblewhisk","Ticklepaws","Fuzzlebottom","Fuzzywhisk","Nosewiggle","Snickerbuns","Hugglewhisk","Whiskerbottom","Chortlepaws","Wigglepaws","Nibblesworth","Snuzzlewhisk","Snugglebottom","Snicklebuns","Nibblewhisk","Pipwhisk","Snicklebop","Snickersnoot","Rumblesniff","Snugglefluff","Snicklebums","Buzzywhisk","Squeakums","Bumblefluff","Snugglewhisk","Snicklebop","Waggletail","Wagglewhisk","Snickerbottom","Flapjack","Nibblewhisk","Snickersnoot","Snugglybum","Twiddlypaws","Flickertail","Nibblewhisk","Hufflebuns","Snickerwhisk","Chortlewhisk","Furrywhisk","Rufflebop","Flapperkins","Nuzzlebuns","Pawster","Snicklesnuff","Snugglekins","Ticklewhisk","Munchykins","Fluffykins","Purrington","Squeakerton","Chubbington","Wigglesworth","Twiddlesnoot","Rumblewhisk","Gigglefluff","McSnuggle","Nibblebuns","Whiskerfoot","Wigglepaw","Nibblewhisk","Fluffster","Snickerdip","Nibbler","Flicksnout","Flittertail","Rascalton","Snugglefoot","Pudgkins","Fluffywhisk","Tiddlywhisk","Huggles","Pawsington","Twinklewhisk","Flufftail","Tickletail","Nibblefluff","Whiskernose","Snicklenose","Bumblywhisk","Snickerwhisk","Wigglewhisk","Pawlywhisk","Tumblewhisk","Rumbletail","Pipwhisk","Nosewiggle","Waggles","Snoozles"};
            string[] MiddleInitials = new string[]{"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
            string[] Suffixes = new string[]{"Jr.","Sr.","II","III","IV","V"};

            Random r = new Random();

            //Construct name
            string FirstName = SelectRandom(FirstNames) + " ";
            string LastName = SelectRandom(LastNames);
            string MiddleInitial = "";
            if (r.NextSingle() < 0.35)
            {
                MiddleInitial = SelectRandom(MiddleInitials) + ". ";
            }
            string Suffix = "";
            if (r.NextSingle() < 0.20)
            {
                Suffix = " " + SelectRandom(Suffixes);
            }
            ToReturn.Name = FirstName + MiddleInitial + LastName + Suffix;

            //Get random species
            Array SpeciesOptions = Enum.GetValues(typeof(AnimalSpecies));
            object? selectedSpecies = SpeciesOptions.GetValue(r.Next(0, SpeciesOptions.Length));
            if (selectedSpecies != null) //have to do this dumb null check so C# stops yelling at me with a warning.
            {
                ToReturn.Species = (AnimalSpecies)selectedSpecies;
            }

            //Random date of birth
            DateTime RangeBeginning = new DateTime(1985, 1, 1);
            DateTime RangeEnd = DateTime.UtcNow;
            int range = (RangeEnd - RangeBeginning).Days; //range, in days
            ToReturn.DateOfBirth = RangeBeginning.AddDays(r.Next(0, range));

            //Random weight
            ToReturn.WeightPounds = r.Next(10, 550);

            //Random feed
            ToReturn.DailyFeedIntakePounds = r.NextSingle() * 300;
            
            return ToReturn;
        }

        private static string SelectRandom(string[] options)
        {
            return options[new Random().Next(0, options.Length)];
        }
    }
}