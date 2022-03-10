using MDT2PxWeb.Bean;
using MDT2PxWeb.Connector;
using System.Collections.Generic;
using System.Data.Common;

namespace MDT2PxWeb
{
    class PxWebMetadata
    {

        private static readonly List<string> inserted = new List<string>();

        public static List<Query> CreateDeletes(Config c)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@userId", c.userId } };
            List<Query> tableSL;
            if (c.HasSecondaryLanguage())
            {
                tableSL = new List<Query>
                {
                    new Query("DELETE FROM \"MENUSELECTION_" + c.secondaryLanguage.suffixDB + "\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"CONTENTS_" + c.secondaryLanguage.suffixDB + "\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"VSVALUE_" + c.secondaryLanguage.suffixDB + "\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"VALUE_" + c.secondaryLanguage.suffixDB + "\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"SUBTABLE_" + c.secondaryLanguage.suffixDB + "\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"VALUESET_" + c.secondaryLanguage.suffixDB + "\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"VALUEPOOL_" + c.secondaryLanguage.suffixDB + "\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"VARIABLE_" + c.secondaryLanguage.suffixDB + "\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"MAINTABLE_" + c.secondaryLanguage.suffixDB + "\" WHERE USERID = @userId", parameters)
                };
            }
            else
            {
                tableSL = new List<Query>();
            }

            List<Query> tableMainL = new List<Query>
                {
                    new Query("DELETE FROM \"SECONDARYLANGUAGE\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"MENUSELECTION\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"CONTENTSTIME\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"CONTENTS\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"VSVALUE\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"VALUE\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"SUBTABLEVARIABLE\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"SUBTABLE\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"VALUESET\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"VALUEPOOL\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"VARIABLE\" WHERE USERID = @userId", parameters),
                    new Query("DELETE FROM \"MAINTABLE\" WHERE USERID = @userId", parameters)
                };
            tableSL.AddRange(tableMainL);
            return tableSL;
        }

