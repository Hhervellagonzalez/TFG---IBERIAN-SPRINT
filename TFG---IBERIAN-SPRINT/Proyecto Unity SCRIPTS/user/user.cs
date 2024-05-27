using System.Collections.Generic;

[System.Serializable]
public class User
{
    public string username;
    public string password;
    public string logros;
    public int monedas;
    public List<string> aspectos;
    public float recordDistancia;
    public Dictionary<string, bool> logrosDict;

    public User(string username, string password, string logros, int monedas, List<string> aspectos, float recordDistancia, Dictionary<string, bool> logrosDict)
    {
        this.username = username;
        this.password = password;
        this.logros = logros;
        this.monedas = monedas;
        this.aspectos = aspectos;
        this.recordDistancia = recordDistancia;
        this.logrosDict = logrosDict;
    }
}

