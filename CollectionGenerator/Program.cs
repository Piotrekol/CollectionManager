using System;
using System.Collections.Generic;
using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO;
using CollectionManagerExtensionsDll.Modules.API;
using CollectionManagerExtensionsDll.Modules.API.osu;


namespace CollectionGenerator
{
    class Program
    {
        private static OsuApi _api;
        private static readonly OsuFileIo OsuFileIo = new OsuFileIo(new BeatmapExtension());
        static void Main(string[] args)
        {
            Console.WriteLine("Collection Generator");
            Console.WriteLine("====================");
            Console.WriteLine("WARNING: ALL paths are hardcoded, meaning if you don't change them and recompile this program IT WILL NOT WORK."); //technically it could but eh, better safe than sorry.
            Console.WriteLine("If you did recompile it with your correct paths then you may continue... (Press enter)");
            Console.WriteLine("====================");

            Console.ReadLine();
            Console.WriteLine("Enter your osu!api key: ");

            string apiKey = Console.ReadLine();
            _api = new OsuApi(apiKey);

            //grab all maps from 2007 
            Console.WriteLine("Getting beatmaps. This can take around 5 minutes.");
            var maps = GetBeatmapsFromYear(2007, false);

            Console.WriteLine("Generating collections");
            OsuFileIo.CollectionLoader.SaveOsdbCollection(GenByYear(maps), @"D:\Kod\kolekcje\ByYear.osdb");
            OsuFileIo.CollectionLoader.SaveOsdbCollection(GenByGenreV2(maps), @"D:\Kod\kolekcje\ByGenre.osdb");

        }
        private static readonly Dictionary<int, string> mapGenres = new Dictionary<int, string>()
        {
            {0,"Any" },
            {1,"Unspecified" },
            {2,"Video Game" },
            {3,"Anime" },
            {4,"Rock" },
            {5,"Pop" },
            {6,"Other" },
            {7,"Novelity" },
            {8,"??" },
            {9,"Hip Hop" },
            {10,"Electronic" }
        };
        private static readonly Dictionary<int, string> languages = new Dictionary<int, string>()
        {
            {0,"Any" },
            {1,"Other" },
            {2,"English" },
            {3,"Japanese" },
            {4,"Chinese" },
            {5,"Instrumental" },
            {6,"Korean" },
            {7,"French" },
            {8,"German" },
            {9,"Swedish" },
            {10,"Spanish" },
            {11,"Italian" }
        };
        static Collections GenByGenre(Beatmaps maps)
        {
            var year = 2007;
            Collections collections = new Collections();

            //Prepare a bunch of collections for genre_id matching
            for (int i = 0; i < 20; i++)
            {
                collections.Add(new Collection(OsuFileIo.LoadedMaps) { Name = i.ToString() });
            }
            //place maps in collections according to genre_id
            foreach (BeatmapExtensionEx map in maps)
            {
                collections[map.GenreId].AddBeatmap(map);
            }

            RemoveEmptyCollections(collections);

            return collections;
        }
        static Collections GenByGenreV2(Beatmaps maps)
        {
            var year = 2007;
            //Collections collections = new Collections();
            Dictionary<int, Dictionary<int, Collection>> sortedMaps = new Dictionary<int, Dictionary<int, Collection>>();

            //Prepare a bunch of collections for genre and language matching
            foreach (var mapGenre in mapGenres)
            {
                sortedMaps.Add(mapGenre.Key, new Dictionary<int, Collection>());
                foreach (var language in languages)
                {
                    sortedMaps[mapGenre.Key].Add(language.Key,
                        new Collection(OsuFileIo.LoadedMaps) { Name = mapGenre.Value + " (" + language.Value + ")" });
                }
            }
            //categorize maps
            foreach (BeatmapExtensionEx map in maps)
            {
                sortedMaps[map.GenreId][map.LanguageId].AddBeatmap(map);
            }
            Collections collections = new Collections();

            //Grab all non-empty collections
            foreach (var dictOfCollections in sortedMaps)
            {
                foreach (var collection in dictOfCollections.Value)
                {
                    if (collection.Value.NumberOfBeatmaps != 0)
                        collections.Add(collection.Value);
                }
            }

            return collections;
        }
        static Collections GenByYear(Beatmaps maps)
        {
            var year = 2007;
            Collections collections = new Collections();
            Dictionary<int, Collection> CollYears = new Dictionary<int, Collection>();


            for (int i = year; i < 2050; i++)
            {
                var collection = new Collection(OsuFileIo.LoadedMaps) { Name = i.ToString() };
                collections.Add(collection);
                CollYears.Add(i, collection);
            }
            foreach (BeatmapExtensionEx map in maps)
            {
                CollYears[map.ApprovedDate.Year].AddBeatmap(map);
            }

            RemoveEmptyCollections(collections);

            return collections;
        }

        static void RemoveEmptyCollections(Collections collections)
        {
            for (int i = collections.Count - 1; i > 0; i--)
            {
                if (collections[i].NumberOfBeatmaps == 0)
                {
                    collections.Remove(collections[i]);
                }
            }

        }

        static Beatmaps GetBeatmapsFromYear(int year, bool grabJustOneYear = true)
        {
            DateTime startDate = new DateTime(year, 01, 01), endDate = new DateTime(year + (grabJustOneYear ? 1 : 50), 01, 01);
            return _api.GetBeatmaps(startDate, endDate);
        }

    }
}
