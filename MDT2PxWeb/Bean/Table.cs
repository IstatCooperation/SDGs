using System.Collections.Generic;

namespace MDT2PxWeb.Bean
{
    public class Table
    {
        public string name;
        public string id;
        public string code;
        public string code1;
        public string typeId;
        public string codeValue;
        public string desc;
        public string descEn;
        public string dimensions;
        public string order;
        public string reference;
        public string reference1;
        public string reference2;
        public string condition;
        public bool integerCode = false;

        public string GetSqlSelect(string refValue = null)
        {
            string query = "SELECT " + code + ", " + desc;
            if (!string.IsNullOrEmpty(descEn))
            {
                query += ", " + descEn;
            }
            if (!string.IsNullOrEmpty(dimensions))
            {
                query += ", " + dimensions;
            }
            if (!string.IsNullOrEmpty(id))
            {
                query += ", " + id;
            }
            if (!string.IsNullOrEmpty(typeId))
            {
                query += ", " + typeId;
            }
            if (!string.IsNullOrEmpty(codeValue))
            {
                query += ", " + codeValue;
            }
            query += " FROM " + name;
            bool where = false;
            if (!string.IsNullOrEmpty(reference))
            {
                query += " WHERE " + reference + " = @REFERENCE";
                where = true;
            }
            if (!string.IsNullOrEmpty(reference1))
            {
                query += " AND " + reference1 + " = @REFERENCE1";

            }
            if (!string.IsNullOrEmpty(reference2))
            {
                query += " AND " + reference2 + " = @REFERENCE2";

            }
            if (!string.IsNullOrEmpty(condition))
            {
                if (where)
                {
                    query += " AND";
                }
                else
                {
                    query += " WHERE";
                }
                query += " (" + condition + ")";
            }
            if (!string.IsNullOrEmpty(order))
            {
                query += " ORDER BY " + order;
            }
            return query;
        }

        public List<Query> GetDropCreateQueries(Database database, string mainTable, SubIndicator subIndicator)
        {
            Query query;
            List<Query> queries = new List<Query>();
            switch (database.type)
            {
                case "ORACLE":
                    query = new Query("CREATE OR REPLACE VIEW " + mainTable + " AS SELECT * FROM " + this.name + " WHERE " + this.code + " = '" + subIndicator.code + "'");
                    queries.Add(query);
                    break;
                default:
                    query = new Query("IF EXISTS (SELECT 1 FROM SYS.VIEWS WHERE NAME = '" + mainTable + "' AND TYPE  = 'v') DROP VIEW " + mainTable);
                    queries.Add(query);
                    query = new Query("GO");
                    queries.Add(query);
                    query = new Query("CREATE VIEW " + mainTable + " AS SELECT * FROM " + this.name + " WHERE " + this.code + " = '" + subIndicator.code + "'");
                    queries.Add(query);
                    query = new Query("GO");
                    queries.Add(query);
                    break;
            }
            return queries;
        }

        public string GetDescription()
        {
            return "{name:" + name + ",code:" + code + ",desc:" + desc + ",dimensions:" + dimensions + ",order:" + order + ",reference:" + reference + ",condition:" + condition + ",integerCode:" + integerCode + "}";
        }

        public string GetDescriptionEn()
        {
            return "{name:" + name + ",code:" + code + ",descEn:" + descEn + ",dimensions:" + dimensions + ",order:" + order + ",reference:" + reference + ",condition:" + condition + ",integerCode:" + integerCode + "}";
        }

    }
}
