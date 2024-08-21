using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using LALoDep.Domain;

/// <summary>
/// Summary description for DbManager
/// </summary>
public sealed class DbManager : IDisposable
{
    // Fields
    private SqlCommand Command;
    private SqlDataAdapter CommandAdapter;
    private SqlParameter CommandParameter;
    private DataSet dsResults;
    private CommandType DefaultCommandType = CommandType.StoredProcedure;
    // Methods
 
    public DbManager(int timeout = 0)
    {
        this.Command = new SqlCommand();
        if (timeout > 0)
            this.Command.CommandTimeout = timeout;
        this.Command.CommandType = DefaultCommandType;
        this.Command.Connection = this.EstablishConnection();

    }
    public DbManager(string connectionString)
    {
        this.Command = new SqlCommand();
        this.Command.CommandType = DefaultCommandType;
        this.Command.Connection = this.EstablishConnection(connectionString);
    }

    public void AddInParam(string parameterName, object parameterValue)
    {
        this.Command.Parameters.AddWithValue(parameterName, parameterValue);
    }

    public void AddInParam(string parameterName, object parameterValue, SqlDbType parameterDbType)
    {
        this.CommandParameter = new SqlParameter();
        this.CommandParameter.ParameterName = parameterName;
        this.CommandParameter.SqlDbType = parameterDbType;
        this.CommandParameter.Value = parameterValue;
        this.Command.Parameters.Add(this.CommandParameter);
    }

    public void AddInParam(string parameterName, SqlDbType parameterDbType, int size, string sourceColumn)
    {
        this.Command.Parameters.Add(new SqlParameter(parameterName, parameterDbType, size, sourceColumn));
    }

    public void AddInParam(string parameterName, object parameterValue, SqlDbType parameterDbType, ParameterDirection parameterDirection)
    {
        this.CommandParameter = new SqlParameter();
        this.CommandParameter.ParameterName = parameterName;
        this.CommandParameter.Value = parameterValue;
        this.CommandParameter.Direction = parameterDirection;
        this.CommandParameter.SqlDbType = parameterDbType;
        this.Command.Parameters.Add(this.CommandParameter);
    }

    public void AddInParam(string parameterName, object parameterValue, SqlDbType parameterDbType, int size)
    {
        this.CommandParameter = new SqlParameter();
        this.CommandParameter.ParameterName = parameterName;
        this.CommandParameter.SqlDbType = parameterDbType;
        this.CommandParameter.Size = size;
        this.CommandParameter.Value = parameterValue;
        this.Command.Parameters.Add(this.CommandParameter);
    }

    public void AddInParam(string parameterName, object parameterValue, SqlDbType parameterDbType, int size, ParameterDirection parameterDirection)
    {
        this.CommandParameter = new SqlParameter();
        this.CommandParameter.ParameterName = parameterName;
        this.CommandParameter.Value = parameterValue;
        this.CommandParameter.Size = size;
        this.CommandParameter.Direction = parameterDirection;
        this.CommandParameter.SqlDbType = parameterDbType;
        this.Command.Parameters.Add(this.CommandParameter);
    }

    public void AddOutParam(string parameterName, SqlDbType parameterDbType)
    {
        this.CommandParameter = new SqlParameter();
        this.CommandParameter.ParameterName = parameterName;
        this.CommandParameter.SqlDbType = parameterDbType;
        this.CommandParameter.Direction = ParameterDirection.Output;
        this.Command.Parameters.Add(this.CommandParameter);
    }

    public void AddOutParam(string parameterName, object parameterValue, SqlDbType parameterDbType)
    {
        this.CommandParameter = new SqlParameter();
        this.CommandParameter.ParameterName = parameterName;
        this.CommandParameter.SqlDbType = parameterDbType;
        this.CommandParameter.Value = parameterValue;
        this.CommandParameter.Direction = ParameterDirection.Output;
        this.Command.Parameters.Add(this.CommandParameter);
    }

    public void AddOutParam(string parameterName, object parameterValue, SqlDbType parameterDbType, int size)
    {
        this.CommandParameter = new SqlParameter();
        this.CommandParameter.ParameterName = parameterName;
        this.CommandParameter.SqlDbType = parameterDbType;
        this.CommandParameter.Size = size;
        this.CommandParameter.Direction = ParameterDirection.Output;
        this.CommandParameter.Value = parameterValue;
        this.Command.Parameters.Add(this.CommandParameter);
    }

    public void CloseConnection()
    {
        if ((this.Command.Connection.State != ConnectionState.Closed) && (this.Command.Transaction == null))
        {
            this.Command.Connection.Close();
        }
    }

