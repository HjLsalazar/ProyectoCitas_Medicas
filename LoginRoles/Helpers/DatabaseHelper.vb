
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Namespace Helpers
    Public Class DatabaseHelper
        Private ReadOnly _connectionString As String =
            ConfigurationManager.ConnectionStrings("Login").ConnectionString

        ' Crea y abre la conexión 
        Public Function GetConnection() As SqlConnection
            Dim cn As New SqlConnection(_connectionString)
            cn.Open()
            Return cn
        End Function

        ' Crea un parámetro SQL
        Public Function CreateParameter(name As String, value As Object) As SqlParameter
            Dim p As New SqlParameter(name, If(value Is Nothing, DBNull.Value, value))
            Return p
        End Function

        ' SELECT → DataTable
        Public Function ExecuteQuery(sql As String,
                                     Optional parameters As List(Of SqlParameter) = Nothing,
                                     Optional isStoredProcedure As Boolean = False) As DataTable
            Using cn = GetConnection()
                Using cmd As New SqlCommand(sql, cn)
                    cmd.CommandType = If(isStoredProcedure, CommandType.StoredProcedure, CommandType.Text)
                    If parameters IsNot Nothing Then cmd.Parameters.AddRange(parameters.ToArray())
                    Using da As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

        ' INSERT, UPDATE, DELETE
        Public Function ExecuteNonQuery(sql As String,
                                        Optional parameters As List(Of SqlParameter) = Nothing,
                                        Optional isStoredProcedure As Boolean = False) As Boolean
            Using cn = GetConnection()
                Using cmd As New SqlCommand(sql, cn)
                    cmd.CommandType = If(isStoredProcedure, CommandType.StoredProcedure, CommandType.Text)
                    If parameters IsNot Nothing Then cmd.Parameters.AddRange(parameters.ToArray())
                    Return cmd.ExecuteNonQuery() > 0
                End Using
            End Using
        End Function

        ' SELECT que devuelve un solo valor
        Public Function ExecuteScalar(Of T)(sql As String,
                                            Optional parameters As List(Of SqlParameter) = Nothing) As T
            Using cn = GetConnection()
                Using cmd As New SqlCommand(sql, cn)
                    If parameters IsNot Nothing Then cmd.Parameters.AddRange(parameters.ToArray())
                    Dim obj = cmd.ExecuteScalar()
                    If obj Is Nothing OrElse obj Is DBNull.Value Then Return Nothing
                    Return CType(Convert.ChangeType(obj, GetType(T)), T)
                End Using
            End Using
        End Function
    End Class
End Namespace
