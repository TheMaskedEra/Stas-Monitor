namespace Stas.Monitor.Domains.Repository;

public interface IAlertRepository
{
    //TODO ne pas utiliser ID mais donc measurement ?
    Alert? RetrieveAlert();
}
