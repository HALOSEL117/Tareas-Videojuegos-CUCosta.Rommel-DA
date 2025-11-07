namespace persona;

public class Empleado : Persona
{
    private string puesto;
    private double salario;
    public Empleado(string nom, string apeP, string apeM, int e, string curp, string pue, double sal) : base(nom, apeP, apeM, e, curp)
    {
        puesto = pue;
        salario = sal;
    }
    public string Puesto
    {
        get { return puesto; }
        set { puesto = value; }
    }
    public double Salario
    {
        get { return salario; }
        set { salario = value; }
    }
    public override string ToString()
    {
        return (base.ToString() + ", Puesto: " + Puesto + ", Salario: " + Salario);
    }
}