        public static List<Query> CreateMenu(Config c)
        {
            Query query;
            List<Query> queries = new List<Query>();
            List<Goal> goals = c.GetGoals();
            Dictionary<string, object> parameters;
            int goalIndex = 1;
            int subIndicatorCounter = 0;

            foreach (Goal goal in goals)
            {
                string goalIndexDesc = goal.GetIndexDescription();
                parameters = new Dictionary<string, object> {
                    { "@goalIndexDesc", goalIndexDesc.Replace(" ", "_")},
                    { "@goalIndex3Desc", '0' + goalIndexDesc },
                    { "@descr", goal.desc },
                    { "@descEn", goal.descEn },
                    { "@userId", c.userId }
                };
                query = new Query("INSERT INTO \"MENUSELECTION\" (MENU, SELECTION, PRESTEXT, PRESTEXTS, DESCRIPTION, LEVELNO, SORTCODE, PRESENTATION, USERID, LOGDATE) VALUES " +
                    "('START', @goalIndexDesc, @descr, '', '', '1', @goalIndex3Desc, 'A', @userId, #TIME#)",
                    parameters);
                queries.Add(query);
                if (c.HasSecondaryLanguage())
                {
                    query = new Query("INSERT INTO \"MENUSELECTION_" + c.secondaryLanguage.suffixDB + "\" (MENU, SELECTION, PRESTEXT, PRESTEXTS, DESCRIPTION, SORTCODE, PRESENTATION, USERID, LOGDATE) VALUES " +
                        "('START', @goalIndexDesc, @descEn, '', '', @goalIndex3Desc, 'A', @userId, #TIME#)",
                        parameters);
                    queries.Add(query);
                }
                int targetIndex = 1;
                foreach (Target target in goal.targets)
                {
                    string targetIndexDesc = "T" + target.GetIndexDescription();
                    parameters = new Dictionary<string, object> {
                        { "@goalIndexDesc", goalIndexDesc.Replace(" ", "_") },
                        { "@targetIndexDesc", targetIndexDesc.Replace(" ", "_") },
                        { "@descr", target.desc },
                        { "@descEn", target.descEn },
                        { "@targetIndexPadded", LeftPadding(targetIndex, 3) },
                        { "@userId", c.userId }
                    };
                    query = new Query("INSERT INTO \"MENUSELECTION\"(MENU, SELECTION, PRESTEXT, PRESTEXTS, DESCRIPTION, LEVELNO, SORTCODE, PRESENTATION, USERID, LOGDATE) VALUES " +
                             "(@goalIndexDesc, @targetIndexDesc, @descr, '', '', '2', @targetIndexPadded, 'A', @userId, #TIME#)",
                             parameters);
                    queries.Add(query);
                    if (c.HasSecondaryLanguage())
                    {
                        query = new Query("INSERT INTO \"MENUSELECTION_" + c.secondaryLanguage.suffixDB + "\"(MENU, SELECTION, PRESTEXT, PRESTEXTS, DESCRIPTION, SORTCODE, PRESENTATION, USERID, LOGDATE) VALUES " +
                             "(@goalIndexDesc, @targetIndexDesc, @descEn, '', '', @targetIndexPadded, 'A', @userId, #TIME#)",
                             parameters);
                        queries.Add(query);
                    }
                    int indicatorIndex = 1;
                    foreach (Indicator indicator in target.indicators)
                    {
                        //    string indicatorIndexDesc = targetIndexDesc + "I" + indicator.GetIndexDescription();
                        string indicatorIndexDesc = indicator.GetIndexDescription();
                        parameters = new Dictionary<string, object> {
                            { "@targetIndexDesc", targetIndexDesc.Replace(" ", "_") },
                            { "@indicatorIndexDesc", indicatorIndexDesc.Replace(" ", "_") },
                            { "@indicatorDescr", indicator.desc },
                            { "@indicatorDescEn", indicator.descEn },
                            { "@indicatorIndexPadded", LeftPadding(indicatorIndex, 3) },
                            { "@userId", c.userId }
                        };
                        query = new Query("INSERT INTO \"MENUSELECTION\"(MENU, SELECTION, PRESTEXT, PRESTEXTS, DESCRIPTION, LEVELNO, SORTCODE, PRESENTATION, USERID, LOGDATE) VALUES " +
                                 "(@targetIndexDesc, @indicatorIndexDesc, @indicatorDescr, '', '', '3', @indicatorIndexPadded, 'A', @userId, #TIME#)",
                                 parameters);
                        queries.Add(query);
                        if (c.HasSecondaryLanguage())
                        {
                            query = new Query("INSERT INTO \"MENUSELECTION_" + c.secondaryLanguage.suffixDB + "\"(MENU, SELECTION, PRESTEXT, PRESTEXTS, DESCRIPTION, SORTCODE, PRESENTATION, USERID, LOGDATE) VALUES " +
                                 "(@targetIndexDesc, @indicatorIndexDesc, @indicatorDescEn, '', '', @indicatorIndexPadded, 'A', @userId, #TIME#)",
                                 parameters);
                            queries.Add(query);
                        }
                        int subIndicatorIndex = 1;
                        foreach (SubIndicator subIndicator in indicator.subIndicators)
                        {
                            string mainTable = subIndicator.GetMainTableName(target);
                            parameters = new Dictionary<string, object> {
                                { "@indicatorIndexDesc", indicatorIndexDesc.Replace(" ", "_") },
                                { "@mainTable", mainTable.Replace(" ", "_") },
                                { "@descr", subIndicator.desc },
                                { "@descEn", subIndicator.descEn },
                                { "@subIndicatorIndexPadded", LeftPadding(subIndicatorIndex, 3) },
                                { "@userId", c.userId }
                            };
                            query = new Query("INSERT INTO \"MENUSELECTION\"(MENU, SELECTION, PRESTEXT, PRESTEXTS, DESCRIPTION, LEVELNO, SORTCODE, PRESENTATION, USERID, LOGDATE) VALUES " +
                               "(@indicatorIndexDesc, @mainTable, @descr, '', '', '4', @subIndicatorIndexPadded, 'A', @userId, #TIME#)",
                               parameters);
                            queries.Add(query);
                            if (c.HasSecondaryLanguage())
                            {
                                query = new Query("INSERT INTO \"MENUSELECTION_" + c.secondaryLanguage.suffixDB + "\"(MENU, SELECTION, PRESTEXT, PRESTEXTS, DESCRIPTION, SORTCODE, PRESENTATION, USERID, LOGDATE) VALUES " +
                                    "(@indicatorIndexDesc, @mainTable, @descEn, '', '', @subIndicatorIndexPadded, 'A', @userId, #TIME#)",
                                    parameters);
                                queries.Add(query);
                            }
                            subIndicatorIndex++;
                            subIndicatorCounter++;
                        }

                        indicatorIndex++;
                    }

                    targetIndex++;
                }

                goalIndex++;
            }
            return queries;
        }

