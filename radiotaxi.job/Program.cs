// See https://aka.ms/new-console-template for more information
using radiotaxi.Model;

Console.WriteLine("Hello, World!");
try
{
	// ESTE PROGRAMA SE EJECUTA PARA ACUTALIZAR lOS PERMOS DE TODOS LOS OPERADORES Y SE GENERA DE MANERA CONRRECTA.
	C_permissions c_Permissions = new C_permissions();
    c_Permissions.calculate();
    //c_Permissions.generate("OP-58192");
	Console.WriteLine("Hola");
}
catch (Exception ex)
{
	throw;
}