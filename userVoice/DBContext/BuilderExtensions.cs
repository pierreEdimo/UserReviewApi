
using Microsoft.EntityFrameworkCore;
using userVoice.Model;
using System; 

namespace userVoice.DBContext
{
    public static class BuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1 , Name="Video Games", ImageUrl= "https://images.unsplash.com/photo-1606160429008-751d8408a874?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1562&q=80" }, 
                new Category { Id = 2, Name="Tv Show/Movies", ImageUrl= "https://images.unsplash.com/photo-1485846234645-a62644f84728?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1340&q=80" }, 
                new Category { Id = 3, Name= "Comics", ImageUrl= "https://images.unsplash.com/photo-1588497859490-85d1c17db96d?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1350&q=80" }, 
                new Category { Id = 4, Name="Electronics", ImageUrl= "https://images.unsplash.com/photo-1519389950473-47ba0277781c?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1350&q=80" }
           );


            modelBuilder.Entity<Item>().HasData(
              new Item { Id =1 , 
                         CategoryId = 1 ,
                         Name="Red Dead Redemption 2",  
                         Description= "**Red Dead Redemption 2** is a 2018 action-adventure game developed and published by Rockstar Games. " +
                                      "The game is the third entry in the Red Dead series and is a prequel to the 2010 game Red Dead Redemption. The story is set in 1899 in a fictionalized representation of the Western, " +
                                      "Midwestern, and Southern United States and follows outlaw Arthur Morgan, a member of the Van der Linde gang. Arthur must deal with the decline of the Wild West whilst " +
                                      "attempting to survive against government forces, rival gangs, and other adversaries. The story also follows fellow gang member John Marston, the protagonist of Red Dead Redemption.",
                        Publisher="Rockstar Games",
                        ImageUrl= "https://hdqwalls.com/download/red-dead-redemption-2-62-1125x2436.jpg", 
                        Genre="Action-adventure game",
                        ReleaseDate= new DateTime(2018, 10, 26)
              }, 
              new Item
              {
                  Id = 2,
                  CategoryId = 1,
                  Name = "God pf war",
                  Description = "**God of war** is an action-adventure game developed by Santa Monica Studio and published by Sony Interactive Entertainment (SIE). Released worldwide on April 20, 2018, for the PlayStation 4 (PS4)," +
                                " it is the eighth installment in the God of War series, the eighth chronologically, and the sequel to 2010's God of War III. Unlike previous games, which were loosely based on Greek mythology, this installment is" +
                                " rooted in Norse mythology, with the majority of it set in ancient Scandinavia in the realm of Midgard. For the first time in the series, there are two protagonists: Kratos, the former Greek God of War who remains the" +
                                " only playable character, and his young son Atreus. Following the death of Kratos' second wife and Atreus' mother, they journey to fulfill her request that her ashes be spread at the highest peak of the nine realms. " +
                                "Kratos keeps his troubled past a secret from Atreus, who is unaware of his divine nature. Along their journey, they encounter monsters and gods of the Norse world " +
                                 "\n" +
                                 "\n" +
                                 "\n" +
                                 "Described by creative director Cory Barlog as a reimagining of the franchise, a major gameplay change is that Kratos makes prominent use of a magical battle axe instead of his signature double-chained blades. " +
                                 "God of War also uses an over-the-shoulder free camera, with the game in one shot, as opposed to the fixed cinematic camera of the previous entries. It was the first 3D AAA game to use a one-shot camera. The game" +
                                 " also includes role-playing video game elements, and Kratos' son Atreus provides assistance in combat. The majority of the original game's development team worked on God of War and designed it to be accessible and " +
                                 "grounded. A separate short text-based game, A Call from the Wilds, was released in February 2018 and follows Atreus on his first adventure",
                  Publisher = "	Sony Interactive Entertainment",
                  ImageUrl = "https://images.hdqwalls.com/wallpapers/god-of-war-uhd-4k-2g.jpg",
                  Genre = "Action-adventure",
                  ReleaseDate = new DateTime(2018, 04, 20)
              },
              new Item
              {
                  Id = 3,
                  CategoryId = 1,
                  Name = "shadow of the Tomb Raider",
                  Description = "**Shadow of the Tomb Raider** is a 2018 action-adventure video game developed by Eidos-Montréal and published by Square Enix. It continues the narrative from the 2015 game Rise of the" +
                                " Tomb Raider and is the twelfth mainline entry in the Tomb Raider series. The game was originally released worldwide for Microsoft Windows, PlayStation 4 and Xbox One. Versions for macOS and Linux, and" +
                                " Stadia, were released in November 2019. After release, the game was expanded upon with downloadable content in both a season pass and as standalone releases " +
                                 "\n" +
                                 "\n" +
                                 "\n" +
                                 "Set shortly after the events of Rise of the Tomb Raider, its story follows Lara Croft as she ventures through the tropical regions of the Americas to the legendary city Paititi, battling the paramilitary " +
                                 "organization Trinity and racing to stop a Mayan apocalypse she has unleashed. Lara must traverse the environment and combat enemies with firearms and stealth as she explores semi-open hubs. In these hubs she can raid challenge tombs to unlock new rewards, complete side missions, and scavenge for resources which can be used to craft useful materials",
                  Publisher = "Square enix",
                  ImageUrl = "https://images.hdqwalls.com/wallpapers/2019-shadow-of-the-tomb-raider-lara-croft-4k-b9.jpg",
                  Genre = "Action-adventure game",
                  ReleaseDate = new DateTime(2018, 09, 14)
              }
             ); 
        }
    }
}