        public static List<Query> CreateVariables(Config c)
        {
            Query query;
            List<Query> queries = new List<Query>();
            Dictionary<string, object> parameters;

            foreach (Dimension dimension in c.dimensions)
            {
                Table table = dimension.table;
                if (table != null)
                {
                    parameters = new Dictionary<string, object> {
                        { "@name", dimension.name },
                        { "@labelA", dimension.label },
                        { "@labelEn", dimension.labelEn },
                        { "@userId", c.userId }
                    };
                    query = new Query("INSERT INTO \"VARIABLE\" (VARIABLE, PRESTEXT, VARIABLEINFO, FOOTNOTE, USERID, LOGDATE) VALUES " +
                        "(@name, @labelA, 'N', 'N', @userId, #TIME#)",
                        parameters);
                    queries.Add(query);
                    if (c.HasSecondaryLanguage())
                    {
                        query = new Query("INSERT INTO \"VARIABLE_" + c.secondaryLanguage.suffixDB + "\" (VARIABLE, PRESTEXT, USERID, LOGDATE) VALUES " +
                            "(@name, @labelEn, @userId, #TIME#)",
                            parameters);
                        queries.Add(query);
                    }
                    if (!dimension.isTime)
                    {
                        parameters = new Dictionary<string, object> {
                            { "@valuePool", dimension.GetValuePool() },
                            { "@desc", "Valuepool for variable " + dimension.label },
                            { "@userId", c.userId }
                        };
                        query = new Query("INSERT INTO \"VALUEPOOL\" (VALUEPOOL, PRESTEXT, DESCRIPTION, VALUETEXTEXISTS, VALUEPRES, METAID, USERID, LOGDATE) VALUES " +
                                "(@valuePool, @desc, 'None', 'L', 'T', '0', @userId, #TIME#)",
                                parameters);
                        queries.Add(query);
                        if (c.HasSecondaryLanguage())
                        {
                            query = new Query("INSERT INTO \"VALUEPOOL_" + c.secondaryLanguage.suffixDB + "\" (VALUEPOOL, PRESTEXT, ValuePoolAlias, USERID, LOGDATE) VALUES " +
                                "(@valuePool, @desc,'', @userId, #TIME#)",
                                parameters);
                            queries.Add(query);
                        }

                        int index = 1;
                        foreach (KeyValuePair<string, string> entry in dimension.values)
                        {
                            string key = entry.Key;
                            string desc = entry.Value;

                            string descEn = dimension.valuesEn[key];
                            parameters = new Dictionary<string, object> {
                                { "@valuePool", dimension.GetValuePool() },
                                { "@index", LeftPadding(index, 3) },
                                { "@key", key },
                                { "@descr", desc },
                                { "@descEn", !string.IsNullOrEmpty(descEn)?descEn:desc },
                                { "@userId", c.userId }
                            };
                            query = new Query("INSERT INTO \"VALUE\" (VALUEPOOL, VALUECODE, SORTCODE, VALUETEXTS, VALUETEXTL, FOOTNOTE, USERID, LOGDATE) VALUES " +
                                "(@valuePool, @key, @index, @descr, @descr, 'N', @userId, #TIME#)",
                                parameters);
                            queries.Add(query);
                            if (c.HasSecondaryLanguage())
                            {
                                query = new Query("INSERT INTO \"VALUE_" + c.secondaryLanguage.suffixDB + "\" (VALUEPOOL, VALUECODE, SORTCODE, VALUETEXTS, VALUETEXTL, USERID, LOGDATE) VALUES " +
                                "(@valuePool, @key, @index, @descEn, @descEn, @userId, #TIME#)",
                                parameters);
                                queries.Add(query);
                            }

                            index++;
                        }
                    }
                }
            }

            return queries;
        }

