namespace Grupo_9__U20230358
{
    using System;
    using System.Collections.Generic;
    using Spectre.Console;

    class Program
    {
        static List<Usuario> usuarios = new List<Usuario>();
        static List<Pedido> pedidos = new List<Pedido>();
        static List<string> inventario = new List<string> { "Tomates", "Lechuga", "Queso" };

        static void Main(string[] args)
        {
            // Usuarios predefinidos
            usuarios.Add(new Cliente("cliente", "cliente123"));
            usuarios.Add(new Gerente("gerente", "gerente123"));
            usuarios.Add(new Cocinero("cocinero", "cocinero123"));

            AnsiConsole.Markup("[bold yellow]Bienvenido al Sistema de Gestión del Restaurante[/]");

            while (true)
            {
                var email = AnsiConsole.Ask<string>("Email:");
                var password = AnsiConsole.Prompt(new TextPrompt<string>("Contraseña:").Secret());

                Usuario usuario = Login(email, password);

                if (usuario != null)
                {
                    AnsiConsole.Markup("[bold green]Inicio de sesión exitoso![/]");
                    MostrarMenu(usuario);
                }
                else
                {
                    AnsiConsole.Markup("[bold red]Usuario o contraseña incorrectos. Inténtalo de nuevo.[/]");
                }
            }
        }

        static Usuario Login(string email, string password)
        {
            foreach (var usuario in usuarios)
            {
                if (usuario.Email == email && usuario.Password == password)
                {
                    return usuario;
                }
            }
            return null;
        }

        static void MostrarMenu(Usuario usuario)
        {
            while (true)
            {
                if (usuario is Cliente)
                {
                    AnsiConsole.Markup("[bold cyan]Menú de Cliente[/]");
                    var opciones = new List<string> { "Realizar Pedido", "Salir" };
                    var opcion = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Elige una opción:").AddChoices(opciones));

                    if (opcion == "Realizar Pedido")
                    {
                        RealizarPedido(usuario as Cliente);
                    }
                    else if (opcion == "Salir")
                    {
                        break;
                    }
                }
                else if (usuario is Cocinero)
                {
                    AnsiConsole.Markup("[bold cyan]Menú de Cocinero[/]");
                    var opciones = new List<string> { "Marcar Pedido como Listo", "Solicitar Ingredientes", "Salir" };
                    var opcion = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Elige una opción:").AddChoices(opciones));

                    if (opcion == "Marcar Pedido como Listo")
                    {
                        MarcarPedidoListo();
                    }
                    else if (opcion == "Solicitar Ingredientes")
                    {
                        SolicitarIngredientes();
                    }
                    else if (opcion == "Salir")
                    {
                        break;
                    }
                }
                else if (usuario is Gerente)
                {
                    AnsiConsole.Markup("[bold cyan]Menú de Gerente[/]");
                    var opciones = new List<string> { "Ver Clientes", "Gestionar Inventario", "Salir" };
                    var opcion = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Elige una opción:").AddChoices(opciones));

                    if (opcion == "Ver Clientes")
                    {
                        VerClientes();
                    }
                    else if (opcion == "Gestionar Inventario")
                    {
                        GestionarInventario();
                    }
                    else if (opcion == "Salir")
                    {
                        break;
                    }
                }
            }
        }

        static void RealizarPedido(Cliente cliente)
        {
            var platillos = new List<string> { "Hamburguesa", "Ensalada", "Pizza" };
            var platillo = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Elige un platillo:").AddChoices(platillos));

            Pedido nuevoPedido = new Pedido { Cliente = cliente, Platillo = platillo, Estado = "Pendiente" };
            pedidos.Add(nuevoPedido);

            AnsiConsole.Markup("[bold green]Pedido realizado exitosamente![/]");
        }

        static void MarcarPedidoListo()
        {
            if (pedidos.Count == 0)
            {
                AnsiConsole.Markup("[bold red]No hay pedidos pendientes.[/]");
                return;
            }

            var opcionesPedidos = new List<string>();
            foreach (var pedido in pedidos)
            {
                opcionesPedidos.Add($"{pedido.Cliente.Email}: {pedido.Platillo} ({pedido.Estado})");
            }

            var pedidoSeleccionado = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Elige un pedido para marcar como listo:").AddChoices(opcionesPedidos));
            var pedidoParaMarcar = pedidos.Find(p => $"{p.Cliente.Email}: {p.Platillo} ({p.Estado})" == pedidoSeleccionado);
            pedidoParaMarcar.Estado = "Listo";

            AnsiConsole.Markup("[bold green]Pedido marcado como listo![/]");
        }

        static void SolicitarIngredientes()
        {
            var ingrediente = AnsiConsole.Ask<string>("¿Qué ingrediente necesitas?");
            AnsiConsole.Markup($"[bold yellow]Notificación enviada al gerente para comprar {ingrediente}.[/]");
        }

        static void VerClientes()
        {
            var clientes = usuarios.FindAll(u => u is Cliente);
            foreach (var cliente in clientes)
            {
                AnsiConsole.Markup($"[bold blue]{cliente.Email}[/]\n");
            }
        }

        static void GestionarInventario()
        {
            var opciones = new List<string> { "Ver Inventario", "Agregar Ingrediente", "Salir" };
            var opcion = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Elige una opción:").AddChoices(opciones));

            if (opcion == "Ver Inventario")
            {
                foreach (var item in inventario)
                {
                    AnsiConsole.Markup($"[bold green]{item}[/]\n");
                }
            }
            else if (opcion == "Agregar Ingrediente")
            {
                var nuevoIngrediente = AnsiConsole.Ask<string>("Nombre del nuevo ingrediente:");
                inventario.Add(nuevoIngrediente);
                AnsiConsole.Markup("[bold green]Ingrediente agregado al inventario![/]");
            }
            else if (opcion == "Salir")
            {
                return;
            }
        }
    }

}