    public SqlConnection EstablishConnection()
    {
        return new SqlConnection { ConnectionString = LALoDepEntities.GetSqlConnectionStringForDbManager() };
    }
    public SqlConnection EstablishConnection(string connectionString)
    {
        return new SqlConnection { ConnectionString = connectionString };
    }
    public DataSet ExecuteDataSet(string commandText)
    {
        return ExecuteDataSet(commandText, this.DefaultCommandType);
    }

    public DataSet ExecuteDataSet(string commandText, CommandType commandType)
    {
        this.dsResults = new DataSet();
        this.CommandAdapter = new SqlDataAdapter();
        this.Command.CommandText = commandText;
        this.Command.CommandType = commandType;
        this.CommandAdapter.SelectCommand = this.Command;
        try
        {
            this.OpenConnection();
            this.CommandAdapter.Fill(this.dsResults);
        }
        catch (Exception exception)
        {
            throw exception;
        }
        finally
        {
            this.CloseConnection();
        }
        return this.dsResults;
    }

    public DataTable ExecuteDataTable(string commandText)
    {
        return this.ExecuteDataTable(commandText, this.DefaultCommandType);
    }

    public DataTable ExecuteDataTable(string commandText, CommandType commandType)
    {
        DataSet dsResult = this.ExecuteDataSet(commandText, commandType);
        DataTable dtResult = null;

        if (dsResult != null)
        {
            if (dsResult.Tables.Count >= 1)
            {
                dtResult = dsResult.Tables[0];
            }
        }
        return dtResult;
    }

    public int ExecuteNonQuery(string commandText)
    {
        return this.ExecuteNonQuery(commandText, this.DefaultCommandType);
    }

    public int ExecuteNonQuery(string commandText, CommandType commandType)
    {
        int num;
        this.Command.CommandType = commandType;
        this.Command.CommandText = commandText;
        try
        {
            this.OpenConnection();
            num = this.Command.ExecuteNonQuery();
        }
        catch (Exception exception)
        {
            throw exception;
        }
        finally
        {
            this.CloseConnection();
        }
        return num;
    }

    public SqlDataReader ExecuteReader(string commandText)
    {
        return this.ExecuteReader(commandText, this.DefaultCommandType);
    }

    public SqlDataReader ExecuteReader(string commandText, CommandType commandType)
    {
        this.Command.CommandText = commandText;
        this.Command.CommandType = commandType;
        this.OpenConnection();
        return this.Command.ExecuteReader(CommandBehavior.CloseConnection);
    }

    public object ExecuteScalar(string commandText)
    {
        return this.ExecuteScalar(commandText, this.DefaultCommandType);
    }

    public object ExecuteScalar(string commandText, CommandType commandType)
    {
        object obj2;
        this.Command.CommandText = commandText;
        this.Command.CommandType = commandType;
        try
        {
            this.OpenConnection();
            obj2 = this.Command.ExecuteScalar();
        }
        catch (Exception exception)
        {
            throw exception;
        }
        finally
        {
            this.CloseConnection();
        }
        return obj2;
    }

    public SqlDataReader ExecuteSingleRowReader(string commandText)
    {
        return this.ExecuteSingleRowReader(commandText, this.DefaultCommandType);
    }

    public SqlDataReader ExecuteSingleRowReader(string commandText, CommandType commandType)
    {
        this.Command.CommandText = commandText;
        this.Command.CommandType = commandType;
        this.OpenConnection();
        return this.Command.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleRow);
    }

    public void InsertDataTable(string commandText, DataTable dtRecords)
    {
        this.dsResults = new DataSet();
        this.CommandAdapter = new SqlDataAdapter();
        this.InsertCommand.CommandText = commandText;
        this.CommandAdapter.InsertCommand = this.InsertCommand;
        try
        {
            this.OpenConnection();
            this.CommandAdapter.Update(dtRecords);
        }
        catch (Exception exception)
        {
            throw exception;
        }
        finally
        {
            this.CloseConnection();
        }
    }

    public void OpenConnection()
    {
        if (this.Command.Connection.State != ConnectionState.Open)
        {
            this.Command.Connection.Open();
        }
    }

    bool isDisposed = false;

    public void Dispose(bool isDisposing)
    {
        if (!isDisposed)
        {
            if (isDisposing)
            {
                CloseConnection();
                this.Command = null;
                this.CommandAdapter = null;
                this.dsResults = null;
            }
        }
        isDisposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Properties
    public SqlCommand InsertCommand { get; set; }

    public SqlParameterCollection Parameters
    {
        get
        {
            return this.Command.Parameters;
        }
    }

    public SqlCommand UpdateCommand { get; set; }
}