        public static List<Query> CreateCubes(Config c)
        {
            DBConnection con1 = null;
            try
            {
                con1 = new DBConnection(c.mdtDb);
                Query query;
                List<Query> queries = new List<Query>();
                Dictionary<string, object> parameters;
                List<Goal> goals = c.GetGoals();
                int subIndicatorCounter = 0;
                foreach (Goal goal in goals)
                {
                    foreach (Target target in goal.targets)
                    {
                        foreach (Indicator indicator in target.indicators)
                        {
                            foreach (SubIndicator subIndicator in indicator.subIndicators)
                            {
                                string mainTable = subIndicator.GetMainTableName(target);
                                parameters = new Dictionary<string, object> {
                                    { "@mainTable", mainTable.Replace(" ", "_") },
                                    { "@descr", target.code + "/" + subIndicator.codeValue + "/" + subIndicator.desc },
                                    { "@descEn", target.code + "/" + subIndicator.codeValue + "/" + subIndicator.descEn },
                                    { "@subIndicatorCode", subIndicator.codeValue },
                                    { "@obsValue", c.obsValue },
                                    { "@organizationCode", c.organizationCode },
                                    { "@userId", c.userId }
                                };
                                query = new Query("INSERT INTO \"MAINTABLE\" (MAINTABLE, PRESTEXT, PRESTEXTS, PRESCATEGORY, TIMESCALE, USERID, LOGDATE, FIRSTPUBLISHED, TABLESTATUS, TABLEID, PRODUCTCODE, SPECCHAREXISTS, METAID, SUBJECTCODE, CONTENTSVARIABLE) VALUES " +
                                    " (@mainTable, @descr, '" + c.goalTypeLabel + "', 'O', 'year', @userId, #TIME#, #TIME#, 'A', @mainTable, '001', 'N', '01', '01', '" + c.mainLanguage.labelValue + "')",
                                    parameters);
                                queries.Add(query);
                                if (c.HasSecondaryLanguage())
                                {
                                    query = new Query("INSERT INTO \"MAINTABLE_" + c.secondaryLanguage.suffixDB + "\" (MAINTABLE, STATUS, PUBLISHED, PRESTEXT, PRESTEXTS, CONTENTSVARIABLE, USERID, LOGDATE) VALUES " +
                                        " (@mainTable, 'A', #TIME#, @descEn, '" + c.goalTypeLabel + "', '" + c.secondaryLanguage.labelValue + "', @userId, #TIME#)",
                                        parameters);
                                    queries.Add(query);

                                    query = new Query("INSERT INTO \"SECONDARYLANGUAGE\" (MAINTABLE, LANGUAGE, COMPLETELYTRANSLATED,PUBLISHED, USERID, LOGDATE) VALUES " +
                                        " (@mainTable, '" + c.secondaryLanguage.code + "','Y', 'Y', @userId, #TIME#)",
                                        parameters);
                                    queries.Add(query);
                                }
                                query = new Query("INSERT INTO \"CONTENTS\" (MAINTABLE, CONTENTS, PRESTEXT, PRESCODE, COPYRIGHT, PRODUCER, LASTUPDATED, UNIT, PRESDECIMALS, PRESCELLSZERO, STOCKFA, DAYADJ, SEASADJ, FOOTNOTECONTENTS, FOOTNOTEVARIABLE, FOOTNOTEVALUE, STORECOLUMNNO, STOREFORMAT, STORENOCHAR, STOREDECIMALS, USERID, LOGDATE, PUBLISHED, PRESTEXTS, STATAUTHORITY, AGGREGPOSSIBLE, FOOTNOTETIME, METAID) VALUES " +
                                    " (@mainTable, @obsValue, '" + c.mainLanguage.labelContent + "', 'DATA', '1', @organizationCode, #TIME#, @obsValue, '2', 'Y', 'F', 'N', 'N', 'N', 'N', 'N', '3', 'N', '17', '2', @userId, #TIME#, #TIME#, @subIndicatorCode, @organizationCode, 'N', 'N', '01')",
                                    parameters);
                                queries.Add(query);
                                if (c.HasSecondaryLanguage())
                                {
                                    query = new Query("INSERT INTO \"CONTENTS_" + c.secondaryLanguage.suffixDB + "\" (MAINTABLE, CONTENTS, PRESTEXT, UNIT, USERID, LOGDATE, PRESTEXTS, REFPERIOD, BASEPERIOD)  VALUES " +
                                        " (@mainTable, @obsValue, '" + c.secondaryLanguage.labelContent + "',  @obsValue, @userId, #TIME#, @subIndicatorCode, NULL,NULL)",
                                        parameters);
                                    queries.Add(query);
                                }
                                query = new Query("INSERT INTO \"SUBTABLE\" (MAINTABLE, SUBTABLE, PRESTEXT, CLEANTABLE, USERID, LOGDATE) VALUES " +
                                    " (@mainTable, '', @descr, 'Y', @userId, #TIME#)",
                                    parameters);
                                queries.Add(query);
                                if (c.HasSecondaryLanguage())
                                {
                                    query = new Query("INSERT INTO \"SUBTABLE_" + c.secondaryLanguage.suffixDB + "\" (MAINTABLE, SUBTABLE, PRESTEXT, USERID, LOGDATE) VALUES " +
                                        " (@mainTable, '', @descEn, @userId, #TIME#)",
                                        parameters);
                                    queries.Add(query);
                                }

                                foreach (Dimension dimension in subIndicator.dimensions)
                                {
                                    if (dimension.isTime)
                                    {
                                        parameters = new Dictionary<string, object> {
                                            { "@mainTable", mainTable },
                                            { "@dimensionName", dimension.name },
                                            { "@userId", c.userId }
                                        };
                                        query = new Query("INSERT INTO \"SUBTABLEVARIABLE\" (MAINTABLE, SUBTABLE, VARIABLE, VARIABLETYPE, STORECOLUMNNO, USERID, LOGDATE) VALUES " +
                                            "(@mainTable, '', @dimensionName, 'T', '1', @userId, #TIME#)",
                                            parameters);
                                        queries.Add(query);

                                        query = new Query("SELECT DISTINCT TIME_PERIOD FROM " + c.mdtDb.dataTable.name + " WHERE " + c.mdtDb.dataTable.code + " = @subindicator AND " + c.obsValue + " IS NOT NULL ORDER BY TIME_PERIOD", new Dictionary<string, object> { { "@subindicator", subIndicator.code } });
                                        DbDataReader reader = query.ExecuteReader(con1);
                                        while (reader.Read())
                                        {
                                            parameters = new Dictionary<string, object> {
                                                    { "@mainTable", mainTable },
                                                    { "@key", reader.GetInt32(0) },
                                                    { "@obsValue", c.obsValue },
                                                    { "@userId", c.userId }
                                                };
                                            query = new Query("INSERT INTO \"CONTENTSTIME\" (MAINTABLE, CONTENTS, TIMEPERIOD, USERID, LOGDATE) VALUES " +
                                                "(@mainTable, @obsValue, @key, @userId, #TIME#)",
                                                parameters);
                                            queries.Add(query);
                                        }
                                        reader.Close();
                                    }
                                    else if (dimension.table != null)
                                    {
                                        List<string> codes = new List<string>();
                                        query = new Query("SELECT DISTINCT " + dimension.name + " FROM " + c.mdtDb.dataTable.name + " WHERE " + c.mdtDb.dataTable.code + " = @subindicator AND " + c.obsValue + " IS NOT NULL AND " + dimension.name + " IS NOT NULL ORDER BY " + dimension.name, new Dictionary<string, object> { { "@subindicator", subIndicator.code } });
                                        DbDataReader reader = query.ExecuteReader(con1);
                                        while (reader.Read())
                                        {
                                            codes.Add(reader.GetString(0).Trim());
                                        }
                                        reader.Close();
                                        if (codes.Count > 1)
                                        {
                                            queries.AddRange(CreateValueSet(c, subIndicator, dimension, codes));

                                            parameters = new Dictionary<string, object> {
                                                { "@mainTable", mainTable },
                                                { "@dimensionName", dimension.name },
                                                { "@valueSet", dimension.GetValueSet(subIndicator) },
                                                { "@obsValue", c.obsValue },
                                                { "@userId", c.userId }
                                            };
                                            query = new Query("INSERT INTO \"SUBTABLEVARIABLE\" (MAINTABLE, SUBTABLE, VARIABLE, VALUESET, VARIABLETYPE, STORECOLUMNNO, USERID, LOGDATE) VALUES " +
                                                "(@mainTable, '', @dimensionName, @valueSet, 'V', '1', @userId, #TIME#)",
                                                parameters);
                                            queries.Add(query);
                                        }
                                    }
                                }

                                queries.AddRange(c.pxwebDb.dataTable.GetDropCreateQueries(c.mdtDb, mainTable, subIndicator));

                                subIndicatorCounter++;
                            }
                        }
                    }
                }
                return queries;
            }
            finally
            {
                if (con1 != null)
                {
                    con1.CloseConnection();
                }
            }
        }

