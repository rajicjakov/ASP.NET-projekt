using MVC_projekt.Models;

namespace MVC_projekt.Services
{
    public class MockTabRepository : ITabRepository
    {
        private readonly List<Tab> _tabs;

        public MockTabRepository()
        {
            // Create mock users
            var user1 = new User { Id = 1, Username = "Đuro", DateJoined = DateTime.Now };
            var user2 = new User { Id = 2, Username = "Pero", DateJoined = DateTime.Now };
            var user3 = new User { Id = 3, Username = "Marko", DateJoined = DateTime.Now };

            // Create mock tabs
            var tab1 = new Tab
            {
                Id = 1,
                Title = "Neka pjesma",
                Artist = "Neki bend",
                CreatorId = 1,
                Creator = user1,
                DateCreated = DateTime.Now,
                StringTuning = "E-A-D-G-H-E",
                Difficulty = Difficulty.Intermediate,
                BPM = 120,
                Measures = new List<TabMeasure>
                {
                    new TabMeasure
                    {
                        CapitalNumber = 1,
                        TimeSignature = (4,4),
                        Columns = new List<TabColumn>
                        {
                            new TabColumn
                            {
                                CapitalNumber = 1,
                                ColumnDuration = new Duration{Base=4, IsDotted=false},
                                Notes = new List<TabNote>
                                {
                                    new TabNote { String = 3, Fret = 0 },
                                    new TabNote { String = 4, Fret = 0 }
                                }
                            },
                            new TabColumn
                            {
                                CapitalNumber = 2,
                                ColumnDuration = new Duration{Base=4, IsDotted=false},
                                Notes = new List<TabNote>
                                {
                                    new TabNote { String = 3, Fret = 3 },
                                    new TabNote { String = 4, Fret = 3 }
                                }
                            },
                            new TabColumn
                            {
                                CapitalNumber = 3,
                                ColumnDuration = new Duration{Base=4, IsDotted=false},
                                Notes = new List<TabNote>
                                {
                                    new TabNote { String = 3, Fret = 5 },
                                    new TabNote { String = 4, Fret = 5 }
                                }
                            }
                        }
                    }
                }
            };

            var tab2 = new Tab
            {
                Id = 2,
                Title = "Song 2",
                Artist = "Neki bend",
                CreatorId = 1,
                Creator = user1,
                DateCreated = DateTime.Now,
                StringTuning = "E-A-D-G-H-E",
                Difficulty = Difficulty.Easy,
                BPM = 60
            };

            var tab3 = new Tab
            {
                Id = 3,
                Title = "Song 3",
                Artist = "Artist 2",
                CreatorId = 2,
                Creator = user2,
                StringTuning = "D-A-D-G-H-E",
                DateCreated = DateTime.Now,
                Difficulty = Difficulty.Intermediate,
                BPM = 120
            };

            var tab4 = new Tab
            {
                Id = 4,
                Title = "Rock Anthem",
                Artist = "Heavy Metal Band",
                CreatorId = 3,
                Creator = user3,
                DateCreated = DateTime.Now.AddDays(-5),
                StringTuning = "E-A-D-G-B-E",
                Difficulty = Difficulty.Hard,
                BPM = 140
            };

            var tab5 = new Tab
            {
                Id = 5,
                Title = "Blues Shuffle",
                Artist = "Blues Master",
                CreatorId = 1,
                Creator = user1,
                DateCreated = DateTime.Now.AddDays(-2),
                StringTuning = "E-A-D-G-B-E",
                Difficulty = Difficulty.VeryEasy,
                BPM = 90
            };

            _tabs = new List<Tab> { tab1, tab2, tab3, tab4, tab5 };
        }

        public List<Tab> GetAllTabs()
        {
            return _tabs;
        }

        public Tab GetTabById(int id)
        {
            return _tabs.FirstOrDefault(t => t.Id == id);
        }
    }
}