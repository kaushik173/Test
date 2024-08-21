using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using System.Data.Entity.Core.EntityClient;
using LALoDep.Domain;


namespace LALoDep.Domain.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly LALoDepEntities _context;

        public UtilityService(LALoDepEntities context)
        {
            _context = context;

        }
        public LALoDepEntities Context
        {
            get { return _context; }
        }


        public void ExecStoredProcedureWithoutResults(string name, Dictionary<string, object> parameters = null)
        {
            var addedParams = new StringBuilder();

            if (parameters != null)
            {

                foreach (var param in parameters)
                {
                    if (param.Value != null)
                    {
                        var value = param.Value;

                        if (value.ToString().Contains("'"))
                        {
                            value = value.ToString().Replace("'", "''");
                        }

                        addedParams.Append(string.Format("{0}=N'{1}',", param.Key, value));
                    }
                    else
                    {
                        addedParams.Append(string.Format("{0}=null,", param.Key));
                    }
                }
            }

            if (addedParams.Length > 0)
            {
                addedParams = addedParams.Remove(addedParams.Length - 1, 1);
            }

            _context.Database.ExecuteSqlCommand(string.Format("EXEC [dbo].{0} {1}", name, addedParams));
        }
        public void ExecStoredProcedureWithoutResults(string name, object parameterWrapper)
        {
            var addedParams = new StringBuilder();

            if (parameterWrapper != null)
            {
                foreach (var prop in parameterWrapper.GetType().GetProperties())
                {
                    var propValue = prop.GetValue(parameterWrapper, null);

                    if (propValue != null)
                    {
                        var value = propValue;

                        if (value.ToString().Contains("'"))
                        {
                            value = value.ToString().Replace("'", "''");
                        }


                        addedParams.Append(string.Format("@{0}=N'{1}',", prop.Name, value));
                    }
                    else
                    {
                        addedParams.Append(string.Format("@{0}=null,", prop.Name, propValue));
                    }
                }
            }

            if (addedParams.Length > 0)
            {
                addedParams = addedParams.Remove(addedParams.Length - 1, 1);
            }
            _context.Database.ExecuteSqlCommand(string.Format("EXEC [dbo].{0} {1}", name, addedParams));

        }
        public object ExecQueryScalerADO(string query)
        {

            var dbManager = new DbManager();
            return dbManager.ExecuteScalar(query, CommandType.Text);


        }
        public void ExecQueryWithoutResultADO(string query)
        {

            var dbManager = new DbManager();
            dbManager.ExecuteNonQuery(query, CommandType.Text);


        }
        public void ExecStoredProcedureWithoutResultADO(string name, object parameterWrapper)
        {
            var addedParams = new StringBuilder();

            if (parameterWrapper != null)
            {
                foreach (var prop in parameterWrapper.GetType().GetProperties())
                {
                    var propName = prop.Name;
                    var propValue = prop.GetValue(parameterWrapper, null);
                    if (propName.Contains("Number_602ClientRoleIDList"))
                    {
                        propName = propName.Replace("Number_602ClientRoleIDList", "602ClientRoleIDList");
                    }
                    if (propValue != null)
                    {
                        var value = propValue;

                        if (value.ToString().Contains("'"))
                        {
                            value = value.ToString().Replace("'", "''");
                        }


                        addedParams.Append(string.Format("@{0}=N'{1}',", propName, value));
                    }
                    else
                    {
                        addedParams.Append(string.Format("@{0}=null,", propName, propValue));
                    }
                }
            }

            if (addedParams.Length > 0)
            {
                addedParams = addedParams.Remove(addedParams.Length - 1, 1);
            }

            var dbManager = new DbManager();
            dbManager.ExecuteNonQuery(string.Format("EXEC [dbo].{0} {1}", name, addedParams), CommandType.Text);


        }
        public IEnumerable<T> ExecStoredProcedureWithResults<T>(string name, Dictionary<string, object> parameters = null)
        {
            var addedParams = new StringBuilder();

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    if (param.Value != null)
                    {
                        var value = param.Value;

                        if (value.ToString().Contains("'"))
                        {
                            value = value.ToString().Replace("'", "''");
                        }

                        addedParams.Append(string.Format("{0}=N'{1}',", param.Key, value));
                    }
                    else
                    {
                        addedParams.Append(string.Format("{0}=null,", param.Key));
                    }
                }
            }

            if (addedParams.Length > 0)
            {
                addedParams = addedParams.Remove(addedParams.Length - 1, 1);
            }

            return _context.Database.SqlQuery<T>(string.Format("EXEC [dbo].{0} {1}", name, addedParams)).ToList();
        }

        public IEnumerable<T> ExecStoredProcedureWithResults<T>(string name, object parameterWrapper, int timeout = 0)
        {
            var addedParams = new StringBuilder();

            if (parameterWrapper != null)
            {
                foreach (var prop in parameterWrapper.GetType().GetProperties())
                {
                    var propValue = prop.GetValue(parameterWrapper, null);

                    if (propValue != null)
                    {
                        var value = propValue;

                        if (value.ToString().Contains("'"))
                        {
                            value = value.ToString().Replace("'", "''");
                        }

                        addedParams.Append(string.Format("@{0}=N'{1}',", prop.Name, value));
                    }
                    else
                    {
                        addedParams.Append(string.Format("@{0}=null,", prop.Name, propValue));
                    }
                }
            }

            if (addedParams.Length > 0)
            {
                addedParams = addedParams.Remove(addedParams.Length - 1, 1);
            }
            if (timeout > 0)
                _context.Database.CommandTimeout = timeout;
            return _context.Database.SqlQuery<T>(string.Format("EXEC [dbo].{0} {1}", name, addedParams)).ToList();
        }

        public DataTable ExecStoredProcedureForDataTable(string name, object parameterWrapper)
        {

            DataTable retVal = new DataTable();
            var conn = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand(name, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adpter = new SqlDataAdapter(cmd);
                if (parameterWrapper != null)
                {
                    foreach (var prop in parameterWrapper.GetType().GetProperties())
                    {
                        var propValue = prop.GetValue(parameterWrapper, null);
                        SqlParameter param = new SqlParameter(prop.Name, DBNull.Value);
                        if (propValue != null) param.Value = propValue;
                        param.Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(param);
                    }
                }
                adpter.Fill(retVal);
                return retVal;
            }
        }

        public DataTable ExecStoredProcedureForDataTableADO(string name, Dictionary<string, object> parameterWrapper = null, int timeout = 0)
        {

            var dbManager = new DbManager(timeout);
            if (parameterWrapper != null)
            {
                foreach (var param in parameterWrapper)
                {
                    if (param.Value != null)
                    {
                        var value = param.Value;


                        dbManager.AddInParam(param.Key, value);

                    }
                    else
                    {
                        dbManager.AddInParam(param.Key, "");
                    }
                }

            }
            return dbManager.ExecuteDataTable(name, CommandType.StoredProcedure);
        }

        public DataTable ExecStoredProcedureForDataTableADO(string name, object parameterWrapper, int timeout = 0)
        {

            var addedParams = new StringBuilder();

            if (parameterWrapper != null)
            {
                foreach (var prop in parameterWrapper.GetType().GetProperties())
                {
                    var propValue = prop.GetValue(parameterWrapper, null);

                    if (propValue != null)
                    {
                        var value = propValue;

                        if (value.ToString().Contains("'"))
                        {
                            value = value.ToString().Replace("'", "''");
                        }


                        addedParams.Append(string.Format("@{0}=N'{1}',", prop.Name, value));
                    }
                    else
                    {
                        addedParams.Append(string.Format("@{0}=null,", prop.Name, propValue));
                    }
                }
            }

            if (addedParams.Length > 0)
            {
                addedParams = addedParams.Remove(addedParams.Length - 1, 1);
            }
            var dbManager = new DbManager(timeout);
            return dbManager.ExecuteDataTable(string.Format("EXEC [dbo].{0} {1}", name, addedParams), CommandType.Text);

        }

        public DataTable ExecQueryForDataTable(string name, object parameterWrapper)
        {

            DataTable retVal = new DataTable();
            var conn = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand(name, conn))
            {
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adpter = new SqlDataAdapter(cmd);
                if (parameterWrapper != null)
                {
                    foreach (var prop in parameterWrapper.GetType().GetProperties())
                    {
                        var propValue = prop.GetValue(parameterWrapper, null);
                        SqlParameter param = new SqlParameter(prop.Name, null);
                        if (propValue != null) param.Value = propValue;
                        param.Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(param);
                    }
                }
                adpter.Fill(retVal);
                return retVal;
            }
        }

        public IEnumerable<T> ExecStoredProcedureWhichHasDuplicateColumnsWithResults<T>(string name, object parameterWrapper) where T : new()
        {
            var data = new List<T>();
            var retVal = new DataTable();
            var conn = (SqlConnection)_context.Database.Connection;
            using (var cmd = new SqlCommand(name, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var adpter = new SqlDataAdapter(cmd);
                if (parameterWrapper != null)
                {
                    foreach (var prop in parameterWrapper.GetType().GetProperties())
                    {
                        var propValue = prop.GetValue(parameterWrapper, null);
                        var param = new SqlParameter(prop.Name, null);
                        if (propValue != null) param.Value = propValue;
                        param.Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(param);
                    }
                }
                adpter.Fill(retVal);

            }

            if (retVal.Rows.Count > 0)
            {
                foreach (DataRow dr in retVal.Rows)
                {
                    var m = new T();
                    foreach (var prop in m.GetType().GetProperties())
                    {
                        try
                        {
                            prop.SetValue(m, dr[prop.Name]);
                        }
                        catch (Exception) { }
                    }

                    data.Add(m);
                }
            }
            return data;

        }

        public IEnumerable<T> ExecSQlQueryWithResults<T>(string query, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>(query, parameters).ToList();
        }

        public IQueryable<T> ExecSQlQueryWithResultsAsQueryable<T>(string query, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>(query, parameters).AsQueryable();
        }

        public object ExecStoredProcedureScalar(string name, object parameterWrapper)
        {
            var addedParams = new StringBuilder();

            if (parameterWrapper != null)
            {
                foreach (var prop in parameterWrapper.GetType().GetProperties())
                {
                    var propValue = prop.GetValue(parameterWrapper, null);

                    if (propValue != null)
                    {
                        var value = propValue;

                        if (value.ToString().Contains("'"))
                        {
                            value = value.ToString().Replace("'", "''");
                        }


                        addedParams.Append(string.Format("@{0}=N'{1}',", prop.Name, value));
                    }
                    else
                    {
                        addedParams.Append(string.Format("@{0}=null,", prop.Name, propValue));
                    }
                }
            }

            if (addedParams.Length > 0)
            {
                addedParams = addedParams.Remove(addedParams.Length - 1, 1);
            }

            var dbManager = new DbManager();
            return dbManager.ExecuteScalar(string.Format("EXEC [dbo].{0} {1}", name, addedParams), CommandType.Text);


        }
        public string ExecStoredProcedureWithJsonResult(string name, object parameterWrapper)
        {
            var jsonData = "";
            var jsonDataList = ExecStoredProcedureWithResults<string>(name, parameterWrapper).ToList();
            foreach (var json in jsonDataList)
                jsonData += json;
            return jsonData;
        }


    }

    public interface IUtilityService
    {
        LALoDepEntities Context { get; }
        void ExecStoredProcedureWithoutResults(string name, Dictionary<string, object> parameters);
        void ExecStoredProcedureWithoutResults(string name, object parameterWrapper = null);
        void ExecStoredProcedureWithoutResultADO(string name, object parameterWrapper = null);
        void ExecQueryWithoutResultADO(string query);
        IEnumerable<T> ExecStoredProcedureWithResults<T>(string name, Dictionary<string, object> parameters = null);
        IEnumerable<T> ExecStoredProcedureWithResults<T>(string name, object parameterWrapper = null, int timeout = 0);
        DataTable ExecStoredProcedureForDataTable(string name, object parameterWrapper);
        IEnumerable<T> ExecStoredProcedureWhichHasDuplicateColumnsWithResults<T>(string name, object parameterWrapper) where T : new();
        IEnumerable<T> ExecSQlQueryWithResults<T>(string query, params object[] parameters);
        IQueryable<T> ExecSQlQueryWithResultsAsQueryable<T>(string query, params object[] parameters);
        DataTable ExecQueryForDataTable(string name, object parameterWrapper);
        object ExecStoredProcedureScalar(string name, object parameterWrapper);
        DataTable ExecStoredProcedureForDataTableADO(string name, Dictionary<string, object> parameterWrapper = null, int timeout = 0);
        DataTable ExecStoredProcedureForDataTableADO(string name, object parameterWrapper, int timeout = 0);
        object ExecQueryScalerADO(string query);
        string ExecStoredProcedureWithJsonResult(string name, object parameterWrapper);
    }
}
