using System.Data;

namespace FairPoint.Application.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}