        private static List<Query> CreateValueSet(Config c, SubIndicator subIndicator, Dimension dimension, List<string> codes)
        {
            Query query;
            List<Query> queries = new List<Query>();
            Dictionary<string, object> parameters;

            if (inserted.Contains(dimension.name + "#" + subIndicator.codeValue))
            {
                return queries;
            }
            inserted.Add(dimension.name + "#" + subIndicator.codeValue);

            Table table = dimension.table;
            if (table != null && !dimension.isTime)
            {
                parameters = new Dictionary<string, object> {
                    { "@valuePool", dimension.GetValuePool() },
                    { "@valueSet", dimension.GetValueSet(subIndicator) },
                    { "@desc", "Valuepool for variable " + dimension.label + "_" + subIndicator.code },
                    { "@userId", c.userId }
                };

                query = new Query("INSERT INTO \"VALUESET\" (VALUESET, PRESTEXT, DESCRIPTION, ELIMINATION, VALUEPOOL, METAID, FOOTNOTE, USERID, LOGDATE, VALUEPRES, SORTCODEEXISTS) VALUES " +
                        "(@valueSet, 'Hmm', 'None', 'A', @valuePool, '0', 'N', '" + c.userId + "', #TIME#, 'T', 'N')",
                        parameters);
                queries.Add(query);
                if (c.HasSecondaryLanguage())
                {
                    query = new Query("INSERT INTO \"VALUESET_" + c.secondaryLanguage.suffixDB + "\" (VALUESET, PRESTEXT, DESCRIPTION, USERID, LOGDATE) VALUES " +
                        "(@valueSet, 'Hmm', 'None', '" + c.userId + "', #TIME#)",
                        parameters);
                    queries.Add(query);
                }

                int index = 1;
                foreach (KeyValuePair<string, string> entry in dimension.values)
                {
                    if (!codes.Contains(entry.Key))
                    {
                        continue;
                    }

                    string key = entry.Key;
                    string desc = entry.Value;

                    string descEn = dimension.valuesEn[key];
                    parameters = new Dictionary<string, object> {
                        { "@valuePool", dimension.GetValuePool() },
                        { "@valueSet", dimension.GetValueSet(subIndicator) },
                        { "@index", LeftPadding(index, 3) },
                        { "@key", key },
                        { "@descr", desc },
                        { "@descEn", !string.IsNullOrEmpty(descEn)?descEn:desc },
                        { "@userId", c.userId }
                    };
                    query = new Query("INSERT INTO \"VSVALUE\" (VALUESET, VALUEPOOL, VALUECODE, USERID, LOGDATE) VALUES " +
                            "(@valueSet, @valuePool, @key, @userId, #TIME#)",
                            parameters);
                    queries.Add(query);
                    if (c.HasSecondaryLanguage())
                    {
                        query = new Query("INSERT INTO \"VSVALUE_" + c.secondaryLanguage.suffixDB + "\" (VALUESET, VALUEPOOL, VALUECODE, USERID, LOGDATE) VALUES " +
                                "(@valueSet, @valuePool, @key, @userId, #TIME#)",
                                parameters);
                        queries.Add(query);
                    }

                    index++;
                }
            }

            return queries;
        }

        private static string LeftPadding(int i, int length)
        {
            string s = "" + i;
            while (s.Length < length)
            {
                s = "0" + s;
            }
            return s;
        }

        public static string Truncate(string value, int maxLength)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
            {
                return value.Substring(0, maxLength);
            }
            return value;
        }

    }

}
