
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
                                 "organization Trinity and racing to stop a Mayan apocalypse she has unleashed. Lara must traverse the environment and combat enemies with firearms and stealth as she explores semi-open hubs. In these hubs she" +
                                 " can raid challenge tombs to unlock new rewards, complete side missions, and scavenge for resources which can be used to craft useful materials",
                  Publisher = "Square Enix",
                  ImageUrl = "https://images.hdqwalls.com/wallpapers/god-of-war-uhd-4k-2g.jpg",
                  Genre = "Action-adventure",
                  ReleaseDate = new DateTime(2018, 09, 14)
              },
              new Item
              {

              }
             ); 
        }
    }
}
