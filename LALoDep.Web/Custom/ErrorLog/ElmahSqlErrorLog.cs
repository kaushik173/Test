﻿#region Imports

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using Elmah;
using LALoDep.Domain;
using ApplicationException = System.ApplicationException;

#endregion

namespace LALoDep.Custom.ErrorLog
{
    public class SqlErrorLog : Elmah.ErrorLog
    {
        private readonly string _connectionString;

        private const int _maxAppNameLength = 60;

        private delegate RV Function<RV, A>(A a);

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlErrorLog"/> class
        /// using a dictionary of configured settings.
        /// </summary>

        public SqlErrorLog(IDictionary config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

             
            string appName = NullString((string)config["applicationName"]);

            if (appName.Length > _maxAppNameLength)
            {
                throw new ApplicationException(string.Format(
                    "Application name is too long. Maximum length allowed is {0} characters.",
                    _maxAppNameLength.ToString("N0")));
            }

            ApplicationName = appName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlErrorLog"/> class
        /// to use a specific connection string for connecting to the database.
        /// </summary>

        public SqlErrorLog(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");

            if (connectionString.Length == 0)
                throw new ArgumentException(null, "connectionString");

            _connectionString = connectionString;
        }

        /// <summary>
        /// Gets the name of this error log implementation.
        /// </summary>

        public override string Name
        {
            get { return "Microsoft SQL Server Error Log"; }
        }

        /// <summary>
        /// Gets the connection string used by the log to connect to the database.
        /// </summary>

        public virtual string ConnectionString
        {
            get { return LALoDepEntities.GetSqlConnectionString(); }
        }

        /// <summary>
        /// Logs an error to the database.
        /// </summary>
        /// <remarks>
        /// Use the stored procedure called by this implementation to set a
        /// policy on how long errors are kept in the log. The default
        /// implementation stores all errors for an indefinite time.
        /// </remarks>

        public override string Log(Error error)
        {
            return "";
            if (error == null)
                throw new ArgumentNullException("error");

            string errorXml = ErrorXml.EncodeString(error);
            Guid id = Guid.NewGuid();

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            using (SqlCommand command = Commands.LogError(
                id, this.ApplicationName,
                error.HostName, error.Type, error.Source, error.Message, error.User,
                error.StatusCode, error.Time.ToUniversalTime(), errorXml))
            {
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();

             


                return id.ToString();
            }
        }

        /// <summary>
        /// Returns a page of errors from the databse in descending order 
        /// of logged time.
        /// </summary>

        public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
        {
            if (pageIndex < 0)
                throw new ArgumentOutOfRangeException("pageIndex", pageIndex, null);

            if (pageSize < 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, null);

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            using (SqlCommand command = Commands.GetErrorsXml(this.ApplicationName, pageIndex, pageSize))
            {
                command.Connection = connection;
                connection.Open();

                XmlReader reader = command.ExecuteXmlReader();

                try
                {
                    ErrorsXmlToList(reader, errorEntryList);
                }
                finally
                {
                    reader.Close();
                }

                int total;
                Commands.GetErrorsXmlOutputs(command, out total);
                return total;
            }
        }

        /// <summary>
        /// Begins an asynchronous version of <see cref="GetErrors"/>.
        /// </summary>

        public override IAsyncResult BeginGetErrors(int pageIndex, int pageSize,
            IList errorEntryList, AsyncCallback asyncCallback, object asyncState)
        {
            if (pageIndex < 0)
                throw new ArgumentOutOfRangeException("pageIndex", pageIndex, null);

            if (pageSize < 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, null);

            //
            // Modify the connection string on the fly to support async 
            // processing otherwise the asynchronous methods on the
            // SqlCommand will throw an exception. This ensures the
            // right behavior regardless of whether configured
            // connection string sets the Async option to true or not.
            //

            SqlConnectionStringBuilder csb =
                new SqlConnectionStringBuilder(this.ConnectionString);
            csb.AsynchronousProcessing = true;
            SqlConnection connection = new SqlConnection(csb.ConnectionString);

            //
            // Create the command object with input parameters initialized
            // and setup to call the stored procedure.
            //

            SqlCommand command =
                Commands.GetErrorsXml(this.ApplicationName, pageIndex, pageSize);
            command.Connection = connection;

            //
            // Create a closure to handle the ending of the async operation
            // and retrieve results.
            //

            AsyncResultWrapper asyncResult = null;

            Function<int, IAsyncResult> endHandler = delegate
            {
                Debug.Assert(asyncResult != null);

                using (connection)
                using (command)
                {
                    using (XmlReader reader =
                        command.EndExecuteXmlReader(asyncResult.InnerResult))
                        ErrorsXmlToList(reader, errorEntryList);

                    int total;
                    Commands.GetErrorsXmlOutputs(command, out total);
                    return total;
                }
            };

            //
            // Open the connenction and execute the command asynchronously,
            // returning an IAsyncResult that wrap the downstream one. This
            // is needed to be able to send our own AsyncState object to
            // the downstream IAsyncResult object. In order to preserve the
            // one sent by caller, we need to maintain and return it from
            // our wrapper.
            //

            try
            {
                connection.Open();

                asyncResult = new AsyncResultWrapper(
                    command.BeginExecuteXmlReader(
                        asyncCallback != null ? /* thunk */
                            delegate { asyncCallback(asyncResult); } : (AsyncCallback)null,
                        endHandler), asyncState);

                return asyncResult;
            }
            catch (Exception)
            {
                connection.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Ends an asynchronous version of <see cref="ErrorLog.GetErrors"/>.
        /// </summary>

        public override int EndGetErrors(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            AsyncResultWrapper wrapper = asyncResult as AsyncResultWrapper;

            if (wrapper == null)
                throw new ArgumentException("Unexepcted IAsyncResult type.", "asyncResult");

            Function<int, IAsyncResult> endHandler =
                (Function<int, IAsyncResult>)wrapper.InnerResult.AsyncState;
            return endHandler(wrapper.InnerResult);
        }

        private void ErrorsXmlToList(XmlReader reader, IList errorEntryList)
        {
            Debug.Assert(reader != null);

            if (errorEntryList != null)
            {
                while (reader.IsStartElement("error"))
                {
                    string id = reader.GetAttribute("errorId");
                    Error error = ErrorXml.Decode(reader);
                    errorEntryList.Add(new ErrorLogEntry(this, id, error));
                }
            }
        }

        /// <summary>
        /// Returns the specified error from the database, or null 
        /// if it does not exist.
        /// </summary>
        public override ErrorLogEntry GetError(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            if (id.Length == 0)
                throw new ArgumentException(null, "id");

            Guid errorGuid;

            try
            {
                errorGuid = new Guid(id);
            }
            catch (FormatException e)
            {
                throw new ArgumentException(e.Message, "id", e);
            }

            string errorXml;

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            using (SqlCommand command = Commands.GetErrorXml(this.ApplicationName, errorGuid))
            {
                command.Connection = connection;
                connection.Open();
                errorXml = (string)command.ExecuteScalar();
            }

            if (errorXml == null)
                return null;

            Error error = ErrorXml.DecodeString(errorXml);
            return new ErrorLogEntry(this, id, error);
        }

        // These utility functions were marked as internal, so I had to copy them locally
        public static string NullString(string s)
        {
            return s ?? string.Empty;
        }

        public static string EmptyString(string s, string filler)
        {
            return NullString(s).Length == 0 ? filler : s;
        }

        // End

        private sealed class Commands
        {
            private Commands() { }

            public static SqlCommand LogError(
                Guid id,
                string appName,
                string hostName,
                string typeName,
                string source,
                string message,
                string user,
                int statusCode,
                DateTime time,
                string xml)
            {
                SqlCommand command = new SqlCommand("ELMAH_LogError");
                command.CommandType = CommandType.StoredProcedure;

                SqlParameterCollection parameters = command.Parameters;

                parameters.Add("@ErrorId", SqlDbType.UniqueIdentifier).Value = id;
                parameters.Add("@Application", SqlDbType.NVarChar, _maxAppNameLength).Value = appName;
                parameters.Add("@Host", SqlDbType.NVarChar, 30).Value = hostName;
                parameters.Add("@Type", SqlDbType.NVarChar, 100).Value = typeName;
                parameters.Add("@Source", SqlDbType.NVarChar, 60).Value = source;
                parameters.Add("@Message", SqlDbType.NVarChar, 500).Value = message;
                parameters.Add("@User", SqlDbType.NVarChar, 50).Value = user;
                parameters.Add("@AllXml", SqlDbType.NText).Value = xml;
                parameters.Add("@StatusCode", SqlDbType.Int).Value = statusCode;
                parameters.Add("@TimeUtc", SqlDbType.DateTime).Value = time;

                return command;
            }

            public static SqlCommand GetErrorXml(string appName, Guid id)
            {
                SqlCommand command = new SqlCommand("ELMAH_GetErrorXml");
                command.CommandType = CommandType.StoredProcedure;

                SqlParameterCollection parameters = command.Parameters;
                parameters.Add("@Application", SqlDbType.NVarChar, _maxAppNameLength).Value = appName;
                parameters.Add("@ErrorId", SqlDbType.UniqueIdentifier).Value = id;

                return command;
            }

            public static SqlCommand GetErrorsXml(string appName, int pageIndex, int pageSize)
            {
                SqlCommand command = new SqlCommand("ELMAH_GetErrorsXml");
                command.CommandType = CommandType.StoredProcedure;

                SqlParameterCollection parameters = command.Parameters;

                parameters.Add("@Application", SqlDbType.NVarChar, _maxAppNameLength).Value = appName;
                parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex;
                parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                parameters.Add("@TotalCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                return command;
            }

            public static void GetErrorsXmlOutputs(SqlCommand command, out int totalCount)
            {
                Debug.Assert(command != null);

                totalCount = (int)command.Parameters["@TotalCount"].Value;
            }
        }

        /// <summary>
        /// An <see cref="IAsyncResult"/> implementation that wraps another.
        /// </summary>

        private sealed class AsyncResultWrapper : IAsyncResult
        {
            private readonly IAsyncResult _inner;
            private readonly object _asyncState;

            public AsyncResultWrapper(IAsyncResult inner, object asyncState)
            {
                _inner = inner;
                _asyncState = asyncState;
            }

            public IAsyncResult InnerResult
            {
                get { return _inner; }
            }

            public bool IsCompleted
            {
                get { return _inner.IsCompleted; }
            }

            public WaitHandle AsyncWaitHandle
            {
                get { return _inner.AsyncWaitHandle; }
            }

            public object AsyncState
            {
                get { return _asyncState; }
            }

            public bool CompletedSynchronously
            {
                get { return _inner.CompletedSynchronously; }
            }
        }
    }
}
