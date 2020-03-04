using MDT2PxWeb.Bean;
using MDT2PxWeb.Connector;
using MDT2PxWeb.PxWeb;
using System;
using System.Collections.Generic;

namespace MDT2PxWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            string inFile, outFile;
            bool print = false, update = false;

#if DEBUG
            inFile = @"\SDGs\MDT2PxWeb\config.json";
            print = true;
            update = false;
            outFile = (print || update) ? @"\SDGs\MDT2PxWeb\output.sql" : null;
#else
            if (args.Length == 0)
            {
                Console.Out.WriteLine();
                Console.Out.WriteLine("USAGE");
                Console.Out.WriteLine();
                Console.Out.WriteLine("MDT2PxWeb conf.json [print [output.sql]] - create SDGs structure into the PxWeb database");
                Console.Out.WriteLine("MDT2PxWeb conf.json update [output.sql]  - update SDGs data into the PxWeb database");
                Console.Out.WriteLine();
                return;
            }

            inFile = args[0];
            print = args.Length > 1 && "print".Equals(args[1].ToLower());
            update = args.Length > 1 && "update".Equals(args[1].ToLower());
            outFile = ( (print || update) && args.Length > 2) ? args[2] : null;
#endif

            Config c = Config.ReadConfig(inFile);
            if (update)
            {
                List<Query> queries = outFile == null ? null : new List<Query>();
                Console.Out.Write(outFile == null ? "Transferring records..." : "Writing queries...");
                int counter = PxWebDataTransfer.Transfer(c, queries);
                if (outFile == null)
                {
                    Console.Out.WriteLine("done");
                    Console.Out.Write("Transferred " + counter + " records");
                }
                else
                {
                    List<string> sqls = new List<string>();
                    foreach (Query query in queries)
                    {
                        sqls.Add(query.ToSql(c.pxwebDb));
                    }
                    System.IO.File.WriteAllLines(outFile, sqls);
                    Console.Out.WriteLine("done");
                }
            }
            else
            {
                List<Goal> goals = c.GetGoals();
                foreach (Goal goal in goals)
                {
                    int indicators = 0, subIndicators = 0;
                    foreach (Target target in goal.targets)
                    {
                        indicators += target.indicators.Count;
                        foreach (Indicator indicator in target.indicators)
                        {
                            subIndicators += indicator.subIndicators.Count;
                        }
                    }
                    Console.Out.Write("Goal " + LeftPadding(goal.code, 2));
                    Console.Out.Write(" #Targets " + LeftPadding(goal.targets.Count, 3));
                    Console.Out.Write(" #Indicators " + LeftPadding(indicators, 3));
                    Console.Out.Write(" #SubIndicators " + LeftPadding(subIndicators, 3));
                    Console.Out.WriteLine();
                }

                List<Query> queriesDeletes = PxWebMetadata.CreateDeletes(c);
                List<Query> queriesMenu = PxWebMetadata.CreateMenu(c);
                List<Query> queriesVariables = PxWebMetadata.CreateVariables(c);
                List<Query> queriesCubes = PxWebMetadata.CreateCubes(c);
                if (print)
                {
                    List<Query> queries = new List<Query>();
                    queries.AddRange(queriesDeletes);
                    queries.AddRange(queriesMenu);
                    queries.AddRange(queriesVariables);
                    queries.AddRange(queriesCubes);
                    if (outFile != null)
                    {
                        List<string> sqls = new List<string>();
                        foreach (Query query in queries)
                        {
                            sqls.Add(query.ToSql(c.pxwebDb));
                        }
                        System.IO.File.WriteAllLines(outFile, sqls);
                    }
                    else if (print)
                    {
                        foreach (Query query in queries)
                        {
                            Console.Out.WriteLine(query.ToSql(c.pxwebDb));
                        }
                    }
                }
                else
                {
                    DBConnection connection = null;
                    try
                    {
                        connection = new DBConnection(c.pxwebDb);
                        ExecuteQueries(connection, queriesDeletes, "Cleaning db");
                        ExecuteQueries(connection, queriesMenu, "Creating menu");
                        ExecuteQueries(connection, queriesVariables, "Init variables");
                        ExecuteQueries(connection, queriesCubes, "Creating cubes");
                    }
                    finally
                    {
                        if (connection != null)
                        {
                            connection.CloseConnection();
                        }
                    }
                }
            }
#if DEBUG
            Console.Out.WriteLine("Press a key to exit");
            Console.ReadKey();
#endif
        }

        private static void ExecuteQueries(DBConnection connection, List<Query> queries, string text)
        {
            int records = 0;
            Console.Out.Write(text + "... ");
            foreach (Query query in queries)
            {
                records += query.ExecuteQuery(connection);
            }
            Console.Out.WriteLine("done (" + queries.Count + " queries/" + records + " records affected)");
        }

        private static string LeftPadding(int i, int length)
        {
            return LeftPadding("" + i, length);
        }

        private static string LeftPadding(string s, int length)
        {
            while (s.Length < length)
            {
                s = " " + s;
            }
            return s;
        }
    }

}
