namespace persona;

public class Persona
{
    private string _Nombre;
    private string _ApePaterno;
    private string _ApeMaterno;
    private int _Edad;
    private string _Curp;
    public Persona(string nombre, string apeP, string apeM, int edad, string curp)
    {
        _Nombre = nombre;
        _ApePaterno = apeP;
        _ApeMaterno = apeM;
        _Edad = edad;
        _Curp = curp;
    }
    public string Nombre
    {
        get { return _Nombre; }
        set { _Nombre = value; }
    }

    public string ApePaterno
    {
        get { return _ApePaterno; }
        set { _ApePaterno = value; }
    }

    public string ApeMaterno
    {
        get { return _ApeMaterno; }
        set { _ApeMaterno = value; }
    }

    public string Curp
    {
        get { return _Curp; }
        set { _Curp = value; }
    }
    public int Edad
    {
        get { return _Edad; }
        set { _Edad = value; }
    }
    public override string ToString()
    {
        return $"{Nombre} {ApePaterno} {ApeMaterno} {Edad} {Curp}";
    }
}