using MDT2PxWeb.Connector;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;

namespace MDT2PxWeb.Bean
{
    class Config
    {
        private List<Goal> goals = null;

        public string organizationCode;
        public string organizationName;
        public string userId;
        public string obsValue;
        public Database mdtDb;
        public Database pxwebDb;
        public Language mainLanguage;
        public Language secondaryLanguage;
        public List<Dimension> dimensions;

        private Config()
        {

        }

        internal bool HasSecondaryLanguage()
        {
            return secondaryLanguage != null;
        }

        internal List<Goal> GetGoals()
        {
            if (goals != null)
            {
                return goals;
            }
            goals = new List<Goal>();
            DBConnection connection = null;
            try
            {
                connection = new DBConnection(mdtDb);

                if ((dimensions == null || dimensions.Count == 0) && mdtDb.dimensionsTable != null)
                {
                    dimensions = new List<Dimension>();
                    DbDataReader reader = connection.ExecuteReader("SELECT NAME, LABEL, LABEL_ENG, TABLE_NAME, CODE, DESCRIPTION, DESCRIPTION_ENG, IS_TIME, INT_CODE FROM DIMENSION ORDER BY SEQUENCE");
                    while (reader.Read())
                    {
                        Dimension d = new Dimension
                        {
                            name = reader.GetString(0).Trim(),
                            label = reader.GetString(1).Trim(),
                            labelEn = reader.GetString(2).Trim(),
                            isTime = reader.GetByte(7) == 1
                        };

                        if (!reader.IsDBNull(3))
                        {
                            Table t = new Table
                            {
                                name = reader.GetString(3).Trim(),
                                code = reader.IsDBNull(4) ? null : reader.GetString(4).Trim(),
                                desc = reader.IsDBNull(5) ? null : reader.GetString(5).Trim(),
                                descEn = reader.IsDBNull(6) ? null : reader.GetString(6).Trim(),
                                integerCode = reader.GetByte(8) == 1
                            };
                            d.table = t;
                        }

                        dimensions.Add(d);
                    }
                    reader.Close();
                }

                foreach (Dimension dimension in dimensions)
                {
                    if (dimension.table == null)
                    {
                        continue;
                    }
                    DataSet dimensionReader = connection.ExecuteDataSet(dimension.table.GetSqlSelect());
                    foreach (DataRow dr in dimensionReader.Tables[0].Rows)
                    {
                        string key = dr[0].ToString().Trim();
                        string value = dr[1].ToString().Trim();
                        dimension.values.Add(key, value);
                        if (!dr.IsNull(2))
                        {
                            string valueEn = dr[2].ToString().Trim();
                            dimension.valuesEn.Add(key, valueEn);
                            if (string.IsNullOrWhiteSpace(value))
                            {
                                dimension.values[key] = valueEn;
                            }
                        }
                        else
                        {
                            dimension.valuesEn.Add(key, value);
                        }
                    }
                }

                DataSet goalReader = connection.ExecuteDataSet(mdtDb.goalTable.GetSqlSelect());
                foreach (DataRow dr in goalReader.Tables[0].Rows)
                {
                    Goal goal = new Goal
                    {
                        code = dr[0].ToString().Trim(),
                        desc = dr[1].ToString().Trim(),
                        descEn = dr[2].ToString().Trim()
                    };
                    DataSet targetReader = connection.ExecuteDataSet(mdtDb.targetTable.GetSqlSelect(), new Dictionary<string, object> { { "@REFERENCE", goal.code } });
                    foreach (DataRow dr1 in targetReader.Tables[0].Rows)
                    {
                        Target target = new Target
                        {
                            code = dr1[0].ToString().Trim(),
                            desc = dr1[1].ToString().Trim(),
                            descEn = dr1[2].ToString().Trim()
                        };
                        DataSet indicatorReader = connection.ExecuteDataSet(mdtDb.indicatorTable.GetSqlSelect(), new Dictionary<string, object> { { "@REFERENCE", target.code } });
                        foreach (DataRow dr2 in indicatorReader.Tables[0].Rows)
                        {
                            Indicator indicator = new Indicator
                            {
                                code = dr2[0].ToString().Trim(),
                                desc = dr2[1].ToString().Trim(),
                                descEn = dr2[2].ToString().Trim()
                            };
                            DataSet subIndicatorReader = connection.ExecuteDataSet(mdtDb.subIndicatorTable.GetSqlSelect(), new Dictionary<string, object> { { "@REFERENCE", indicator.code } });
                            foreach (DataRow dr3 in subIndicatorReader.Tables[0].Rows)
                            {
                                SubIndicator subIndicator = new SubIndicator
                                {
                                    code = dr3[0].ToString().Trim(),
                                    desc = dr3[1].ToString().Trim().Length == 0 ? dr3[0].ToString().Trim() : dr3[1].ToString().Trim(),
                                    descEn = dr3[2].ToString().Trim().Length == 0 ? dr3[0].ToString().Trim() : dr3[2].ToString().Trim()
                                };
                                string dims = dr3[3].ToString().Trim();
                                for (int i = 0; i < dims.Length && i < dimensions.Count; i++)
                                {
                                    if (dims[i] == '1')
                                    {
                                        subIndicator.dimensions.Add(dimensions[i]);
                                    }
                                }

                                indicator.subIndicators.Add(subIndicator);
                            }
                            target.indicators.Add(indicator);
                        }
                        goal.targets.Add(target);
                    }
                    goals.Add(goal);
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.CloseConnection();
                }
            }
            return goals;
        }

        public static Config ReadConfig(string jsonFile)
        {
            string json = File.ReadAllText(jsonFile, Encoding.UTF8);
            Config config = JsonConvert.DeserializeObject<Config>(json);
            return config;
        }

    }

}
